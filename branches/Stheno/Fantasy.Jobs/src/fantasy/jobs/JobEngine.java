package fantasy.jobs;

import java.rmi.server.UnicastRemoteObject;
import java.util.*;
import java.util.concurrent.*;

import org.jdom2.Element;
import org.joda.time.*;

import fantasy.*;
import fantasy.ThreadFactory;
import fantasy.jobs.management.*;
import fantasy.xserialization.*;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import fantasy.io.*;

public class JobEngine extends UnicastRemoteObject implements IJobEngine
{
	public JobEngine(UUID id)
	{
		_currentEngine = this;
		this.setJobId(id);
	}

	private ServiceContainer _serviceContainer = new ServiceContainer();

	private Job _job;

	



	private Object _waitEvent = new Object();
	private IJobManager _jobManager;

	private Thread _workThread = null;

	private java.util.HashSet<IJobEngineEventHandler> _eventHandlers = new java.util.HashSet<IJobEngineEventHandler>();

	private static IJobEngine _currentEngine = null;

	public static IJobEngine getCurrentEngine()
	{
		return _currentEngine;
	}

	public final void Initialize()
	{

		_jobManager = new JobManagerAccessor().GetJobManager();

		IJobManagerSettingsReader reader = (IJobManagerSettingsReader)_jobManager.getService(IJobManagerSettingsReader.class);
		_jobDirectory = String.format(String.format("%1$s\\%2$s", reader.GetSetting(String.class, "JobDirectoryFullPath"), this.getJobId()));
		if (!Directory.exists(_jobDirectory))
		{
			Directory.create(_jobDirectory);
		}

		ILogger logger = _jobManager.getService(ILogger.class);
		try
		{
			//Initialize Services from app.config
			java.util.ArrayList<Object> services = new java.util.ArrayList<Object>();
			services.addAll(Arrays.asList(AddIn.CreateObjects("jobEngine/engine.services/service")));
			services.addAll(Arrays.asList(AddIn.CreateObjects("jobEngine/job.services/service")));
			services.add(this);
			this._serviceContainer.initializeServices(_jobManager, services.toArray(new Object[]{}));
			//Register to Manager
			IJobController controller = (IJobController)_jobManager.getService(IJobController.class);
			controller.RegisterJobEngine(this);
		}
		catch(Exception error)
		{
			if (logger != null)
			{
				logger.LogError("Engine", error,"Job {0} initialze failed", this.getJobId());
			}
			throw error;
		}
	}



	private void FireEvent(final MethodInvoker<IJobEngineEventHandler> method)
	{
		java.util.ArrayList<IJobEngineEventHandler> expired = new java.util.ArrayList<IJobEngineEventHandler>();

		IJobEngineEventHandler[] handlers;
		synchronized(this._eventHandlers)
		{
			handlers = this._eventHandlers.toArray(new IJobEngineEventHandler[0]);
		}

		ExecutorService exec = Executors.newCachedThreadPool();
		try
		{
			for(final IJobEngineEventHandler handler : handlers)
			{
				exec.execute(new Runnable(){

					@Override
					public void run() {
						try
						{
							method.invoke(handler);
						}
						catch (java.lang.Exception e)
						{
							synchronized(JobEngine.this._eventHandlers)
							{
								_eventHandlers.remove(handler);
							}
						}
					}});

			}
			exec.wait();
		}
		finally
		{
			exec.shutdown();
		}





	}

	public final void WaitForExit()
	{
		_waitEvent.wait();
		_serviceContainer.uninitializeServices();
	}

	private void OnExit(int exitCode)
	{
		try
		{
			final JobExitEventArgs e = new JobExitEventArgs(exitCode);
			FireEvent(new MethodInvoker<IJobEngineEventHandler>(){

				@Override
				public void invoke(IJobEngineEventHandler obj) throws Exception {
					obj.HandleExit(JobEngine.this, e);
					
				}});
			
		  
		}
		catch (InterruptedException e)
		{
			throw e;
		}
		catch (Exception error)
		{
			ILogger logger = this.getService(ILogger.class);
			if (logger != null)
			{
				logger.LogError(LogCategories.getEngine(), error, "A error occurs when job is exiting.");
			}
		}

		_waitEvent.notifyAll();
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
			this._workThread.interrupt();
		}
	}

	public final void Suspend()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.Suspended;
			this._workThread.interrupt();
		}
	}

	public final void UserPause()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.UserPaused;
			this._workThread.interrupt();
		}
	}

	public final void Fail()
	{
		if (this._workThread != null && this._workThread.isAlive())
		{
			this._abortState = JobState.Failed;
			this._workThread.interrupt();
		}
	}

	public final void Sleep(Duration timeout)
	{
		final java.util.Date timeToWait =   Instant.now().plus(timeout).toDate();
		ResourceParameter res = new ResourceParameter(new Resource(){String name = "WaitTime"; Date time = timeToWait;});
		IResourceService resSvc = this.getService(IResourceService.class);
		if (resSvc != null)
		{
			IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res });
			
			handle.dispose();
			

		}
	}

	private void StartWorkThread(Runnable start)
	{
		this._workThread = ThreadFactory.createAndStart(start);


	}

	public final void AddHandler(IJobEngineEventHandler handler)
	{
		this._eventHandlers.add(handler);
	}

	public final void RemoveHandler(IJobEngineEventHandler handler)
	{
		this._eventHandlers.remove(handler);
	}

	public final<T> T GetService(java.lang.Class<T> serviceType) throws Exception
	{
		return _serviceContainer.getService(serviceType);
	}

	private UUID privateJobId = UUID.randomUUID();
	public final UUID getJobId()
	{
		return privateJobId;
	}
	private void setJobId(UUID value)
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

	private RuntimeException _executingError;
	private boolean _abortingSaved = false;

	public final void SaveStatus()
	{
		Element ele = this._job.SaveStatus();
		
		this.getRequiredService(IJobStatusStorageService.class).Save();

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