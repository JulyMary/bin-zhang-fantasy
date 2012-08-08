package fantasy.jobs.resources;

import java.util.*;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;

import fantasy.jobs.management.*;

import fantasy.runtime.caching.SimpleCache;

public class WaitForResourceProvider extends ResourceProvider implements IResourceProvider
{

	private SimpleCache<ArrayList<UUID>> _cache = new SimpleCache<ArrayList<UUID>>(60*60, 60*60);

	private Object _syncRoot = new Object();

	

	public final boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("WaitFor", name);
		
	}

	public final void initialize()
	{
		
	}

	private String getKey(String ids) throws Exception
	{
	
		return MD5HashCodeHelper.compute(ids);
	}

	public final boolean isAvailable(ResourceParameter parameter) throws Exception
	{

		String ids = parameter.getValues().get("jobs");
		if (!StringUtils2.isNullOrEmpty(ids))
		{

			WaitForMode model = WaitForMode.valueOf(MapUtils.getValueOrDefault(parameter.getValues(), "mode", "All"));
			String key = model.toString() + this.getKey(ids);
			java.util.ArrayList<UUID> waitList;
			synchronized (_syncRoot)
			{
				waitList = this._cache.get(key);
				if (waitList == null)
				{
					waitList = new Enumerable<String>(StringUtils2.split(ids, ";",true)).select(new Selector<String, UUID>(){

						@Override
						public UUID select(String item) {
							
							return UUID.fromString(item);
						}}).toArrayList();  
				}

				_cache.put(key, waitList);
			}

			if (waitList.size() > 0)
			{
				synchronized (waitList)
				{
					java.util.ArrayList<UUID> temp = new java.util.ArrayList<UUID>(waitList);
					IJobQueue queue = this.getSite().getRequiredService(IJobQueue.class);

					if (model == WaitForMode.All)
					{
						for (UUID id : temp)
						{
							
							if (!queue.IsTerminated(id))
							{
								return false;
							}
							else
							{
								waitList.remove(id);
							}
						}
						return true;
					}
					else
					{
						for (UUID id : temp)
						{
							if (queue.IsTerminated(id))
							{
								waitList.clear();
								return true;
							}
						}
						return false;
					}
				}
			}
			else
			{
				return true;
			}

		}
		else
		{
			return true;
		}

	}

	public final boolean request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{
		resource.argvalue = null;
		return this.isAvailable(parameter);
	}

	public final void release(Object resource)
	{

	}
}