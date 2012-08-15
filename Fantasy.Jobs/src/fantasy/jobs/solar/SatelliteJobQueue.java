package fantasy.jobs.solar;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

public class SatelliteJobQueue extends AbstractService implements IJobQueue
{

	@Override
	public void InitializeService()
	{
		this._actionQueue = this.Site.<ISolarActionQueue>GetRequiredService();
		this._satellite = this.Site.<ISatellite>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._satellite.JobAdded += new EventHandler<JobQueueEventArgs>(SatelliteJobAdded);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._satellite.JobChanged += new EventHandler<JobQueueEventArgs>(SatelliteJobChanged);
		super.InitializeService();
	}

	private void SatelliteJobChanged(Object sender, JobQueueEventArgs e)
	{
		this.OnChanged(e);
	}

	private void SatelliteJobAdded(Object sender, JobQueueEventArgs e)
	{
		this.OnAdded(e);
	}


	private ISolarActionQueue _actionQueue;
	private ISatellite _satellite;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobQueue Members

	public final Iterable<JobMetaData> getUnterminates()
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			int count = Integer.MAX_VALUE;
			int i = 0;
			while (count >= 100)
			{
				JobMetaData[] rs = client.Client.Unterminates(i * 100, 100);
				i += rs.length;
				count = rs.length;

				for (JobMetaData job : rs)
				{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
					yield return job;
				}
			}
		}
		finally
		{
		}

	}

	public final Iterable<JobMetaData> getTerminates()
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			int count = Integer.MAX_VALUE;
			int i = 0;
			while (count >= 100)
			{
				JobMetaData[] rs = client.Client.Terminates(i * 100, 100);
				i += rs.length;
				count = rs.length;

				for (JobMetaData job : rs)
				{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
					yield return job;
				}
			}
		}
		finally
		{
		}
	}

	public final JobMetaData FindJobMetaDataById(UUID id)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.FindJobMetaDataById(id);
		}
		finally
		{
		}
	}

	public final Iterable<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.FindTerminated(totalCount, filter, args, order, skip, take);
		}
		finally
		{
		}
	}

	public final Iterable<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.FindUnterminated(totalCount, filter, args, order, skip, take);
		}
		finally
		{
		}
	}

	public final Iterable<JobMetaData> FindAll()
	{
		return this.getUnterminates().Union(this.getTerminates());
	}

	public final JobMetaData CreateJobMetaData()
	{
		return new JobMetaData();
	}

	public final void ApplyChange(JobMetaData job)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_actionQueue.Enqueue(solar => solar.ApplyChange(job));
	}

	public final void Resume(UUID id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_actionQueue.Enqueue(solar => solar.Resume(id));
	}

	public final void Cancel(UUID id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_actionQueue.Enqueue(solar => solar.Cancel(id));
	}

	public final void Suspend(UUID id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_actionQueue.Enqueue(solar => solar.Suspend(id));
	}

	public final void UserPause(UUID id)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		_actionQueue.Enqueue(solar => solar.UserPause(id));
	}



//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> Changed;

	protected void OnChanged(JobQueueEventArgs e)
	{
		if (this.Changed != null)
		{
			this.Changed(this, e);
		}
	}




//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<JobQueueEventArgs> Added;

	protected void OnAdded(JobQueueEventArgs e)
	{
		if (this.Added != null)
		{
			this.Added(this, e);
		}
	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> IJobQueue.RequestCancel
//		{
//			add
//			{
//				throw new NotImplementedException();
//			}
//			remove
//			{
//				throw new NotImplementedException();
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> IJobQueue.RequestSuspend
//		{
//			add
//			{
//				throw new NotImplementedException();
//			}
//			remove
//			{
//				throw new NotImplementedException();
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	event EventHandler<JobQueueEventArgs> IJobQueue.RequestUserPause
//		{
//			add
//			{
//				throw new NotImplementedException();
//			}
//			remove
//			{
//				throw new NotImplementedException();
//			}
//		}

	public final String[] GetAllApplications()
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.GetAllApplications();
		}
		finally
		{
		}
	}

	public final String[] GetAllUsers()
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.GetAllUsers();
		}
		finally
		{
		}
	}

	public final boolean IsTerminated(UUID id)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
		try
		{
			return client.Client.IsTermianted(id);
		}
		finally
		{
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}