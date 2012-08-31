package fantasy.jobs;

import java.io.*;
import java.rmi.server.UnicastRemoteObject;
import java.text.SimpleDateFormat;
import java.util.*;
import java.util.concurrent.*;

import org.jdom2.Element;


import fantasy.*;
import fantasy.ThreadFactory;
import fantasy.jobs.management.*;
import fantasy.jobs.properties.Resources;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import fantasy.io.*;


@SuppressWarnings("rawtypes") 
public class JobEngine extends UnicastRemoteObject implements IJobEngine
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 3597754801453285429L;



	public JobEngine(UUID id) throws Exception
	{
		this.setJobId(id);
	}

	private ServiceContainer _serviceContainer = new ServiceContainer();

	private Job _job;

	
	private Object _waitEvent = new Object();
	private IJobManager _jobManager;

	private Thread _workThread = null;

	private java.util.HashSet<IJobEngineEventHandler> _eventHandlers = new java.util.HashSet<IJobEngineEventHandler>();

	

	public final void Initialize() throws Exception
	{

		_jobManager = new JobManagerAccessor().GetJobManager();

		IJobManagerSettingsReader reader = (IJobManagerSettingsReader)_jobManager.getService(IJobManagerSettingsReader.class);
		_jobDirectory = String.format(String.format("%1$s\\%2$s", reader.getSetting(String.class, "JobDirectoryFullPath"), this.getJobId()));
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
				logger.LogError(LogCategories.getEngine(), error, Resources.getJobInitializeFailedMessage(), this.getJobId());
			}
			throw error;
		}
	}



	private void FireEvent(final Action1<IJobEngineEventHandler> method) throws Exception
	{
		

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
							method.call(handler);
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
			
		}
		finally
		{
			exec.shutdown();
			exec.awaitTermination(Long.MAX_VALUE, TimeUnit.NANOSECONDS);
		}





	}

	public final void WaitForExit() throws Exception
	{
		synchronized(this._waitEvent)
		{
		    _waitEvent.wait();
		}
		_serviceContainer.uninitializeServices();
	}

	private void OnExit(int exitCode) throws Exception
	{
		try
		{
			final JobExitEventArgs e = new JobExitEventArgs(exitCode);
			FireEvent(new Action1<IJobEngineEventHandler>(){

				@Override
				public void call(IJobEngineEventHandler obj) throws Exception {
					obj.HandleExit(JobEngine.this, e);
					
				}});
			
		  
		}
		catch (InterruptedException e)
		{
			
		}
		catch (Exception error)
		{
			ILogger logger = this.getService(ILogger.class);
			if (logger != null)
			{
				logger.LogError(LogCategories.getEngine(), error, Resources.getJobExitErrorMessage());
			}
		}

		synchronized(this._waitEvent)
		{
		    _waitEvent.notifyAll();
		}
	}

	private int _abortState = JobState.Suspended;

	
	
	
	public final void Start(final JobStartInfo startInfo)
	{

		Runnable del = new Runnable(){public void run()
		{

			int exitState = JobState.Failed;
			
			ILogger logger = null;
			try {
				logger = (ILogger)JobEngine.this.getService(ILogger.class);
			} catch (Exception e1) {
			
				e1.printStackTrace();
			}
			try
			{


              

				if (logger != null)
				{
					logger.LogMessage(LogCategories.getEngine(), Resources.getStartMessage());

				}

				FireEvent(new Action1<IJobEngineEventHandler>(){
					
					@Override
					public void call(IJobEngineEventHandler handler) throws Exception
					{
						handler.HandleStart(JobEngine.this);
					}
				});
				

				JobEngine.this._job = new Job();
				JobEngine.this._job._id = JobEngine.this.getJobId();

				JobEngine.this._serviceContainer.AddService(_job);
				_job.Initialize();

				_job.LoadStartInfo((JobStartInfo)startInfo);

               FireEvent(new Action1<IJobEngineEventHandler>(){
					
					@Override
					public void call(IJobEngineEventHandler handler) throws Exception
					{
						handler.HandleLoad(JobEngine.this);
					}
				});
               JobEngine.this.SaveStatus();

				((IJob)_job).Execute();
				exitState = JobState.Succeed;
				JobEngine.this.SaveStatus();
			}

			catch (InterruptedException e)
			{
				exitState = _abortState;
			}
			catch (Exception error)
			{
				if (logger != null)
				{
					logger.LogError(LogCategories.getEngine(), error, Resources.getJobFatalErrorMessage());
					
				}
			}
			finally
			{
				if (logger != null)
				{
					logger.LogMessage(LogCategories.getEngine(), Resources.getJobExitMessage(), JobState.ToString(exitState));
				}
				try {
					JobEngine.this.OnExit(exitState);
				} catch (Exception e) {
					
					e.printStackTrace();
				}
				
			}
		}};
		
		this.StartWorkThread(del);
	}



	public final void Resume(JobStartInfo startInfo) throws Exception
	{

		IJobStatusStorageService storage = this.getService(IJobStatusStorageService.class);
		if (storage.getExists())
		{

			Runnable del =  new Runnable(){public void run()
			{
				int exitState = JobState.Failed;


				ILogger logger = null;
				try {
					logger = (ILogger)JobEngine.this.getService(ILogger.class);
				} catch (Exception e1) {
					
					e1.printStackTrace();
				}
				try
				{

					if (logger != null)
					{
						logger.LogMessage(LogCategories.getEngine(), Resources.getResumeMessage());
					}

		               FireEvent(new Action1<IJobEngineEventHandler>(){
							
							@Override
							public void call(IJobEngineEventHandler handler) throws Exception
							{
								handler.HandleResume(JobEngine.this);
							}
						});


		            JobEngine.this._job = new Job();

		            JobEngine.this._serviceContainer.AddService(_job);
					_job.Initialize();
					InputStream stream = JobEngine.this.getRequiredService(IJobStatusStorageService.class).Load();
					Element ele = JDomUtils.loadElement(stream);
					_job.LoadStatus(ele);

		               FireEvent(new Action1<IJobEngineEventHandler>(){
							
							@Override
							public void call(IJobEngineEventHandler handler) throws Exception
							{
								handler.HandleLoad(JobEngine.this);
							}
						});


					((IJob)_job).Execute();
					exitState = JobState.Succeed;
					JobEngine.this.SaveStatus();
				}
				catch (InterruptedException e)
				{
					exitState = _abortState;
				}
				catch (Exception error)
				{

					if (logger != null)
					{
						logger.LogError(LogCategories.getEngine(), error, Resources.getJobFatalErrorMessage());
					}
				}
				finally
				{
					if (logger != null)
					{
						logger.LogMessage(LogCategories.getEngine(), Resources.getJobExitMessage(), JobState.ToString(exitState));
					}
					try {
						JobEngine.this.OnExit(exitState);
					} catch (Exception e) {
						
						e.printStackTrace();
					}
				}
			}
			 };

			this.StartWorkThread(del);
		}
		else
		{
			this.Start(startInfo);
		}
		
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

	
	public final void Sleep(TimeSpan timeout) throws Exception
	{
		final java.util.Date timeToWait =   TimeSpan.add(new Date(), timeout);
		
		HashMap<String, String> args = new HashMap<String, String>();
		args.put("time", new SimpleDateFormat().format(timeToWait));
		
		ResourceParameter res = new ResourceParameter("WaitTime", args);
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

	public final<T> T getService(java.lang.Class<T> serviceType) throws Exception
	{
		return _serviceContainer.getService(serviceType);
	}

	private UUID privateJobId = UUIDUtils.Empty;
	public final UUID getJobId()
	{
		return privateJobId;
	}
	private void setJobId(UUID value)
	{
		privateJobId = value;
	}

	private String _jobDirectory = null;
	public final String getJobDirectory()
	{
		if (_jobDirectory == null)
		{

		}
		return _jobDirectory;
	}

	private Exception _executingError;
	private boolean _abortingSaved = false;

	public final void SaveStatus() throws Exception
	{
		Element ele = this._job.SaveStatus();
		
		ByteArrayOutputStream output = new ByteArrayOutputStream();
		
		JDomUtils.saveElement(ele, output);
		
		ByteArrayInputStream input = new ByteArrayInputStream(output.toByteArray());
		
		
		this.getRequiredService(IJobStatusStorageService.class).Save(input);

	}

	public final void SaveStatusForError(Exception error) throws Exception
	{

		if (_job != null)
		{
			if (error instanceof InterruptedException)
			{
				if (!_abortingSaved)
				{
					_abortingSaved = true;
					if (!((IJob)this._job).getRuntimeStatus().getIsRestoring())
					{
						this.SaveStatus();
					}
				}
			}
			else if (this._executingError != error)
			{

				_executingError = error;
				if (!((IJob)this._job).getRuntimeStatus().getIsRestoring())
				{
					this.SaveStatus();
				}

			}
		}

	}



	@Override
	public Object getService2(Class serviceType) throws Exception {
		return this._serviceContainer.getService2(serviceType);
	}



	@Override
	public Object getRequiredService2(Class serviceType) throws Exception {
		return this._serviceContainer.getRequiredService2(serviceType);
	}



	@Override
	public <T> T getRequiredService(Class<T> serviceType) throws Exception {
		return this._serviceContainer.getRequiredService(serviceType);
	}

}