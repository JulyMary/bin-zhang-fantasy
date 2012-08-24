package fantasy.jobs.resources;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.collections.*;
import fantasy.servicemodel.*;

public class ResourceRequestQueue extends AbstractService implements IResourceRequestQueue
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1660546234366679099L;


	public ResourceRequestQueue() throws RemoteException {
		super();
	
	}

	private java.util.HashMap<UUID, ResourceParameter[]> _queue = new java.util.HashMap<UUID, ResourceParameter[]>();

	private Object _syncRoot = new Object();


	public final void registerResourceRequest(UUID jobId, ResourceParameter[] parameters)
	{
		synchronized (_syncRoot)
		{
			if (!_queue.containsKey(jobId))
			{
				_queue.put(jobId, parameters);
			}
		}
	}

	public final void unregisterResourceRequest(UUID jobId)
	{
		synchronized (_syncRoot)
		{
			this._queue.remove(jobId);
		}
	}

	public final ResourceParameter[] getRequiredResources(UUID jobId)
	{
		ResourceParameter[] rs;
		synchronized (_syncRoot)
		{
			rs = MapUtils.getValueOrDefault(this._queue, jobId, new ResourceParameter[] { });
		}
		return rs;
	}


}