package fantasy.jobs.resources;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.*;

import fantasy.collections.*;
import fantasy.jobs.*;
import fantasy.servicemodel.*;

public class ResourceService extends AbstractService implements IResourceService, IResourceConsumer
{

	

	/**
	 * 
	 */
	private static final long serialVersionUID = -6009847731989870135L;

	public ResourceService() throws RemoteException {
		super();
		
	}


	private java.util.HashMap<UUID, ResourceHandle> _resources = new java.util.HashMap<UUID, ResourceHandle>();

	public final IResourceHandle Request(ResourceParameter[] parameters) throws Exception
	{

		ResourceHandle rs = InnerRequest(parameters);
		if (rs != null)
		{
			rs.setSuspendEngine(true);
			return rs;
		}
		else
		{
			IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
			IResourceRequestQueue queue = this.getSite().getRequiredService(IResourceRequestQueue.class);
			queue.RegisterResourceRequest(engine.getJobId(), parameters);
			ILogger logger = this.getSite().getService(ILogger.class);
			String newLine = System.getProperty("line.separator");
			Log.SafeLogMessage(logger, "Resource", "Suspend job engine because it failed to request resource:" + newLine + StringUtils2.join(newLine, new Enumerable<ResourceParameter>(parameters)));
			Suspend();
			return null;
		}
	}

	private void Suspend() throws Exception
	{
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		engine.Suspend();
		if(Thread.interrupted())
		{
			throw new InterruptedException();
		}

	}

	private ResourceHandle InnerRequest(ResourceParameter[] parameters) throws Exception
	{
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		IResourceManager manager = this.getSite().getRequiredService(IResourceManager.class);
		UUID resId = manager.Request(engine.getJobId(), parameters);
		if (!resId.equals(UUIDUtils.Empty))
		{
			ResourceHandle tempVar = new ResourceHandle();
			tempVar.setId(resId);
			tempVar.setResourceService(this);
			tempVar.setParameters(parameters);
			ResourceHandle rs = tempVar;
			synchronized (this._resources)
			{
				this._resources.put(resId, rs);
			}
			return rs;
		}
		else
		{
			return null;
		}
	}

	public final IResourceHandle TryRequest(ResourceParameter[] parameters) throws Exception
	{
		return InnerRequest(parameters);
	}

	public final void Release(IResourceHandle resource) throws Exception
	{
		if (resource == null)
		{
			throw new IllegalArgumentException("resource");
		}
		IResourceManager manager = this.getSite().getRequiredService(IResourceManager.class);
		manager.Release(((ResourceHandle)resource).getId());
	}



	@Override
	public void initializeService() throws Exception
	{
		IResourceManager manager = this.getSite().getRequiredService(IResourceManager.class);
		manager.addConsumer(this);
		super.initializeService();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		IResourceManager manager = this.getSite().getRequiredService(IResourceManager.class);
		IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
		manager.removeConsumer(engine.getJobId());
	}

	public void Revoke(UUID id) throws Exception
	{
		ResourceHandle resource;
		synchronized (_resources)
		{
			resource = this._resources.get(id);
		}
		if (resource != null)
		{
			try
			{
				RevokeArgs args = new RevokeArgs(this);
				resource.OnRevoke(args);

				if (resource.getSuspendEngine() && args.getSuspendEngine())
				{
					ILogger logger = this.getSite().getService(ILogger.class);
					if (logger != null)
					{
						
						String newLine = System.getProperty("line.separator");
						logger.LogMessage("Resource", "Suspend job engine because a resource is revoked." + newLine + StringUtils2.join(newLine, new Enumerable<ResourceParameter>(resource.getParameters())));
					}

					IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
					IResourceRequestQueue queue = this.getSite().getRequiredService(IResourceRequestQueue.class);
					queue.RegisterResourceRequest(engine.getJobId(), resource.getParameters());
					Suspend();
				}
			}
			finally
			{
				resource.dispose();
			}
		}
	}


	public UUID Id()
	{
        UUID rs = null;
		IJobEngine engine = null;
		try {
			engine = this.getSite().getRequiredService(IJobEngine.class);
			rs = engine.getJobId();
		} catch (Exception e) {
			
		}
		return rs;

	}

}