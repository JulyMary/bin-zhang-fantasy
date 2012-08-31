package fantasy.jobs.solar;

import java.util.*;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;

public class SatelliteJobQueue extends AbstractService implements IJobQueue
{

	@Override
	public void initializeService() throws Exception
	{
		this._actionQueue = this.getSite().getRequiredService(ISolarActionQueue.class);
		this._satellite = this.getSite().getRequiredService(ISatellite.class);
		
		this._satellite.addListener(new ISatelliteListener(){

			@Override
			public void Changed(JobMetaData job) throws Exception {
				SatelliteJobQueue.this.OnChange(job);
				
			}

			@Override
			public void Added(JobMetaData job) throws Exception {
				SatelliteJobQueue.this.OnAdded(job);
				
			}});
		

		super.initializeService();
	}

	


	private ISolarActionQueue _actionQueue;
	private ISatellite _satellite;


	@Override
	public final Iterable<JobMetaData> getUnterminates() throws Exception
	{

		
		ISolar client = ClientFactory.create(ISolar.class);
		return new Enumerable<JobMetaData>(client.getUnterminates());
		

	}

	
    @Override
	public final JobMetaData findJobMetaDataById(UUID id) throws Exception
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ISolar client = ClientFactory.create(ISolar.class);
		return client.findJobMetaDataById(id);
		
	}

	public final Iterable<JobMetaData> findTerminated(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ISolar client = ClientFactory.create(ISolar.class);
		
		return client.findTerminated(totalCount, filter, order, skip, take)
		ClientRef<ISolar> client = ClientFactory.<ISolar>create();
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
		ClientRef<ISolar> client = ClientFactory.<ISolar>create();
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




	public final String[] GetAllApplications()
	{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
		ClientRef<ISolar> client = ClientFactory.<ISolar>create();
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
		ClientRef<ISolar> client = ClientFactory.<ISolar>create();
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
		
		

		ClientRef<ISolar> client = ClientFactory.<ISolar>create();
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