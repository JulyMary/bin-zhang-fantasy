package fantasy.jobs.management;

import java.rmi.*;
import java.util.*;

import fantasy.collections.*;
import fantasy.jobs.*;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import fantasy.*;

public class StandaloneJobDispatcherService extends AbstractService implements IJobDispatcher, IJobQueueListener
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -9077723331377116255L;

	public StandaloneJobDispatcherService() throws RemoteException {
		super();
		
	}
	
	
	private UUID _resourceHandlerID = UUID.randomUUID();

	@Override
	public void initializeService() throws Exception
	{
		this._queue = (IJobQueue)this.getSite().getRequiredService(IJobQueue.class);
		this._filters = AddIn.CreateObjects(IJobStartupFilter.class, "jobService/startupFilters/filter");
		this._waitHandle = new Object();

		_startJobThread = ThreadFactory.create(new Runnable(){

			@Override
			public void run() {
				try {
					StandaloneJobDispatcherService.this.Run();
				} catch (Exception e) {
					
					e.printStackTrace();
				}
				
			}});

		for (IJobStartupFilter filter : this._filters)
		{
			if (filter instanceof IObjectWithSite)
			{
				((IObjectWithSite)filter).setSite(this.getSite());
			}
		}
		
		this._queue.addListener(this);
		
		


		_resourceQueue = this.getSite().getService(IResourceRequestQueue.class);
		_resourceManager = this.getSite().getService(IResourceManager.class);
		if (_resourceManager != null)
		{
            _resourceManager.RegisterHandler(new IResourceManagerHandler(){

				@Override
				public void Revoke(UUID id) {
					
				}

				@Override
				public void Available() {
					ResourceAvailable();
					
				}

				@Override
				public UUID Id() {
					return _resourceHandlerID;
				}});
			
		}

		this._controller = this.getSite().getRequiredService(IJobController.class);

		super.initializeService();
	}

	private void ResourceAvailable()
	{
		this._waitHandle.notifyAll();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		this._startJobThread.interrupt();
		super.uninitializeService();
	}

	private Object _waitHandle;

	private Thread _startJobThread;

	private IJobQueue _queue;
	private IResourceRequestQueue _resourceQueue;
	private IResourceManager _resourceManager;

	private IJobStartupFilter[] _filters;

	private IJobController _controller;

	public final void Start()
	{
		_startJobThread.start();
	}

	public  void RequestUserPause(JobMetaData job) throws Exception
	{
		IJobController controller = (IJobController)this.getSite().getRequiredService(IJobController.class);
		controller.UserPause(job.getId());
	}

	public void RequestSuspend(JobMetaData job) throws Exception
	{
		IJobController controller = (IJobController)this.getSite().getRequiredService(IJobController.class);
		controller.Suspend(job.getId());
	}

	public void RequestCancel(JobMetaData job) throws Exception
	{
		IJobController controller = (IJobController)this.getSite().getRequiredService(IJobController.class);
		controller.Cancel(job.getId());
	}

	public void Changed(JobMetaData job)
	{
		this._waitHandle.notifyAll();
	}
	
	public void Added(JobMetaData job) throws Exception
	{
		this._waitHandle.notifyAll();
	}

	public final void TryDispatch()
	{
		this._waitHandle.notifyAll();
	}

	public final void Run() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		while (true)
		{

			try
			{

				while (TryStartAJob())
				{
					if(Thread.interrupted())
					{
						throw new InterruptedException();
					}
				}
			}
			catch (InterruptedException e)
			{
				throw e;
			}
			catch(Exception error)
			{
				if (logger != null)
				{
					logger.LogError(LogCategories.getManager(), error, "An error occurs when try start a new job.");
				}
			}
			_waitHandle.wait();
		}
	}

	private boolean _starting = false;

	private boolean TryStartAJob() throws Exception
	{
		boolean rs = false;
		if (!_starting)
		{
			_starting = true;
			try
			{
				Iterable<JobMetaData> jobs = this._queue.getUnterminates();
				for (IJobStartupFilter filter : this._filters)
				{
					jobs = filter.Filter(jobs);
				}

				JobMetaData job = new Enumerable<JobMetaData>(jobs).firstOrDefault();
				if (job != null)
				{
					if (this._resourceQueue != null)
					{
						this._resourceQueue.UnregisterResourceRequest(job.getId());
					}
					ILogger logger = this.getSite().getService(ILogger.class);
					if (job.getState() == JobState.Unstarted)
					{
						job.setState(JobState.RequestStart);
						if (logger != null)
						{
							logger.LogMessage("Dispatching", "Start Job {0} ({1}).", job.getName(), job.getId());
						}


						_controller.StartJob(job);
					}
					else
					{
						job.setState(JobState.RequestStart);
						if (logger != null)
						{
							logger.LogMessage("Dispatching", "Resume Job {0} ({1}).", job.getName(), job.getId());
						}
						_controller.Resume(job);
					}

					rs = true;
				}
			}
			finally
			{
				_starting = false;
			}

		}

		return rs;

	}



}