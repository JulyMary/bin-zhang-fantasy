package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;

public abstract class CapacityResourceProviderBase extends ResourceProvider implements IResourceProvider
{
	protected Object _syncRoot = new Object();

	protected void tryRevoke() throws Exception
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
			int max = getMaxCount(group.getKey());
			int count = group.toEnumerable().count();
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

	public abstract boolean canHandle(String name);

	protected String getKey(ResourceParameter parameter)
	{
		return "";
	}

	protected abstract int getMaxCount(String key) throws Exception;

	private int getAllocatedCount(final String key) throws Exception
	{
		
		int rs = new Enumerable<Resource>(this.getAllocated()).where(new Predicate<Resource>(){

			@Override
			public boolean evaluate(Resource obj) throws Exception {
				return StringUtils.equals(obj.Key, key);
			}}).count();

		
		return rs;
	}


	public boolean isAvailable(ResourceParameter parameter) throws Exception
	{
		synchronized (_syncRoot)
		{
			String key = this.getKey(parameter);
			int count = this.getAllocatedCount(key);
			int max = this.getMaxCount(key);

			return count < max;
		}
	}

	public boolean request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{
		synchronized (_syncRoot)
		{
			if (isAvailable(parameter))
			{
				Resource tempVar = new Resource();
				tempVar.Key = this.getKey(parameter);
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

	public final void release(Object resource) throws Exception
	{
		boolean available;
		synchronized (_syncRoot)
		{
			Resource res = (Resource)resource;
			this._allocated.remove(res);
			int max = this.getMaxCount(res.Key);
			if (max < Integer.MAX_VALUE)
			{

				int count = getAllocatedCount(res.Key);
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