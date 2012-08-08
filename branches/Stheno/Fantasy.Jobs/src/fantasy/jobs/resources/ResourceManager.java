package fantasy.jobs.resources;

import java.rmi.RemoteException;
import java.util.*;
import java.util.concurrent.*;

import fantasy.*;

import fantasy.collections.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;

public class ResourceManager extends AbstractService implements IResourceManager
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -2680085641081482311L;

	public ResourceManager() throws RemoteException {
		super();
		
	}


	protected IResourceProvider[] _providers;

	private java.util.ArrayList<ResourceGroup> _allocatedResources = new java.util.ArrayList<ResourceGroup>();

	private ScheduledExecutorService _scheduler;

	private void CheckJobsExist() throws Exception
	{
		
		
		IJobController controller = this.getSite().getRequiredService(IJobController.class);

		ILogger logger = this.getSite().getService(ILogger.class);


		for (ResourceGroup resource : _allocatedResources.toArray(new ResourceGroup[]{}))
		{
			try
			{
				if (!controller.IsJobProccessExisted(resource.getJobId()))
				{
					if (logger != null)
					{
						logger.LogMessage("ResourceManager", "Job %1$s has exited, force release resource.", resource.getJobId());
					}

					Release(resource);
				}
			}
			catch (java.lang.Exception e)
			{

			}
		}
		

	}

	@Override
	public void initializeService() throws Exception
	{
		_providers = AddIn.CreateObjects(IResourceProvider.class, "jobService/resources/provider");
		for (IResourceProvider provider : this._providers)
		{
			if (provider instanceof IObjectWithSite)
			{
				((IObjectWithSite)provider).setSite(this.getSite());
			}
			
			
			provider.addListener(new IResourceProviderListener(){

				@Override
				public void Available(EventObject e) {
					ResourceManager.this.ProviderResourceAvailable(e);
					
				}

				@Override
				public void Revoke(ProviderRevokeArgs e) throws Exception {
					ResourceManager.this.ProviderResourceRevoke(e);
					
				}});

			provider.initialize();
		}

		super.initializeService();
       
		_scheduler = Executors.newScheduledThreadPool(1);
		_scheduler.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				try {
					ResourceManager.this.CheckJobsExist();
				} catch (Exception e) {
				
				}
			}}, 15, 15, TimeUnit.SECONDS);
		
		_scheduler.shutdown();
		


	}


	private int _availableLock = 0;
	private Object _availableSyncRoot = new Object();

	private void LockAvaialbe()
	{
		synchronized (_availableSyncRoot)
		{
			_availableLock++;
		}
	}

	private void UnlockAvaiable()
	{
		boolean fire = false;
		synchronized (_availableSyncRoot)
		{
			if (_availableLock > 0)
			{
				_availableLock--;
				fire = _availableLock == 0;
			}

		}
		if (fire)
		{
			this.OnAvailable(new EventObject(this));
		}
	}

	private void ProviderResourceRevoke(final ProviderRevokeArgs e) throws Exception
	{
		this.LockAvaialbe();
		try
		{
			Iterable<IGrouping<UUID, ResourceGroup>> groups;
			synchronized (_resSyncRoot)
			{
				groups = new Enumerable<ResourceGroup>(this._allocatedResources).where(new Predicate<ResourceGroup>(){

					@Override
					public boolean evaluate(ResourceGroup rg) throws Exception {
						return rg.ContainsResource(e.getResource());
					}}).groupBy(new Selector<ResourceGroup, UUID>(){

						@Override
						public UUID select(ResourceGroup item) {
							return item.getJobId();
						}});

			}
			
			for(final IGrouping<UUID, ResourceGroup> g : groups)
			{
				fantasy.ThreadFactory.queueUserWorkItem(new Runnable(){

					@Override
					public void run() {
						for (ResourceGroup r : g)
						{
							IResourceConsumer handler = null;
							synchronized (ResourceManager.this._consumers)
							{
								handler = ResourceManager.this._consumers.get(g.getKey());
							}
							if (handler != null)
							{
								if (!r.getReleased())
								{
									try {
										handler.Revoke(r.getId());
									} catch (Exception e) {
										
									}
								}
							}
							else
							{
								break;
							}
						}
						
					}});
			}

		  
		}
		finally
		{
			this.UnlockAvaiable();
		}

	}

	private void ProviderResourceAvailable(EventObject e)
	{
		this.LockAvaialbe();
		this.UnlockAvaiable();

	}

	
	private HashSet<IResourceManagerListener> _listeners = new HashSet<IResourceManagerListener>();

	protected void OnAvailable(EventObject e)
	{
		for(IResourceManagerListener listener : this._listeners)
		{
			listener.available(e);
		}
	}


	private void Release(ResourceGroup res) throws Exception
	{
		this.LockAvaialbe();
		try
		{

			this._allocatedResources.remove(res);
			res.Release();
		}
		finally
		{
			this.UnlockAvaiable();
		}
	}


	public void Release(final UUID id) throws Exception
	{
		synchronized (_resSyncRoot)
		{
			ResourceGroup res = new Enumerable<ResourceGroup>(this._allocatedResources).singleOrDefault(new Predicate<ResourceGroup>(){

				@Override
				public boolean evaluate(ResourceGroup rg) throws Exception {
					return rg.getId().equals(id);
				}});
		
			if (res != null)
			{
				Release(res);
			}
		}
	}

	protected Object _resSyncRoot = new Object();

	@Override
	public void uninitializeService() throws Exception
	{
		
		super.uninitializeService();
		for (IResourceProvider provider : this._providers)
		{
			if (provider instanceof IDisposable)
			{
				((IDisposable)provider).dispose();
			}
		}
	}

	public UUID Request(UUID jobId, ResourceParameter[] parameters) throws Exception
	{
		UUID rs = UUIDUtils.Empty;
		if (parameters == null)
		{
			throw new IllegalArgumentException("parameters");
		}
		synchronized (this._resSyncRoot)
		{
			ResourceGroup group = new ResourceGroup(jobId, parameters);
			group.setSite(this.getSite());
			try
			{

				boolean available = true;
                
				for(ResourceParameter param : parameters)
				{
					for(IResourceProvider provider : this._providers)
					{
						if(provider.canHandle(param.getName()))
						{
							Object resource = null;
							RefObject<Object> tempRef_resource = new RefObject<Object>(resource);
							available = provider.request(param, tempRef_resource);
							resource = tempRef_resource.argvalue;
							if (available)
							{
								if (resource != null)
								{
									group.AddResource(provider, resource);
								}
							}
							else
							{
								break;
							}
						}
					}
				}
				
				
			
				if (available)
				{
					rs = group.getId();

					this._allocatedResources.add(group);
				}
				else
				{
					group.Release();
				}
			}

			catch(Exception error)
			{
				group.Release();
				if(! (error instanceof InterruptedException))
				{
					ILogger logger = this.getSite().getService(ILogger.class);
					if (logger != null)
					{
						logger.LogError("ResourceManager", error, "An error occurs when request a resource.");
					}
				}
				else
				{
					throw error;
				}
				
			}
		}

		return rs;
	}




	public final boolean IsAvailable(ResourceParameter[] parameters) throws Exception
	{
		synchronized (_resSyncRoot)
		{
			for(ResourceParameter param : parameters)
			{
				for(IResourceProvider provider : this._providers)
				{
					if(provider.canHandle(param.getName()))
					{
						if(!provider.isAvailable(param))
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}

	private java.util.HashMap<UUID,IResourceConsumer> _consumers = new java.util.HashMap<UUID,IResourceConsumer>();

	public final void addConsumer(IResourceConsumer handler)
	{
		synchronized (_consumers)
		{
			_consumers.put(handler.Id(), handler);
		}
	}

	public final void removeConsumer(UUID id)
	{
		synchronized (_consumers)
		{
			_consumers.remove(id);
		}
	}

	@Override
	public void addListener(IResourceManagerListener listener) {
		this._listeners.add(listener);
		
	}

	@Override
	public void removeListener(IResourceManagerListener listener) {
		this._listeners.remove(listener);
		
	}


}