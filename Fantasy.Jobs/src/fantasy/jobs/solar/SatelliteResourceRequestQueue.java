package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import fantasy.*;


public class SatelliteResourceRequestQueue extends AbstractService implements IResourceRequestQueue
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -8619149247103605913L;

	public SatelliteResourceRequestQueue() throws RemoteException {
		super();
		// TODO Auto-generated constructor stub
	}

	private ISolarActionQueue _actionQueue;

	@Override
	public void initializeService() throws Exception
	{
		this._actionQueue = this.getSite().getRequiredService(ISolarActionQueue.class);
		super.initializeService();
	}


	public final void registerResourceRequest(final UUID jobId, final ResourceParameter[] parameters) throws Exception
	{
		this._actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.registerResourceRequest(jobId, parameters);
				
			}});
	}

	public final void unregisterResourceRequest(final UUID jobId) throws Exception
	{
		
		this._actionQueue.enqueue(new Action1<ISolar>(){

			@Override
			public void call(ISolar solar) throws Exception {
				solar.unregisterResourceRequest(jobId);
				
			}});

	}

	public final ResourceParameter[] getRequiredResources(UUID jobId) throws Exception
	{

		ISolar client = ClientFactory.create(ISolar.class);
	    return client.getRequiredResources(jobId);
		
	}

}