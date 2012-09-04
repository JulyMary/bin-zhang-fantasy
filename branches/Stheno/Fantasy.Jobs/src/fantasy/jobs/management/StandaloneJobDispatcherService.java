package fantasy.jobs.management;

import java.rmi.*;
import java.util.*;

import fantasy.collections.*;
import fantasy.jobs.*;
import fantasy.jobs.properties.Resources;
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
					
					//e.printStackTrace();
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
            _resourceManager.addListener(new IResourceManagerListener(){
				@Override
				public void available(EventObject e) {
					ResourceAvailable();
					
				}});

		}

		this._controller = this.getSite().getRequiredService(IJobController.class);

		super.initializeService();
	}

	private void ResourceAvailable()
	{
		synchronized(this._waitHandle)
		{
		    this._waitHandle.notifyAll();
		}
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

	public final void start()
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
		synchronized(this._waitHandle)
		{
		    this._waitHandle.notifyAll();
		}
	}
	
	public void Added(JobMetaData job) throws Exception
	{
		synchronized(this._waitHandle)
		{
		    this._waitHandle.notifyAll();
		}
	}

	public final void tryDispatch()
	{
		synchronized(this._waitHandle)
		{
		    this._waitHandle.notifyAll();
		}
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
				synchronized(this._waitHandle)
				{
				   _waitHandle.wait();
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
						this._resourceQueue.unregisterResourceRequest(job.getId());
					}
					ILogger logger = this.getSite().getService(ILogger.class);
					if (job.getState() == JobState.Unstarted)
					{
						job.setState(JobState.RequestStart);
						if (logger != null)
						{
							logger.LogMessage(LogCategories.getDispatching(), Resources.getStartJobMessage(), job.getName(), job.getId());
						}


						_controller.StartJob(job);
					}
					else
					{
						job.setState(JobState.RequestStart);
						if (logger != null)
						{
							logger.LogMessage(LogCategories.getDispatching(), Resources.getResumeJobMessage(), job.getName(), job.getId());
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