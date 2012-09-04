package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;

public class SatelliteJobQueue extends AbstractService implements IJobQueue
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -4629186411100175910L;


	public SatelliteJobQueue() throws RemoteException {
		super();
		
	}


	@Override
	public void initializeService() throws Exception
	{
		this._actionQueue = this.getSite().getRequiredService(ISolarActionQueue.class);
		this._satellite = this.getSite().getRequiredService(ISatellite.class);
		
		this._satellite.addListener(new ISatelliteListener(){

			@Override
			public void Changed(JobMetaData job) throws Exception {
				
				SatelliteJobQueue.this.onChanged(job);
				
			}

			@Override
			public void Added(JobMetaData job) throws Exception {
				SatelliteJobQueue.this.onAdded(job);
				
			}});
		

		super.initializeService();
	}

	


	private ISolarActionQueue _actionQueue;
	private ISatellite _satellite;
	private HashSet<IJobQueueListener> _listeners = new HashSet<IJobQueueListener>();


	@Override
	public final Iterable<JobMetaData> getUnterminates() throws Exception
	{

		
		ISolar client = ClientFactory.create(ISolar.class);
		return new Enumerable<JobMetaData>(client.getUnterminates());
		

	}

	
    @Override
	public final JobMetaData findJobMetaDataById(UUID id) throws Exception
	{

		ISolar client = ClientFactory.create(ISolar.class);
		return client.findJobMetaDataById(id);
		
	}

    @Override
	public final List<JobMetaData> findTerminated(String filter, String order, int skip, int take) throws Exception
	{
		
		ISolar client = ClientFactory.create(ISolar.class);
		JobMetaData[] rs = client.findTerminated(filter, order, skip, take);
		
		return Arrays.asList(rs);

	}

    
    @Override
	public final List<JobMetaData> findUnterminated(String filter, String order, int skip, int take) throws Exception
	{
		ISolar client = ClientFactory.create(ISolar.class);
		JobMetaData[] rs = client.findUnterminated(filter, order, skip, take);
		
		return Arrays.asList(rs);
	}

	

	public final JobMetaData createJobMetaData()
	{
		return new JobMetaData();
	}



	@Override
	public final void resume(final UUID id) throws Exception
	{
		_actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.resume(id);
			}});
	}

	
	@Override
	public final void cancel(final UUID id) throws Exception
	{
		_actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.cancel(id);
			}});
	}

	@Override
	public final void suspend(final UUID id) throws Exception
	{
		_actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.suspend(id);
			}});
	}

	
	@Override
	public final void userPause(final UUID id) throws Exception
	{
		_actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.userPause(id);
			}});
	}

	protected void onChanged(JobMetaData job) throws Exception
	{
		for(IJobQueueListener listener : this._listeners)
		{
			listener.Changed(job);
		}
	}


	
	protected void onAdded(JobMetaData job) throws Exception
	{
		for(IJobQueueListener listener : this._listeners)
		{
			listener.Added(job);
		}
	}




	@Override
	public final String[] getAllApplications() throws Exception
	{
		ISolar client = ClientFactory.create(ISolar.class);
		return client.getAllApplications();
	}

	
	@Override
	public final String[] getAllUsers() throws Exception
	{
		ISolar client = ClientFactory.create(ISolar.class);
		return client.getAllUsers();
	}

	
	@Override
	public final boolean isTerminated(UUID id) throws Exception
	{
		ISolar client = ClientFactory.create(ISolar.class);
		return client.isTerminated(id);

	}


	@Override
	public int getTerminatedCount() throws Exception {
		ISolar client = ClientFactory.create(ISolar.class);
		return client.getTerminatedCount();
	}


	@Override
	public int getUnterminatedCount() throws Exception {
		ISolar client = ClientFactory.create(ISolar.class);
		return client.getUnterminatedCount();
	}


	@Override
	public void add(JobMetaData job) throws Exception {
		ISolar client = ClientFactory.create(ISolar.class);
		client.add(job);
		
	}


	@Override
	public void updateState(JobMetaData job, boolean isStart) throws Exception {
		ISolar client = ClientFactory.create(ISolar.class);
		client.updateState(job, isStart);
		
	}


	@Override
	public void archive(JobMetaData job) throws Exception {
		ISolar client = ClientFactory.create(ISolar.class);
		client.archive(job);
		
	}


	@Override
	public void addListener(IJobQueueListener listener) {
		this._listeners.add(listener);
		
	}


	@Override
	public void removeListener(IJobQueueListener listener) {
		this._listeners.remove(listener);
		
	}

}