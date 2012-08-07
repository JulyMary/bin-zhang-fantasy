package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;

public abstract class CapacityResourceProviderBase extends ResourceProvider implements IResourceProvider
{
	protected Object _syncRoot = new Object();

	protected void TryRevoke() throws Exception
	{
		java.util.ArrayList<Resource> temp;
		synchronized (_syncRoot)
		{
			temp = new java.util.ArrayList<Resource>(this._allocated);
		}

		Enumerable<IGrouping<String,Resource>> query = new Enumerable<Resource>(temp).groupBy(new Selector<Resource, String>(){

			@Override
			public String select(Resource item) {
				return item.Key;
			}});

		for (IGrouping<String,Resource> group : query)
		{
			int max = GetMaxCount(group.getKey());
			int count = group.count();
			if (count > max)
			{
				for (Resource res : group.toEnumerable().reverse().take(count - max))
				{
					try
					{
						ProviderRevokeArgs e = new ProviderRevokeArgs(this);
						e.setResource(res);
						this.onRevoke(e);
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

	protected String GetKey(ResourceParameter parameter)
	{
		return "";
	}

	protected abstract int GetMaxCount(String key) throws Exception;

	private int GetAllocatedCount(final String key) throws Exception
	{
		
		int rs = new Enumerable<Resource>(this.getAllocated()).where(new Predicate<Resource>(){

			@Override
			public boolean evaluate(Resource obj) throws Exception {
				return StringUtils.equals(obj.Key, key);
			}}).count();

		
		return rs;
	}


	public boolean IsAvailable(ResourceParameter parameter) throws Exception
	{
		synchronized (_syncRoot)
		{
			String key = this.GetKey(parameter);
			int count = this.GetAllocatedCount(key);
			int max = this.GetMaxCount(key);

			return count < max;
		}
	}

	public boolean Request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{
		synchronized (_syncRoot)
		{
			if (IsAvailable(parameter))
			{
				Resource tempVar = new Resource();
				tempVar.Key = this.GetKey(parameter);
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

	public final void Release(Object resource) throws Exception
	{
		boolean available;
		synchronized (_syncRoot)
		{
			Resource res = (Resource)resource;
			this._allocated.remove(res);
			int max = this.GetMaxCount(res.Key);
			if (max < Integer.MAX_VALUE)
			{

				int count = GetAllocatedCount(res.Key);
				available = count < max;
			}
			else
			{
				available = true;
			}

		}

		if (available)
		{
			this.onAvailable();
		}
	}

	
	protected static class Resource
	{
		public String Key;
	}

}