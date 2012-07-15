package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

public class ResourceManager extends AbstractService implements IResourceManager
{
	protected IResourceProvider[] _providers;

	private java.util.ArrayList<ResourceGroup> _allocatedResources = new java.util.ArrayList<ResourceGroup>();

	private Thread _checkThread;

	private void CheckJobsExist()
	{
		IJobController controller = this.Site.<IJobController>GetRequiredService();
		TimeSpan timeout = new TimeSpan(0, 5, 0);
		ILogger logger = this.Site.<ILogger>GetService();
		while (true)
		{
			Thread.sleep(15000);
			java.util.Date now = new java.util.Date();
			for (ResourceGroup resource : _allocatedResources.toArray(new ResourceGroup[]{}))
			{
				try
				{
					if (!controller.IsJobProccessExisted(resource.getJobId()))
					{
						if (logger != null)
						{
							logger.LogMessage("ResourceManager", "Job {0} has exited, force release resource.", resource.getJobId());
						}

						Release(resource);
					}
				}
				catch (java.lang.Exception e)
				{

				}
			}
		}

	}

	@Override
	public void InitializeService()
	{
		_providers = AddIn.<IResourceProvider>CreateObjects("jobService/resources/provider");
		for (IResourceProvider provider : this._providers)
		{
			if (provider instanceof IObjectWithSite)
			{
				((IObjectWithSite)provider).Site = this.Site;
			}
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			provider.Available += new EventHandler(ProviderResourceAvailable);
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
			provider.Revoke += new EventHandler<ProviderRevokeArgs>(ProviderResourceRevoke);
			provider.Initialize();
		}

		super.InitializeService();

		_checkThread = ThreadFactory.CreateThread(this.CheckJobsExist).WithStart();


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
			this.OnAvailable(EventArgs.Empty);
		}
	}

	private void ProviderResourceRevoke(Object sender, ProviderRevokeArgs e)
	{
		this.LockAvaialbe();
		try
		{
			IGrouping<Guid, ResourceGroup>[] groups;
			synchronized (_resSyncRoot)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				groups = (from r in this._allocatedResources where r.ContainsResource(e.getResource()) group r by r.JobId into g select g).toArray();

			}

			//foreach(IGrouping<Guid, ResourceGroup> g in groups)
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Parallel.ForEach(groups, g =>
			{
				for (ResourceGroup r : g)
				{
					IResourceManagerHandler handler = null;
					synchronized (_handlers)
					{
						handler = this._handlers.GetValueOrDefault(g.getKey());
					}
					if (handler != null)
					{
						if (!r.getReleased())
						{
							handler.Revoke(r.getId());
						}
					}
					else
					{
						break;
					}
				}
			}
		   );
		}
		finally
		{
			this.UnlockAvaiable();
		}

	}

	private void ProviderResourceAvailable(Object sender, EventArgs e)
	{
		this.LockAvaialbe();
		this.UnlockAvaiable();

	}


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Available;

	protected void OnAvailable(EventArgs e)
	{
		if (this.Available != null)
		{
			this.Available(this, e);
		}
	}


	private void Release(ResourceGroup res)
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


	public void Release(Guid id)
	{
		synchronized (_resSyncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			ResourceGroup res = this._allocatedResources.Where(r => id.equals(r.Id)).SingleOrDefault();
			if (res != null)
			{
				Release(res);
			}
		}
	}

	protected Object _resSyncRoot = new Object();

	@Override
	public void UninitializeService()
	{
		_checkThread.stop();
		super.UninitializeService();
		for (IResourceProvider provider : this._providers)
		{
			if (provider instanceof IDisposable)
			{
				((IDisposable)provider).dispose();
			}
		}
	}

	public Guid Request(Guid jobId, ResourceParameter[] parameters)
	{
		Guid rs = Guid.Empty;
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		synchronized (this._resSyncRoot)
		{
			ResourceGroup group = new ResourceGroup(jobId, parameters);
			group.Site = this.Site;
			try
			{

				boolean available = true;
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
				var query = from param in parameters from provider in this._providers where provider.CanHandle(param.getName()) select new { param = param, provider = provider };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
				for (var test : query)
				{
					Object resource = null;
					RefObject<Object> tempRef_resource = new RefObject<Object>(resource);
					available = test.provider.Request(test.param, tempRef_resource);
					resource = tempRef_resource.argvalue;
					if (available)
					{
						if (resource != null)
						{
							group.AddResource(test.provider, resource);
						}
					}
					else
					{
						break;
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

			catch(RuntimeException error)
			{
				if(! (error instanceof ThreadAbortException))
				{
					ILogger logger = this.Site.<ILogger>GetService();
					if (logger != null)
					{
						logger.LogError("ResourceManager", error, "An error occurs when request a resource.");
					}
				}
				group.Release();
			}
		}

		return rs;
	}




	public final boolean IsAvailable(ResourceParameter[] parameters)
	{
		synchronized (_resSyncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			var query = from param in parameters from provider in this._providers where provider.CanHandle(param.getName()) select new { param = param, provider = provider };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
			for (var test : query)
			{
				if (test.provider.IsAvailable(test.param) == false)
				{
					return false;
				}
			}

			return true;
		}
	}

	private java.util.HashMap<Guid,IResourceManagerHandler> _handlers = new java.util.HashMap<Guid,IResourceManagerHandler>();

	public final void RegisterHandler(IResourceManagerHandler handler)
	{
		synchronized (_handlers)
		{
			_handlers.put(handler.Id(), handler);
		}
	}

	public final void UnregisterHandler(Guid id)
	{
		synchronized (_handlers)
		{
			_handlers.remove(id);
		}
	}


}