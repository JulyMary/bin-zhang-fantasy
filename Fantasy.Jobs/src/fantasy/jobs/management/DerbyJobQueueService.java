package fantasy.jobs.management;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;
import fantasy.collections.*;

public class DerbyJobQueueService extends AbstractService implements IJobQueue
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 6195616031269525491L;

	public DerbyJobQueueService() throws RemoteException {
		super();
		
	}


	private FantasyJobsEntities _entities;

	private java.util.ArrayList<JobMetaData> _unterminates = new java.util.ArrayList<JobMetaData>();

	@Override
	public void initializeService() throws Exception
	{
		_entities = new FantasyJobsEntities();
		
		
		_unterminates = new Enumerable<JobMetaData>(_entities.getUnterminates()).orderBy(new Selector<JobMetaData, UUID>(){

			@Override
			public UUID select(JobMetaData item) {
				return item.getId();
			}}).toArrayList();
		
	
		super.initializeService();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
	
		_entities.dispose();
	}


	public final Iterable<JobMetaData> getUnterminates()
	{
		java.util.ArrayList<JobMetaData> uns;
		synchronized (_syncRoot)
		{
			uns = new java.util.ArrayList<JobMetaData>(_unterminates);
		}
		return uns;
	}

	

	public final JobMetaData FindJobMetaDataById(UUID id) throws Exception
	{
		synchronized (_syncRoot)
		{
			JobMetaData rs = null;
			int pos =  CollectionUtils.binarySearchBy(this._unterminates, id, new Selector<JobMetaData, UUID>(){

				@Override
				public UUID select(JobMetaData item) {
					return item.getId();
				}});
			if (pos >= 0)
			{
				rs = _unterminates.get(pos);
			}

			if (rs == null)
			{
				String filter = String.format("ID = x'%1$s'", UUIDUtils.toString(id, "n"));
			    List<JobMetaData> list = this._entities.query("APP.FTS_JOB_ARCHIVEDJOBS", filter, null, Integer.MAX_VALUE, 0);
			    
			    if(list.size() > 0)
			    {
			    	rs = list.get(0);
			    }
			    
			}

			return rs;
		}
	}



	public final JobMetaData CreateJobMetaData()
	{
		return new JobMetaData();
	}


	private Object _syncRoot = new Object();
	
	
	public void Add(JobMetaData job) throws Exception
	{
		synchronized (_syncRoot)
		{
			this._entities.addToUnterminates(job);
		}
		
		this.onAdded(job);
	}
	
	public void UpdateState(JobMetaData job, boolean isStart) throws Exception
	{
		synchronized (_syncRoot)
		{
			JobMetaData old = this.FindJobMetaDataById(job.getId());
			if (old != null && old != job)
			{
				old.setState(job.getState());
				if(isStart )
					{
					old.setStartTime(job.getStartTime());
					}
					
				job = old;
			}
			
			
			if(isStart)
			{
				_entities.setStart(job);
			}
			else
			{
				_entities.setState(job);
			}
			
		}
		
		this.onChanged(job);
	}
	
	public void Archive(JobMetaData job) throws Exception
	{
		
		synchronized (_syncRoot)
		{
			
			int pos =  CollectionUtils.binarySearchBy(this._unterminates, job.getId(), new Selector<JobMetaData, UUID>(){

				@Override
				public UUID select(JobMetaData item) {
					return item.getId();
				}});
			if (pos >= 0)
			{
				this._unterminates.remove(pos);
			}
			
			this._entities.moveToTerminates(job);
			
		}
		this.onChanged(job);
	}
	
	
	
	
	
	
	public final List<JobMetaData> FindTerminated(RefObject<Integer> totalCount, String filter,  String order, int skip, int take) throws Exception
	{
		List<JobMetaData> rs;
		synchronized (_syncRoot)
		{

			totalCount.argvalue = this._entities.getTerminatedCount();
			
			rs = this._entities.query("APP.FTS_JOB_ARCHIVEDJOBS", filter, order, take, skip);
		}
		return rs;
	}
	public final List<JobMetaData> FindUnterminated(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception
	{
		List<JobMetaData> rs;
		synchronized (_syncRoot)
		{
			totalCount.argvalue = this._unterminates.size();
			rs = this._entities.query("APP.FTS_JOB_JOBS", filter, order, take, skip);
		}

		return rs;

	}

   	

	

	

	
	private HashSet<IJobQueueListener> _listeners = new HashSet<IJobQueueListener>();

	protected void onChanged(JobMetaData job) throws Exception
	{
		synchronized(_listeners)
		{
			for(IJobQueueListener listener: _listeners)
			{
				listener.Changed(job);
			}
		}
	}

	protected void onAdded(JobMetaData job) throws Exception
	{
		synchronized(_listeners)
		{
			for(IJobQueueListener listener: _listeners)
			{
				listener.Added(job);
			}
		}
	}

	public final void Resume(UUID id) throws Exception
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			switch (meta.getState())
			{

				case JobState.UserPaused:
					meta.setState(meta.getStartTime() != null ? JobState.Suspended : JobState.Unstarted);
					this.UpdateState(meta, false);
					break;
				default:
					throw new IllegalStateException(String.format(Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Suspended)));
			}
		}
	}
	
	private void onRequestCancel(JobMetaData job) throws Exception
	{
		synchronized(_listeners)
		{
			for(IJobQueueListener listener: _listeners)
			{
				listener.RequestCancel(job);
			}
		}
	}

	public final void Cancel(UUID id) throws Exception
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if(!meta.getIsTerminated())
			{
				if(meta.getState() == JobState.Running)
				{
					onRequestCancel(meta);
				}
				else
				{
					meta.setState(JobState.Cancelled);
					meta.setEndTime(new java.util.Date());
					this.Archive(meta);
				}
			}
			else

			{
				throw new IllegalStateException(String.format(Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Cancelled)));
			}
		}
	}

	public final void Suspend(UUID id) throws Exception
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if (!meta.getIsTerminated())
			{
				if (meta.getState() == JobState.Running)
				{
					this.onRequestSuspend(meta);

				}
				else
				{
					meta.setState(JobState.Suspended);
					this.UpdateState(meta, false);
				}
			}
			else
			{
				throw new IllegalStateException(String.format(Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.Cancelled)));
			}
		}
	}

	private void onRequestSuspend(JobMetaData meta) throws Exception {
		synchronized(_listeners)
		{
			for(IJobQueueListener listener: _listeners)
			{
				listener.RequestSuspend(meta);
			}
		}
		
	}

	public final void UserPause(UUID id) throws Exception
	{
		JobMetaData meta = this.FindJobMetaDataById(id);
		if (meta != null)
		{
			if (!meta.getIsTerminated())
			{
				if (meta.getState() == JobState.Running)
				{
					this.onRequestUserPause(meta);
				}
				else
				{
					meta.setState(JobState.UserPaused);
					this.UpdateState(meta, false);
				}
			}
			else
			{
				throw new IllegalStateException(String.format(Resources.getInvalidJobTransitionText(), id, JobState.ToString(meta.getState()), JobState.ToString(JobState.UserPaused)));
			}
		}
	}



	private void onRequestUserPause(JobMetaData meta) throws Exception {
		synchronized(_listeners)
		{
			for(IJobQueueListener listener: _listeners)
			{
				listener.RequestUserPause(meta);
			}
		}
		
	}

	public final String[] GetAllApplications()
	{
		return new String[0];
	}

	public final String[] GetAllUsers()
	{
		return new String[0];
	}





	public final boolean IsTerminated(UUID id)
	{
		synchronized (_syncRoot)
		{
			int pos =  CollectionUtils.binarySearchBy(this._unterminates, id, new Selector<JobMetaData, UUID>(){

				@Override
				public UUID select(JobMetaData item) {
					return item.getId();
				}});
			
			return pos < 0;
		}
	}

	

	@Override
	public void addListener(IJobQueueListener listener) {
		synchronized(this._listeners)
		{
			this._listeners.add(listener);
		}
		
	}

	@Override
	public void removeListener(IJobQueueListener listener) {
		synchronized(this._listeners)
		{
			this._listeners.remove(listener);
		}
		
	}


}