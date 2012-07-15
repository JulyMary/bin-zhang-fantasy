package Fantasy.Jobs.Resources;

import Fantasy.ServiceModel.*;

public class ResourceRequestQueue extends AbstractService implements IResourceRequestQueue
{

	private java.util.HashMap<Guid, ResourceParameter[]> _queue = new java.util.HashMap<Guid, ResourceParameter[]>();

	private Object _syncRoot = new Object();


	public final void RegisterResourceRequest(Guid jobId, ResourceParameter[] parameters)
	{
		synchronized (_syncRoot)
		{
			if (!_queue.containsKey(jobId))
			{
				_queue.put(jobId, parameters);
			}
		}
	}

	public final void UnregisterResourceRequest(Guid jobId)
	{
		synchronized (_syncRoot)
		{
			this._queue.remove(jobId);
		}
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceRequestQueue Members


	public final ResourceParameter[] GetRequiredResources(Guid jobId)
	{
		ResourceParameter[] rs;
		synchronized (_syncRoot)
		{
			rs = this._queue.GetValueOrDefault(jobId, new ResourceParameter[] { });
		}
		return rs;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}