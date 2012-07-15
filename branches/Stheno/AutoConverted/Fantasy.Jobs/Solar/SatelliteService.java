package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Management.*;
import Fantasy.Jobs.Resources.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
public class SatelliteService extends AbstractService implements ISatellite
{

	private ClientRef<ISatelliteHandler> _satelliteHandler;

	private IJobController _controller;

	private String _name;

	private Thread _refreshThread;

	private IComputerLoadFactorEvaluator _loadFactorEvaluator;
	private IResourceManager _resourceManager;

	@Override
	public void InitializeService()
	{
		this._controller = this.Site.<IJobController>GetRequiredService();
		_name = Environment.MachineName + ":" + Process.GetCurrentProcess().Id;

		this._loadFactorEvaluator = AddIn.<IComputerLoadFactorEvaluator>CreateObjects("jobService/computerLoadFactorEvaluator").SingleOrDefault();

		if (this._loadFactorEvaluator != null && this._loadFactorEvaluator instanceof IObjectWithSite)
		{
			((IObjectWithSite)_loadFactorEvaluator).Site = this.Site;
		}

		this._resourceManager = this.Site.<IResourceManager>GetService();
		if (_resourceManager != null)
		{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			this._resourceManager.Available += new EventHandler(ResourceManager_Available);
		}

		_refreshThread = ThreadFactory.CreateThread(this.Refresh).WithStart();


		super.InitializeService();
	}

	private void ResourceManager_Available(Object sender, EventArgs e)
	{
		ISolarActionQueue actionQueue = this.Site.<ISolarActionQueue>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		actionQueue.Enqueue(solar => solar.ResourceAvaiable());
	}

	private void Refresh()
	{
		while (true)
		{
			TryCreateHandler();
			Thread.sleep(15 * 1000);
		}

	}

	private void TryCreateHandler()
	{

		ILogger logger = this.Site.<ILogger>GetService();

		if (this._satelliteHandler != null)
		{
			try
			{
				this._satelliteHandler.Client.Echo();
			}
			catch (ThreadAbortException e)
			{
			}
			catch (RuntimeException error)
			{
				_satelliteHandler.dispose();
				_satelliteHandler = null;
				if (!WCFExceptionHandler.CanCatch(error))
				{
					throw error;
				}
				else
				{
					logger.SafeLogError("Solar", error, "WCF error");
				}

			}
		}

		if (_satelliteHandler == null)
		{

			try
			{
				_satelliteHandler = ClientFactory.<ISatelliteHandler>CreateDuplex(this);
				_satelliteHandler.Client.Connect(this._name);

				logger.SafeLogMessage("Satellite", "Success connect to solar service.");

			}
			catch (ThreadAbortException e2)
			{
			}
			catch (RuntimeException error)
			{

				logger.SafeLogError("Solar", error, "WCF error");
				logger.SafeLogWarning("Satellite", error, MessageImportance.Normal, "Failed to connect to solor service.");
				_satelliteHandler.dispose();
				_satelliteHandler = null;

				if (!WCFExceptionHandler.CanCatch(error))
				{
					throw error;
				}

			}
		}
	}




	@Override
	public void UninitializeService()
	{
		this._refreshThread.stop();
		this._refreshThread.join();
		if (this._satelliteHandler != null)
		{

			try
			{
				this._satelliteHandler.Client.Disconnect();
			}
			catch (java.lang.Exception e)
			{

			}

			this._satelliteHandler.dispose();
		}

		super.UninitializeService();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ISatellite Members

	public final void Echo()
	{

	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> JobChanged;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> JobAdded;

	public final void OnJobAdded(JobMetaData job)
	{
		if (this.JobAdded != null)
		{
			this.JobAdded(this, new JobQueueEventArgs(job));
		}
	}

	public final void OnJobChanged(JobMetaData job)
	{
		if (this.JobChanged != null)
		{
			this.JobChanged(this, new JobQueueEventArgs(job));
		}
	}

	public final boolean IsResourceAvailable(ResourceParameter[] parameters)
	{

		if (_resourceManager != null)
		{
			return _resourceManager.IsAvailable(parameters);
		}
		else
		{
			return true;
		}
	}

	public final double GetLoadFactor()
	{
		return this._loadFactorEvaluator != null this._loadFactorEvaluator.Evaluate() :this._controller.GetAvailableProcessCount();
	}

	public final void RequestStartJob(JobMetaData job)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Start job {0} ({1}).", job.getName(), job.getId());
		this._controller.StartJob(job);
	}

	public final void RequestResume(JobMetaData job)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Resume job {0} ({1}).", job.getName(), job.getId());
		this._controller.Resume(job);
	}

	public final void RequestCancel(Guid id)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Cancel job {0}.", id);
		this._controller.Cancel(id);
	}

	public final void RequestSuspend(Guid id)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Suspend job {0}.", id);
		this._controller.Suspend(id);
	}

	public final void RequestUserPause(Guid id)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Pause job {0}.", id);
		this._controller.UserPause(id);
	}



	public final void RequestSuspendAll()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		logger.SafeLogMessage("Satellite", "Suspend all running jobs.");
		_controller.SuspendAll(true);
	}


	public final boolean IsRunning(Guid id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return _controller.GetRunningJobs().Any(job => id.equals(job.Id));
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}