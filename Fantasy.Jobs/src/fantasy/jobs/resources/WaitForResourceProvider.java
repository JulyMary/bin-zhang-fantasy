package fantasy.jobs.resources;

import Fantasy.Jobs.Management.*;
import Fantasy.Jobs.Utils.*;

public class WaitForResourceProvider extends ObjectWithSite implements IResourceProvider
{

	private MemoryCache _cache;

	private Object _syncRoot = new Object();

	CacheItemPolicy tempVar = new CacheItemPolicy();
	tempVar.SlidingExpiration = new TimeSpan(1, 0, 0);
	private static final CacheItemPolicy _cachePolicy = tempVar;

	public final boolean CanHandle(String name)
	{
		return String.equals(name, "WaitFor", StringComparison.OrdinalIgnoreCase);
	}

	public final void Initialize()
	{
		_cache = new MemoryCache("WaitForResourceProvider");
	}

	private String GetKey(String ids)
	{
		MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(ids));
		return MD5HashCodeHelper.GetMD5HashCode(stream);
	}

	public final boolean IsAvailable(ResourceParameter parameter)
	{

		String ids = parameter.getValues().GetValueOrDefault("jobs", "");
		if (!DotNetToJavaStringHelper.isNullOrEmpty(ids))
		{
//ORIGINAL LINE: WaitForMode model = (WaitForMode)Enum.Parse(typeof(WaitForMode), parameter.Values.GetValueOrDefault("mode", "All"), true);
//C# TO JAVA CONVERTER WARNING: Java does not have a 'ignoreCase' parameter for the static 'valueOf' method of enum types:
			WaitForMode model = WaitForMode.valueOf(parameter.getValues().GetValueOrDefault("mode", "All"));
			String key = model.toString() + this.GetKey(ids);
			java.util.ArrayList<UUID> waitList;
			synchronized (_syncRoot)
			{
				waitList = (java.util.ArrayList<UUID>)this._cache[key];
				if (waitList == null)
				{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
					waitList = (from id in parameter.getValues().GetValueOrDefault("jobs", "").split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries) select new UUID(id)).ToList();
				}

				_cache.Add(key, waitList, _cachePolicy);
			}

			if (waitList.size() > 0)
			{
				synchronized (waitList)
				{
					java.util.ArrayList<UUID> temp = new java.util.ArrayList<UUID>(waitList);
					IJobQueue queue = this.Site.<IJobQueue>GetRequiredService();

					if (model == WaitForMode.All)
					{
						for (UUID id : temp)
						{
							JobMetaData job = queue.FindJobMetaDataById(id);
							if (! queue.IsTerminated(id))
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

	public final boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{
		resource.argvalue = null;
		return this.IsAvailable(parameter);
	}

	public final void Release(Object resource)
	{

	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Available
//		{
//			add
//			{
//			}
//			remove
//			{
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<ProviderRevokeArgs> Revoke
//		{
//			add
//			{
//			}
//			remove
//			{
//			}
//		}
}