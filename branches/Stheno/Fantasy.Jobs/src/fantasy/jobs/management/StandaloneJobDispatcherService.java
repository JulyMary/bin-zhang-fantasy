package fantasy.jobs.management;

import Fantasy.Jobs.Resources.*;
import Fantasy.ServiceModel.*;

public class StandaloneJobDispatcherService extends AbstractService implements IJobDispatcher
{
	@Override
	public void InitializeService()
	{
		this._queue = (IJobQueue)this.getSite().GetService(IJobQueue.class);
		this._filters = AddIn.<IJobStartupFilter>CreateObjects("jobService/startupFilters/filter");
		this._waitHandle = new AutoResetEvent(false);

		_startJobThread = ThreadFactory.CreateThread(this.Run);

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

		_resourceQueue = this.getSite().<IResourceRequestQueue>GetService();
		_resourceManager = this.getSite().<IResourceManager>GetService();
		if (_resourceManager != null)
		{
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			_resourceManager.Available += new EventHandler(ResourceAvailable);
		}

		this._controller = this.getSite().<IJobController>GetRequiredService();

		super.InitializeService();
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

	private IJobQueue _queue;
	private IResourceRequestQueue _resourceQueue;
	private IResourceManager _resourceManager;

	private IJobStartupFilter[] _filters;

	private IJobController _controller;

	public final void Start()
	{
		_startJobThread.start();
	}

	private void QueueRequestUserPause(Object sender, JobQueueEventArgs e)
	{
		IJobController controller = (IJobController)this.getSite().GetService(IJobController.class);
		controller.UserPause(e.getJob().getId());
	}

	private void QueueRequestSuspend(Object sender, JobQueueEventArgs e)
	{
		IJobController controller = (IJobController)this.getSite().GetService(IJobController.class);
		controller.Suspend(e.getJob().getId());
	}

	private void QueueRequestCancel(Object sender, JobQueueEventArgs e)
	{
		IJobController controller = (IJobController)this.getSite().GetService(IJobController.class);
		controller.Cancel(e.getJob().getId());
	}

	private void QueueChanged(Object sender, JobQueueEventArgs e)
	{
		this._waitHandle.Set();
	}

	public final void TryDispatch()
	{
		_waitHandle.Set();
	}

	public final void Run()
	{
		ILogger logger = this.getSite().<ILogger>GetService();
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

	private boolean _starting = false;

	private boolean TryStartAJob()
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

				JobMetaData job = jobs.FirstOrDefault();
				if (job != null)
				{
					if (this._resourceQueue != null)
					{
						this._resourceQueue.UnregisterResourceRequest(job.getId());
					}
					ILogger logger = this.getSite().<ILogger>GetService();
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