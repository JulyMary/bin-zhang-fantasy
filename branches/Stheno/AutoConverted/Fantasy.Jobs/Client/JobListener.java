package Fantasy.Jobs.Client;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;
import Fantasy.Tracking.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
public class JobListener implements IJobEventHandler, IDisposable
{

	public JobListener()
	{
		_refreshThread = ThreadFactory.CreateThread(this.Refresh);


		_listenedJobs = new Collection<Guid>();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_listenedJobs.Inserted += new EventHandler<CollectionEventArgs<Guid>>(ListenedJobsInserted);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_listenedJobs.Removed += new EventHandler<CollectionEventArgs<Guid>>(ListenedJobsRemoved);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		_listenedJobs.Cleared += new EventHandler(ListenedJobsCleared);

	}

	private void ListenedJobsCleared(Object sender, EventArgs e)
	{

	}

	private void ListenedJobsRemoved(Object sender, CollectionEventArgs<Guid> e)
	{

	}

	private void ListenedJobsInserted(Object sender, CollectionEventArgs<Guid> e)
	{

	}

	public final void Start()
	{
		_refreshThread.start();
	}


	private boolean _connected;

	public final boolean getConnected()
	{
		return _connected;
	}
	public final void setConnected(boolean value)
	{
		if (_connected != value)
		{
			_connected = value;

			if (_connected)
			{
				this.OnConnect(EventArgs.Empty);
			}
			else
			{
				this.OnDisconnect(EventArgs.Empty);
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Connect;

	protected void OnConnect(EventArgs e)
	{
		if (this.Connect != null)
		{
			this.Connect(this, e);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Disconnect;

	protected void OnDisconnect(EventArgs e)
	{
		if (this.Disconnect != null)
		{
			this.Disconnect(this, e);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobListenerJobEventArgs> JobAdded;

	protected void OnJobAdded(JobListenerJobEventArgs e)
	{
		if (this.JobAdded != null)
		{
			this.JobAdded(this, e);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobListenerJobEventArgs > JobStateChanged;

	protected void OnJobStateChanged(JobListenerJobEventArgs e)
	{
		if (this.JobStateChanged != null)
		{
			this.JobStateChanged(this, e);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobListenerTrackEventArgs> TrackCreated;

	protected void OnTrackCreated(JobListenerTrackEventArgs e)
	{
		if (this.TrackCreated != null)
		{
			this.TrackCreated(this, e);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobListenerTrackEventArgs> TrackChanged;

	protected void OnTrackChanged(JobListenerTrackEventArgs e)
	{
		if (this.TrackChanged != null)
		{
			this.TrackChanged(this, e);
		}
	}


	private Collection<Guid> _listenedJobs;

	public final java.util.List<Guid> getListenedJobs()
	{
		return _listenedJobs;
	}



	private Object _syncRoot = new Object();

	private Thread _refreshThread;
	private ClientRef<IJobEventService> _jobEvent;

	private TrackFactory _trackConnection;

	private long privateVersion;
	public final long getVersion()
	{
		return privateVersion;
	}
	private void setVersion(long value)
	{
		privateVersion = value;
	}

	private void NewVersion()
	{
		synchronized (this._syncRoot)
		{
			setVersion(new java.util.Date().Ticks);
		}
	}

	private java.util.ArrayList<JobMetaData> _jobs = new java.util.ArrayList<JobMetaData>();
	public final JobMetaData[] getJobMetaData()
	{
		synchronized (this._syncRoot)
		{
			return _jobs.toArray(new JobMetaData[]{});
		}
	}

	private java.util.ArrayList<ITrackListener> _tracks = new java.util.ArrayList<ITrackListener>();
	public final ITrackListener[] getTracks()
	{
		synchronized (this._syncRoot)
		{
			return _tracks.toArray(new ITrackListener[]{});
		}
	}


	private void Refresh()
	{
		TimeSpan waitTime = new TimeSpan();
		_trackConnection = new TrackFactory();
		while (true)
		{

			try
			{
				boolean reload = false;

				if (_jobEvent == null)
				{
					reload = true;
					_jobEvent = ClientFactory.<IJobEventService>CreateDuplex(this);
					_jobEvent.Client.Connect();
				}
				else
				{
					_jobEvent.Client.Echo();
				}
				if (reload)
				{

//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//					using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
					ClientRef<IJobService> service = ClientFactory.<IJobService>Create();
					try
					{
						for (Guid id : this._listenedJobs)
						{

							JobMetaData job = service.Client.FindJobById(id);
							if (job != null)
							{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
								JobMetaData old = this._jobs.Find(j => id.equals(j.Id));
								if (old == null)
								{
									this._jobs.add(job);
									this.OnJobAdded(new JobListenerJobEventArgs(job));
								}
								else if (old.getState() != job.getState())
								{
									this.OnJobStateChanged(new JobListenerJobEventArgs(job));
								}

								CreateOrDisposeTrack(job);
							}
						}
					}
					finally
					{
					}

					synchronized (_syncRoot)
					{
						this.NewVersion();
					}
				}
				waitTime = new TimeSpan(0, 2, 0);
				this.setConnected(true);
			}
			catch (ThreadAbortException e)
			{
				waitTime = TimeSpan.getMinValue();
				if (_jobEvent != null && _jobEvent.State == CommunicationState.Opened)
				{
					_jobEvent.Client.Disconnect();
				}
			}
			catch (java.lang.Exception e2)
			{
				_jobEvent = null;
				synchronized (_syncRoot)
				{
					this.NewVersion();
				}
				waitTime = new TimeSpan(0, 0, 10);
				this.setConnected(false);
			}
			Thread.sleep(waitTime);
		}
	}

	private void CreateOrDisposeTrack(JobMetaData job)
	{
		Guid id = job.getId();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		ITrackListener oldTrack = this._tracks.Find(t => id.equals(t.Id));
		if (job.getState() == JobState.Running && oldTrack == null)
		{
			ITrackListener track = this._trackConnection.CreateTrackListener(id);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			track.Changed += new EventHandler<TrackChangedEventArgs>(TrackListenerChanged);
			this._tracks.add(track);
			this.OnTrackCreated(new JobListenerTrackEventArgs(track, null));
		}
		else if (job.getState() != JobState.Running && oldTrack != null)
		{
			this._tracks.remove(oldTrack);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			oldTrack.Changed -= new EventHandler<TrackChangedEventArgs>(TrackListenerChanged);
			oldTrack.dispose();
		}
	}

	private void TrackListenerChanged(Object sender, TrackChangedEventArgs e)
	{
		this.OnTrackChanged(new JobListenerTrackEventArgs((ITrackListener)sender, e));
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEventHandler Members

	private void Added(JobMetaData job)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			if (this._listenedJobs.indexOf(job.getId()) >= 0 && this._jobs.Find(j => job.getId().equals(j.Id)) == null)
			{
				this._jobs.add(job);
				this.OnJobAdded(new JobListenerJobEventArgs(job));
			}
		}
	}

	private void Changed(JobMetaData job)
	{
		synchronized (_syncRoot)
		{
			if (this._listenedJobs.indexOf(job.getId()) >= 0)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				int idx = this._jobs.FindIndex(j => job.getId().equals(j.Id));
				if (idx < 0)
				{
					this._jobs.add(job);
					this.OnJobAdded(new JobListenerJobEventArgs(job));
				}
				else
				{
					this._jobs.set(idx, job);
					this.OnJobStateChanged(new JobListenerJobEventArgs(job));
				}

				CreateOrDisposeTrack(job);
			}
			this.NewVersion();
		}
	}

	private void Echo()
	{

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IDisposable Members

	public final void dispose()
	{
		this._refreshThread.stop();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}