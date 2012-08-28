package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.*;


import fantasy.RefObject;
import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;

public class SolarService extends AbstractService implements ISolar
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 8461040238981801650L;

	public SolarService() throws RemoteException {
		super();
		
	}


	private IJobQueue _queue;

	@Override
	public void initializeService() throws Exception
	{
		this._queue = this.getSite().getRequiredService(IJobQueue.class);

		super.initializeService();
	}

	



	


	public final void registerResourceRequest(UUID jobId, ResourceParameter[] parameters) throws Exception
	{
		IResourceRequestQueue resQueue = this.getSite().getService(IResourceRequestQueue.class);
		if (resQueue != null)
		{
			resQueue.registerResourceRequest(jobId, parameters);
		}
	}




	public final void unregisterResourceRequest(UUID jobId) throws Exception
	{
		IResourceRequestQueue resQueue = this.getSite().getService(IResourceRequestQueue.class);
		if (resQueue != null)
		{
			resQueue.unregisterResourceRequest(jobId);
		}
	}

	public final ResourceParameter[] getRequiredResources(UUID jobId) throws Exception
	{
		IResourceRequestQueue resQueue = this.getSite().getService(IResourceRequestQueue.class);
		if (resQueue != null)
		{
			return resQueue.getRequiredResources(jobId);
		}
		else
		{
			return new ResourceParameter[] { };
		}
	}



	public final void resourceAvaiable() throws Exception
	{
		IJobDispatcher dispatcher = this.getSite().getRequiredService(IJobDispatcher.class);
		dispatcher.TryDispatch();
	}


	







	@Override
	public JobMetaData[] getUnterminates() throws Exception {
		return new Enumerable<JobMetaData>(this._queue.getUnterminates()).toArrayList().toArray(new JobMetaData[0]);
	}








	@Override
	public JobMetaData findJobMetaDataById(UUID id) throws Exception {
		return this._queue.findJobMetaDataById(id);
	}








	@Override
	public boolean isTerminated(UUID id) throws Exception {
		return _queue.isTerminated(id);
	}








	@Override
	public JobMetaData[] findTerminated(RefObject<Integer> totalCount,
			String filter, String order, int skip, int take) throws Exception {
		
		return new Enumerable<JobMetaData>(_queue.findTerminated(totalCount, filter, order, skip, take)).toArrayList().toArray(new JobMetaData[0]);
	}








	@Override
	public JobMetaData[] findUnterminated(RefObject<Integer> totalCount,
			String filter, String order, int skip, int take) throws Exception {
		return new Enumerable<JobMetaData>(_queue.findUnterminated(totalCount, filter, order, skip, take)).toArrayList().toArray(new JobMetaData[0]);
	}








	@Override
	public void add(JobMetaData job) throws Exception {
		this._queue.add(job);
		
	}








	@Override
	public void updateState(JobMetaData job, boolean isStart) throws Exception {
		this._queue.updateState(job, isStart);
		
	}








	@Override
	public void archive(JobMetaData job) throws Exception {
		this._queue.archive(job);
		
	}








	@Override
	public void resume(UUID id) throws Exception {
		this._queue.resume(id);
		
	}








	@Override
	public void cancel(UUID id) throws Exception {
		this._queue.cancel(id);
		
	}








	@Override
	public void suspend(UUID id) throws Exception {
		this._queue.suspend(id);
		
	}








	@Override
	public void userPause(UUID id) throws Exception {
		this._queue.userPause(id);
		
	}








	@Override
	public int getTerminatedCount() throws Exception {
		return this._queue.getTerminatedCount();
	}








	@Override
	public int getUnterminatedCount() throws Exception {
		
		return this._queue.getUnterminatedCount();
	}








	@Override
	public String[] getAllApplications() {
		return this._queue.getAllApplications();
	}








	@Override
	public String[] getAllUsers() {
	    return this._queue.getAllUsers();
	}




	@Override
	public void connect(String name, ISatellite satellite) throws Exception {
		
		SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		manager.RegisterSatellite(name, satellite);
	}








	@Override
	public void disconnect(String name) throws Exception {
		SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		manager.UnregisterSatellite(name);
		
	}








	@Override
	public void echo() {
		
		
	}


}