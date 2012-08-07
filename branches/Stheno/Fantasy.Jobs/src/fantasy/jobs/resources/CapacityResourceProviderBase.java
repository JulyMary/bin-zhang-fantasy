package fantasy.jobs.resources;

import fantasy.*;

public abstract class CapacityResourceProviderBase extends ObjectWithSite implements IResourceProvider
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceProvider Members


	protected Object _syncRoot = new Object();

	protected void TryRevoke()
	{
		java.util.ArrayList<Resource> temp;
		synchronized (_syncRoot)
		{
			temp = new java.util.ArrayList<Resource>(this._allocated);
		}

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from res in temp group res by res.getKey() into g select g;

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var group : query)
		{
			int max = GetMaxCount(group.getKey());
			int count = group.Count();
			if (count > max)
			{
				for (Resource res : group.Reverse().Take(count - max))
				{
					try
					{
						this.OnRevoke(res);
					}
					finally
					{
						this._allocated.remove(res);
					}
				}
			}
		}

	}

	private java.util.ArrayList<Resource> _allocated = new java.util.ArrayList<Resource>();
	protected final java.util.ArrayList<Resource> getAllocated()
	{
		return _allocated;
	}

	public abstract boolean CanHandle(String name);

	public void Initialize()
	{

	}

	protected String GetKey(ResourceParameter parameter)
	{
		return "";
	}

	protected abstract int GetMaxCount(String key);

	private int GetAllocatedCount(String key)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from r in this.getAllocated() where r.getKey() == key select r;
		int rs = query.Count();
		return rs;
	}


	public boolean IsAvailable(ResourceParameter parameter)
	{
		synchronized (_syncRoot)
		{
			String key = this.GetKey(parameter);
			int count = this.GetAllocatedCount(key);
			int max = this.GetMaxCount(key);

			return count < max;
		}
	}

	public boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{
		synchronized (_syncRoot)
		{
			if (IsAvailable(parameter))
			{
				Resource tempVar = new Resource();
				tempVar.setKey(this.GetKey(parameter));
				Resource rs = tempVar;

				this.getAllocated().add(rs);
				resource.argvalue = rs;
				return true;
			}
			else
			{
				resource.argvalue = null;
				return false;
			}
		}
	}

	public final void Release(Object resource)
	{
		boolean available;
		synchronized (_syncRoot)
		{
			Resource res = (Resource)resource;
			this._allocated.remove(res);
			int max = this.GetMaxCount(res.getKey());
			if (max < Integer.MAX_VALUE)
			{

				int count = GetAllocatedCount(res.getKey());
				available = count < max;
			}
			else
			{
				available = true;
			}

		}

		if (available)
		{
			this.OnAvailable();
		}
	}

	protected void OnAvailable()
	{
		if (this.Available != null)
		{
			this.Available(this, EventArgs.Empty);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Available;

	protected void OnRevoke(Resource res)
	{
		if (this.Revoke != null)
		{
			ProviderRevokeArgs tempVar = new ProviderRevokeArgs();
			tempVar.setResource(res);
			this.Revoke(this, tempVar);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<ProviderRevokeArgs> Revoke;

	protected static class Resource
	{
		public String Key;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}