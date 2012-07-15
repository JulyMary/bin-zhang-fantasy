package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Resources.*;
import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

public class SolarJobDispatcherService extends AbstractService implements IJobDispatcher
{

	private java.util.ArrayList<JobStartupData> _runningJobs = new java.util.ArrayList<JobStartupData>();
	private Object _syncRoot = new Object();
	private IResourceManager _resourceManager;


	@Override
	public void InitializeService()
	{
		this._satelliteManager = this.Site.<SatelliteManager>GetRequiredService();
		this._queue = this.Site.<IJobQueue>GetRequiredService();
		this._filters = AddIn.<IJobStartupFilter>CreateObjects("jobService/startupFilters/filter");
		this._waitHandle = new AutoResetEvent(false);

		_startJobThread = ThreadFactory.CreateThread(this.Run);

		_refreshThread = ThreadFactory.CreateThread(this.Refresh);

		for (IJobStartupFilter filter : this._filters)
		{
			if (filter instanceof IObjectWithSite)
			{
				((IObjectWithSite)filter).Site = this.Site;
			}
		}

//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Added += new EventHandler<JobQueueEventArgs>(QueueChanged);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.Changed += new EventHandler<JobQueueEventArgs>(QueueChanged);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.RequestCancel += new EventHandler<JobQueueEventArgs>(QueueRequestCancel);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.RequestSuspend += new EventHandler<JobQueueEventArgs>(QueueRequestSuspend);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_queue.RequestUserPause += new EventHandler<JobQueueEventArgs>(QueueRequestUserPause);

		_resourceQueue = this.Site.<IResourceRequestQueue>GetService();
		_resourceManager = this.Site.<IResourceManager>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_resourceManager.Available += new EventHandler(ResourceManager_Available);

		super.InitializeService();
	}

	private void ResourceManager_Available(Object sender, EventArgs e)
	{
		this.TryDispatch();
	}

	private void ResourceAvailable(Object sender, EventArgs e)
	{
		this._waitHandle.Set();
	}

	@Override
	public void UninitializeService()
	{
		this._startJobThread.stop();
		super.UninitializeService();
	}

	private AutoResetEvent _waitHandle;

	private Thread _startJobThread;
	private Thread _refreshThread;

	private IJobQueue _queue;
	private IResourceRequestQueue _resourceQueue;

	private SatelliteManager _satelliteManager;

	private IJobStartupFilter[] _filters;

	public final void Start()
	{
		_startJobThread.start();
		_refreshThread.start();
	}


	private JobStartupData GetStartupData(Guid id)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			return this._runningJobs.Find(data => id.equals(data.JobMetaData.Id));
		}
	}

	private void QueueRequestUserPause(Object sender, JobQueueEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			JobStartupData data = this.GetStartupData(e.getJob().getId());
			if (data != null)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				this._satelliteManager.Enqueue(data.getSatellite(), null, (satellite, state) =>
				{
					satellite.RequestUserPause(e.getJob().getId());
				}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			   , state =>
				{
					synchronized (_syncRoot)
					{
						this._runningJobs.remove(data);
					}
					e.getJob().setState(JobState.UserPaused);
					this._queue.ApplyChange(e.getJob());
				}
			   );
			}
		}
	   );
	}

	private void QueueRequestSuspend(Object sender, JobQueueEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			JobStartupData data = this.GetStartupData(e.getJob().getId());
			if (data != null)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				this._satelliteManager.Enqueue(data.getSatellite(),null, (satellite, state) =>
				{
					satellite.RequestSuspend(e.getJob().getId());
				}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			   , (state) =>
				{
					synchronized (_syncRoot)
					{
						this._runningJobs.remove(data);
					}
					e.getJob().setState(JobState.Suspended);
					this._queue.ApplyChange(e.getJob());
				}
			   );
			}
		}
	   );
	}

	private void QueueRequestCancel(Object sender, JobQueueEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			JobStartupData data = this.GetStartupData(e.getJob().getId());
			if (data != null)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				this._satelliteManager.Enqueue(data.getSatellite(),null, (satellite, state) =>
				{
					satellite.RequestCancel(e.getJob().getId());
				}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			   , (state) =>
				{
					synchronized (_syncRoot)
					{
						this._runningJobs.remove(data);
					}
					e.getJob().setState(JobState.Cancelled);
					e.getJob().setEndTime(new java.util.Date());
					this._queue.ApplyChange(e.getJob());
				}
			   );
			}
		}
	   );
	}

	private void QueueChanged(Object sender, JobQueueEventArgs e)
	{
		if (e.getJob().getState() != JobState.Running)
		{
			synchronized (_syncRoot)
			{
				JobStartupData data = this.GetStartupData(e.getJob().getId());
				if (data != null)
				{
					this._runningJobs.remove(data);
				}
			}
		}

		this._waitHandle.Set();
	}

	public final void TryDispatch()
	{
		_waitHandle.Set();
	}

	public final void Run()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		while (true)
		{

			try
			{

				while (TryStartAJob());
			}
			catch (ThreadAbortException e)
			{
			}
			catch(RuntimeException error)
			{
				if (logger != null)
				{
					logger.LogError(LogCategories.getManager(), error, "An error occurs when try start a new job.");
				}
			}
			_waitHandle.WaitOne();
		}
	}


	private void Refresh()
	{
		while (true)
		{

			java.util.ArrayList<JobStartupData> list;
			synchronized (this._syncRoot)
			{
				list = new java.util.ArrayList<JobStartupData>(this._runningJobs);
			}
			for (JobStartupData data : list)
			{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				this._satelliteManager.Enqueue(data.getSatellite(), data, (satellite, state) =>
				{
					boolean running = false;
					JobStartupData d = (JobStartupData)state;
					synchronized (this._syncRoot)
					{
						if (this._runningJobs.contains(d))
						{
							running = satellite.IsRunning(d.getJobMetaData().getId());
							if (!running)
							{
								this._runningJobs.remove(d);
							}
						}
					}
					if (!running)
					{
						d.getJobMetaData().setState(JobState.Suspended);
						this._queue.ApplyChange(d.getJobMetaData());
					}
				}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			   , state =>
				{
					JobStartupData d = (JobStartupData)state;
					boolean contains = false;
					synchronized (this._syncRoot)
					{
						contains = this._runningJobs.contains(d);
						if (contains)
						{
							this._runningJobs.remove(d);
						}
					}
					if (contains)
					{
						d.getJobMetaData().setState(JobState.Suspended);
						this._queue.ApplyChange(d.getJobMetaData());
					}
				}
			   );

			}
			Thread.sleep(15 * 1000);
		}
	}

	private boolean _starting = false;

	private Iterable<JobStartupData> GetUnterminatesData()
	{
		for (JobMetaData job : this._queue.getUnterminates())
		{
			JobStartupData tempVar = new JobStartupData();
			tempVar.setJobMetaData(job);
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
			yield return tempVar;
		}
	}

	private boolean TryStartAJob()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		boolean rs = false;
		if (!_starting)
		{
			_starting = true;
			try
			{

				Iterable<JobStartupData> jobs = GetUnterminatesData();
				for (IJobStartupFilter filter : this._filters)
				{
					jobs = filter.Filter(jobs);
				}

				for(JobStartupData data : jobs)
				{

					JobMetaData job = data.getJobMetaData();

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					SatelliteSite site = _satelliteManager.getSatellites().FirstOrDefault(s => data.getSatellite().equals(s.getName()));

					if (site != null)
					{
						try
						{
							if (job.getState() == JobState.Unstarted)
							{
								if (logger != null)
								{
									logger.LogMessage("Dispatch", "Start job {0} ({1}) on satellite {2}", job.getName(), job.getId(), data.getSatellite());
								}
								job.setState(JobState.RequestStart);
								site.getSatellite().RequestStartJob(data.getJobMetaData());
							}
							else
							{
								if (logger != null)
								{
									logger.LogMessage("Dispatch", "Resume job {0} ({1}) on satellite {2}", job.getName(), job.getId(), data.getSatellite());
								}
								job.setState(JobState.RequestStart);
								site.getSatellite().RequestResume(job);
							}

							this._runningJobs.add(data);

							rs = true;
						}
						catch (ThreadAbortException e)
						{
						}
						catch (RuntimeException error)
						{
							if (!WCFExceptionHandler.CanCatch(error))
							{

								if (logger != null)
								{
									logger.LogError("Dispatch", error, "An error occurs while try start/resume job {0} ({1}) on satellite service {2}.", job.getName(), job.getId(), data.getSatellite());
								}
							}
							else
							{
								logger.SafeLogError("Solar", error, "WCF error");
							}
						}

						if (rs)
						{
							if (this._resourceQueue != null)
							{
								this._resourceQueue.UnregisterResourceRequest(job.getId());
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