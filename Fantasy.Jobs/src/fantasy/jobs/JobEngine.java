package fantasy.jobs;

import fantasy.jobs.Management.*;
import fantasy.xserialization.*;
import fantasy.jobs.Resources.*;
import fantasy.servicemodel.*;

public class JobEngine extends MarshalByRefObject implements IJobEngine
{
	public JobEngine(Guid id)
	{
		_currentEngine = this;
		this.setJobId(id);
	}

	private ServiceContainer _serviceContainer = new ServiceContainer();

	private Job _job;

	@Override
	public Object InitializeLifetimeService()
	{
		//Never expired;
		return null;
	}



	private ManualResetEvent _waitEvent = new ManualResetEvent(false);
	private IJobManager _jobManager;

	private Thread _workThread = null;

	private java.util.ArrayList<IJobEngineEventHandler> _eventHandlers = new java.util.ArrayList<IJobEngineEventHandler>();

	private static IJobEngine _currentEngine = null;

	public static IJobEngine getCurrentEngine()
	{
		return _currentEngine;
	}

	public final void Initialize()
	{

		_jobManager = new JobManagerAccessor().GetJobManager();

		IJobManagerSettingsReader reader = (IJobManagerSettingsReader)_jobManager.GetService(IJobManagerSettingsReader.class);
		_jobDirectory = String.format(String.format("%1$s\\%2$s", reader.<String>GetSetting("JobDirectoryFullPath"), this.getJobId()));
		if (!Directory.Exists(_jobDirectory))
		{
			Directory.CreateDirectory(_jobDirectory);
		}

		ILogger logger = _jobManager.<ILogger>GetService();
		try
		{
			//Initialize Services from app.config
			java.util.ArrayList<Object> services = new java.util.ArrayList<Object>();
			services.addAll(AddIn.CreateObjects("jobEngine/engine.services/service"));
			services.addAll(AddIn.CreateObjects("jobEngine/job.services/service"));
			services.add(this);
			this._serviceContainer.InitializeServices(_jobManager, services.toArray(new Object[]{}));
			//Register to Manager
			IJobController controller = (IJobController)_jobManager.GetService(IJobController.class);
			controller.RegisterJobEngine(this);
		}
		catch(RuntimeException error)
		{
			if (logger != null)
			{
				logger.LogError("Engine", error,"Job {0} initialze failed", this.getJobId());
			}
			throw error;
		}
	}



	private void FireEvent(MethodInvoker<IJobEngineEventHandler> method)
	{
		java.util.ArrayList<IJobEngineEventHandler> expired = new java.util.ArrayList<IJobEngineEventHandler>();
		// foreach(IJobEngineEventHandler handler in this._eventHandlers) 
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Parallel.ForEach(this._eventHandlers.toArray(new IJobEngineEventHandler[]{}), handler =>
		{
			try
			{
				method(handler);
			}
			catch (java.lang.Exception e)
			{
				_eventHandlers.remove(handler);
			}
		}
	   );


	}

	public final void WaitForExit()
	{
		_waitEvent.WaitOne();
		_serviceContainer.UninitializeServices();
	}

	private void OnExit(int exitCode)
	{
		try
		{
			JobExitEventArgs e = new JobExitEventArgs(exitCode);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			FireEvent(delegate(IJobEngineEventHandler handler)
			{
				handler.HandleExit(this, e);
			}
		   );
		}
		catch (ThreadAbortException e)
		{
		}
		catch (RuntimeException error)
		{
			ILogger logger = this.<ILogger>GetService();
			if (logger != null)
			{
				logger.LogError(LogCategories.getEngine(), error, "A error occurs when job is exiting.");
			}
		}

		_waitEvent.Set();
	}

	private int _abortState = JobState.Suspended;

	public final void Start(JobStartInfo startInfo)
	{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		ThreadStart del = delegate()
		{

			int exitState = JobState.Failed;
			ILogger logger = (ILogger)this.GetService(ILogger.class);
			try
			{




				if (logger != null)
				{
					logger.LogMessage(LogCategories.getEngine(), "Start");

				}

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				FireEvent(delegate(IJobEngineEventHandler handler)
				{
					handler.HandleStart(this);
				}
			   );

				this._job = new Job();
				this._job._id = this.getJobId();

				this._serviceContainer.AddService(_job);
				_job.Initialize();

				_job.LoadStartInfo((JobStartInfo)startInfo);

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				FireEvent(delegate(IJobEngineEventHandler handler)
				{
					handler.HandleLoad(this);
				}
			   );
				this.SaveStatus();

				((IJob)_job).Execute();
				exitState = JobState.Succeed;
				this.SaveStatus();
			}

			catch (ThreadAbortException e)
			{
				exitState = _abortState;
			}
			catch (RuntimeException error)
			{
				if (logger != null)
				{
					logger.LogError(LogCategories.getEngine(), error, "A fatal error occurs, exit excuting.");
					if (error instanceof ReflectionTypeLoadException)
					{
						for (RuntimeException child : ((ReflectionTypeLoadException)error).LoaderExceptions)
						{
							logger.LogMessage(LogCategories.getEngine(), "Load exception.");
							logger.LogError(LogCategories.getEngine(), child, "Load exception.");
						}
					}
				}
			}
			finally
			{
				if (logger != null)
				{
					logger.LogMessage(LogCategories.getEngine(), "Exit with code {0}.", JobState.ToString(exitState));
				}
				this.OnExit(exitState);
			}
		}

		this.StartWorkThread(del);
	}



	public final void Resume(JobStartInfo startInfo)
	{

		IJobStatusStorageService storage = this.<IJobStatusStorageService>GetService();
		if (storage.getExists())
		{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			ThreadStart del = delegate()
			{
				int exitState = JobState.Failed;


				ILogger logger = (ILogger)this.GetService(ILogger.class);
				try
				{

					if (logger != null)
					{
						logger.LogMessage(LogCategories.getEngine(), "Resume");
					}

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					FireEvent(delegate(IJobEngineEventHandler handler)
					{
						handler.HandleResume(this);
					}
				   );

					this._job = new Job();

					this._serviceContainer.AddService(_job);
					_job.Initialize();
					Stream stream = this.<IJobStatusStorageService>GetRequiredService().Load();
					XElement ele = XElement.Load(stream);
					_job.LoadStatus(ele);

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					FireEvent(delegate(IJobEngineEventHandler handler)
					{
						handler.HandleLoad(this);
					}
				   );

					((IJob)_job).Execute();
					exitState = JobState.Succeed;
					this.SaveStatus();
				}
				catch (ThreadAbortException e)
				{
					exitState = _abortState;
				}
				catch (RuntimeException error)
				{

					if (logger != null)
					{
						logger.LogError(LogCategories.getEngine(), error, "A fatal error occurs, exit excuting.");
					}
				}
				finally
				{
					if (logger != null)
					{
						logger.LogMessage(LogCategories.getEngine(), "Exit with code {0}.", JobState.ToString(exitState));
					}
					this.OnExit(exitState);
				}
			}

			this.StartWorkThread(del);
		}
		else
		{
			this.Start(startInfo);
		}
	}

	private void ResumeEntryPoint(Object resumeInfo)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		FireEvent(delegate(IJobEngineEventHandler handler)
		{
			handler.HandleResume(this);
		}
	   );
		this.OnExit(JobState.Succeed);
	}

	public final void Terminate()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.Cancelled;
			this._workThread.stop();
		}
	}

	public final void Suspend()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.Suspended;
			this._workThread.stop();
		}
	}

	public final void UserPause()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.UserPaused;
			this._workThread.stop();
		}
	}

	public final void Fail()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.Failed;
			this._workThread.stop();
		}
	}

	public final void Sleep(TimeSpan timeout)
	{
		java.util.Date timeToWait = new java.util.Date() + timeout;
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		ResourceParameter res = new ResourceParameter("WaitTime", new { time = timeToWait });
		IResourceService resSvc = this.<IResourceService>GetService();
		if (resSvc != null)
		{
			IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res });
			if (handle != null)
			{
				handle.dispose();
			}

		}
	}

	private void StartWorkThread(ThreadStart start)
	{
		this._workThread = ThreadFactory.CreateThread(start).WithStart();


	}

	public final void AddHandler(IJobEngineEventHandler handler)
	{
		this._eventHandlers.add(handler);
	}

	public final void RemoveHandler(IJobEngineEventHandler handler)
	{
		this._eventHandlers.remove(handler);
	}

	public final Object GetService(java.lang.Class serviceType)
	{
		return _serviceContainer.GetService(serviceType);
	}

	private Guid privateJobId = new Guid();
	public final Guid getJobId()
	{
		return privateJobId;
	}
	private void setJobId(Guid value)
	{
		privateJobId = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEngine Members


	private String _jobDirectory = null;
	public final String getJobDirectory()
	{
		if (_jobDirectory == null)
		{

		}
		return _jobDirectory;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	private RuntimeException _executingError;
	private boolean _abortingSaved = false;

	public final void SaveStatus()
	{
		XElement ele = this._job.SaveStatus();
		MemoryStream stream = new MemoryStream();
		XmlWriterSettings tempVar = new XmlWriterSettings();
		tempVar.Encoding = Encoding.UTF8;
		tempVar.Indent = true;
		tempVar.IndentChars = "    ";
		tempVar.NamespaceHandling = NamespaceHandling.OmitDuplicates;
		XmlWriterSettings settings = tempVar;
		XmlWriter writer = XmlWriter.Create(stream, settings);
		ele.Save(writer);
		writer.Close();
		stream.Seek(0, SeekOrigin.Begin);
		this.<IJobStatusStorageService>GetRequiredService().Save(stream);

	}

	public final void SaveStatusForError(RuntimeException error)
	{

		if (_job != null)
		{
			if (error instanceof ThreadAbortException)
			{
				if (!_abortingSaved)
				{
					_abortingSaved = true;
					if (!((IJob)this._job).getRuntimeStatus().IsRestoring)
					{
						this.SaveStatus();
					}
				}
			}
			else if (this._executingError != error)
			{

				_executingError = error;
				if (!((IJob)this._job).getRuntimeStatus().IsRestoring)
				{
					this.SaveStatus();
				}

			}
		}

	}

}