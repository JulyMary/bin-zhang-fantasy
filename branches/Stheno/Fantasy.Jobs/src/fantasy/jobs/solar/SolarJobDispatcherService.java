package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.*;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

import org.apache.commons.lang3.ObjectUtils;


import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.*;
import fantasy.jobs.resources.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;

public class SolarJobDispatcherService extends AbstractService implements IJobDispatcher
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 387956167668610227L;


	public SolarJobDispatcherService() throws RemoteException {
		super();
		
	}

	private java.util.ArrayList<JobStartupData> _runningJobs = new java.util.ArrayList<JobStartupData>();
	private Object _syncRoot = new Object();
	private IResourceManager _resourceManager;


	@Override
	public void initializeService() throws Exception
	{
		this._satelliteManager = this.getSite().getRequiredService(SatelliteManager.class);
		this._queue = this.getSite().getRequiredService(IJobQueue.class);
		this._filters = AddIn.CreateObjects(IJobStartupFilter.class, "jobService/startupFilters/filter");
		this._waitHandle = new Object();

		_startJobThread = ThreadFactory.create(new Runnable(){

			@Override
			public void run() {
				try {
					SolarJobDispatcherService.this.run();
				} catch (Exception e) {
				
					e.printStackTrace();
				}
				
			}});

		this._executor = Executors.newScheduledThreadPool(1);
		//_refreshThread = ThreadFactory.CreateThread(this.Refresh);

		for (IJobStartupFilter filter : this._filters)
		{
			if (filter instanceof IObjectWithSite)
			{
				((IObjectWithSite)filter).setSite(this.getSite());
			}
		}

		
		this._queue.addListener(new IJobQueueListener(){

			@Override
			public void Changed(JobMetaData job) throws Exception {
				
				SolarJobDispatcherService.this.queueChanged(job);
			}

			@Override
			public void Added(JobMetaData job) throws Exception {
				SolarJobDispatcherService.this.queueChanged(job);
				
			}

			@Override
			public void RequestCancel(final JobMetaData job) throws Exception {
				
				
				ThreadFactory.queueUserWorkItem(new Runnable(){

					@Override
					public void run() {
						try {
							SolarJobDispatcherService.this.queueRequestCancel(job);
						} catch (Exception e) {
							
							e.printStackTrace();
						}
						
					}});
				
			}

			@Override
			public void RequestSuspend(final JobMetaData job) throws Exception {
				
				
				ThreadFactory.queueUserWorkItem(new Runnable(){

					@Override
					public void run() {
						try {
							SolarJobDispatcherService.this.queueRequestSuspend(job);
						} catch (Exception e) {
						
							e.printStackTrace();
						}
						
					}});
				
			}

			@Override
			public void RequestUserPause(final JobMetaData job) throws Exception {
				
				ThreadFactory.queueUserWorkItem(new Runnable(){

					@Override
					public void run() {
						try {
							SolarJobDispatcherService.this.queueRequestUserPause(job);
						} catch (Exception e) {
							
							e.printStackTrace();
						}
						
					}});
				
				
			}});
		


		_resourceQueue = this.getSite().getService(IResourceRequestQueue.class);
		_resourceManager = this.getSite().getRequiredService(IResourceManager.class);
		
		_resourceManager.addListener(new IResourceManagerListener(){

			@Override
			public void available(EventObject e) throws Exception {
				SolarJobDispatcherService.this.tryDispatch();
				
			}});
		super.initializeService();
	}

	

	

	@Override
	public void uninitializeService() throws Exception
	{
		this._startJobThread.interrupt();
		super.uninitializeService();
	}

	private Object _waitHandle;

	private Thread _startJobThread;
	ScheduledExecutorService _executor;

	private IJobQueue _queue;
	private IResourceRequestQueue _resourceQueue;

	private SatelliteManager _satelliteManager;

	private IJobStartupFilter[] _filters;

	@Override
	public final void start()
	{
		_startJobThread.start();
		this._executor.schedule(new Runnable(){

			@Override
			public void run() {
				SolarJobDispatcherService.this.refresh();
				
			}}, 15, TimeUnit.SECONDS);
	}


	private JobStartupData getStartupData(final UUID id) throws Exception
	{
		synchronized (_syncRoot)
		{
			return new Enumerable<JobStartupData>(this._runningJobs).firstOrDefault(new Predicate<JobStartupData>(){

				@Override
				public boolean evaluate(JobStartupData obj) throws Exception {
					return ObjectUtils.equals(obj.getJobMetaData().getId(), id);
				}});
		
		}
	}

	private void queueRequestUserPause(final JobMetaData job) throws Exception
	{
				final JobStartupData data = getStartupData(job.getId());
				if (data != null)
				{
					_satelliteManager.enqueue(data.getSatellite(), null, new Action2<ISatellite, Object>(){

						@Override
						public void call(ISatellite satellite, Object arg2)
								throws Exception {
							satellite.requestUserPause(job.getId());
							
						}}, new Action1<Object>(){

						@Override
						public void call(Object arg) throws Exception {
							synchronized (_syncRoot)
							{
								_runningJobs.remove(data);
							}
							job.setState(JobState.UserPaused);
							_queue.updateState(job, false);
							
						}}
				   );
				}

	}

	private void queueRequestSuspend(final JobMetaData job) throws Exception
	{
		
		
		final JobStartupData data = getStartupData(job.getId());
		if (data != null)
		{
			_satelliteManager.enqueue(data.getSatellite(), null, new Action2<ISatellite, Object>(){

				@Override
				public void call(ISatellite satellite, Object arg2)
						throws Exception {
					satellite.requestSuspend(job.getId());
					
				}}, new Action1<Object>(){

				@Override
				public void call(Object arg) throws Exception {
					synchronized (_syncRoot)
					{
						_runningJobs.remove(data);
					}
					job.setState(JobState.Suspended);
					_queue.updateState(job, false);
					
				}}
		   );
		}
		

	}

	private void queueRequestCancel(final JobMetaData job) throws Exception
	{
		
		final JobStartupData data = getStartupData(job.getId());
		if (data != null)
		{
			_satelliteManager.enqueue(data.getSatellite(), null, new Action2<ISatellite, Object>(){

				@Override
				public void call(ISatellite satellite, Object arg2)
						throws Exception {
					satellite.requestCancel(job.getId());
					
				}}, new Action1<Object>(){

				@Override
				public void call(Object arg) throws Exception {
					synchronized (_syncRoot)
					{
						_runningJobs.remove(data);
					}
					job.setState(JobState.Cancelled);
					_queue.updateState(job, false);
					
				}}
		   );
		}
		
		

	}

	private void queueChanged(JobMetaData job) throws Exception
	{
		if (job.getState() != JobState.Running)
		{
			synchronized (_syncRoot)
			{
				JobStartupData data = this.getStartupData(job.getId());
				if (data != null)
				{
					this._runningJobs.remove(data);
				}
			}
		}

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

	private final void run() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		while (true)
		{

			try
			{

				while (tryStartAJob())
					{
					    if(Thread.interrupted())
					    {
					    	throw new InterruptedException();
					    }
					};
			}
			
			catch(InterruptedException e)
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
			
			synchronized(_waitHandle)
			{
			    _waitHandle.wait();
			}
		}
	}


	private void refresh()
	{
		
			java.util.ArrayList<JobStartupData> list;
			synchronized (this._syncRoot)
			{
				list = new java.util.ArrayList<JobStartupData>(this._runningJobs);
			}
			for (final JobStartupData data : list)
			{
				this._satelliteManager.enqueue(data.getSatellite(), data, new Action2<ISatellite, Object>(){

					@Override
					public void call(ISatellite satellite, Object state)
							throws Exception {
						boolean running = false;
						JobStartupData d = (JobStartupData)state;
						synchronized (SolarJobDispatcherService.this._syncRoot)
						{
							if (SolarJobDispatcherService.this._runningJobs.contains(d))
							{
								running = satellite.isRunning(d.getJobMetaData().getId());
								if (!running)
								{
									SolarJobDispatcherService.this._runningJobs.remove(d);
								}
							}
						}
						if (!running)
						{
							d.getJobMetaData().setState(JobState.Suspended);
							SolarJobDispatcherService.this._queue.updateState(d.getJobMetaData(), false);
						}
						
					}}, new Action1<Object>(){

						@Override
						public void call(Object state) throws Exception {
							JobStartupData d = (JobStartupData)state;
							boolean contains = false;
							synchronized (SolarJobDispatcherService.this._syncRoot)
							{
								contains = SolarJobDispatcherService.this._runningJobs.contains(d);
								if (contains)
								{
									SolarJobDispatcherService.this._runningJobs.remove(d);
								}
							}
							if (contains)
							{
								d.getJobMetaData().setState(JobState.Suspended);
								SolarJobDispatcherService.this._queue.updateState(d.getJobMetaData(), false);
							}
							
						}});
				
		}
	}

	private boolean _starting = false;

	private Iterable<JobStartupData> getUnterminatesData() throws Exception
	{
		
		return new Enumerable<JobMetaData>(this._queue.getUnterminates()).select(new Selector<JobMetaData, JobStartupData>(){

			@Override
			public JobStartupData select(JobMetaData job) {
				JobStartupData rs = new JobStartupData();
				rs.setJobMetaData(job);
				return rs;
			}});
		
		
	}

	private boolean tryStartAJob() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		boolean rs = false;
		if (!_starting)
		{
			_starting = true;
			try
			{

				Iterable<JobStartupData> jobs = getUnterminatesData();
				for (IJobStartupFilter filter : this._filters)
				{
					jobs = filter.filter(jobs);
				}

				for(final JobStartupData data : jobs)
				{

					JobMetaData job = data.getJobMetaData();


					SatelliteSite site = new Enumerable<SatelliteSite>(_satelliteManager.getSatellites()).firstOrDefault(new Predicate<SatelliteSite>(){

						@Override
						public boolean evaluate(SatelliteSite s)
								throws Exception {
							return data.getSatellite().equals(s.getName());
						}});

					if (site != null)
					{
						try
						{
							if (job.getState() == JobState.Unstarted)
							{
								if (logger != null)
								{
									logger.LogMessage("Dispatch", "Start job %1$s (%2$s) on satellite %3$s", job.getName(), job.getId(), data.getSatellite());
								}
								job.setState(JobState.RequestStart);
								site.getSatellite().requestStartJob(data.getJobMetaData());
							}
							else
							{
								if (logger != null)
								{
									logger.LogMessage("Dispatch", "Resume job %1$s (%2$s) on satellite %3$s", job.getName(), job.getId(), data.getSatellite());
								}
								job.setState(JobState.RequestStart);
								site.getSatellite().requestResume(job);
							}

							this._runningJobs.add(data);

							rs = true;
						}
						catch (InterruptedException e)
						{
							throw e;
						}
						catch (Exception error)
						{
							if (!WCFExceptionHandler.canCatch(error))
							{

								if (logger != null)
								{
									logger.LogError("Dispatch", error, "An error occurs while try start/resume job {0} ({1}) on satellite service {2}.", job.getName(), job.getId(), data.getSatellite());
								}
							}
							
						}

						if (rs)
						{
							if (this._resourceQueue != null)
							{
								this._resourceQueue.unregisterResourceRequest(job.getId());
							}

							break;
						}

					}

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