package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.collections.*;
import fantasy.*;

public class MutexResourceProvider extends ResourceProvider implements IResourceProvider
{

	private java.util.ArrayList<Resource> _allocated = new java.util.ArrayList<Resource>();
	private Object _syncRoot = new Object();
	private IGlobalMutexService _globalSvc;

	@Override
	public final boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("mutex", name);
		
	}

	@Override
	public void initialize() throws Exception
	{
		_globalSvc = this.getSite().getService(IGlobalMutexService.class);
	}

	public final boolean isAvailable(ResourceParameter parameter) throws Exception
	{
		String key = parameter.getValues().get("key");
		boolean isGlobal = Boolean.parseBoolean(MapUtils.getValueOrDefault(parameter.getValues(), "global", "true"));

		boolean globalAvaile;

		if(isGlobal && _globalSvc != null)
		{
			globalAvaile = _globalSvc.isAvaiable(key);
		}
		else
		{
			globalAvaile = true;
		}

		if (globalAvaile)
		{
			synchronized (_syncRoot)
			{
				return CollectionUtils.binarySearchBy(this._allocated, key, new Selector<Resource, String>(){

					@Override
					public String select(Resource item) {
						return item.getKey();
					}}, String.CASE_INSENSITIVE_ORDER) < 0;
			}
		}
		else
		{
			return false;
		}
	}

	
	private long parseTimeout(String s)
	{
		return Long.parseLong(s);
	}
	
	public final boolean request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{
		resource.argvalue = null;
		String key = parameter.getValues().get("key");


		synchronized (_syncRoot)
		{

			int n = CollectionUtils.binarySearchBy(this._allocated, key, new Selector<Resource, String>(){

				@Override
				public String select(Resource item) {
					return item.getKey();
				}}, String.CASE_INSENSITIVE_ORDER);
			if (n < 0)
			{

				boolean isGlobal = Boolean.parseBoolean(MapUtils.getValueOrDefault(parameter.getValues(), "global", "true"));

				boolean globalAvaile;

				long timeout = this.parseTimeout(MapUtils.getValueOrDefault(parameter.getValues(),"timeout", "900000"));

				if (isGlobal && _globalSvc != null)
				{
					globalAvaile = _globalSvc.request(key, timeout);
				}
				else
				{
					globalAvaile = true;
				}

				if (globalAvaile)
				{
					Resource tempVar = new Resource();
					tempVar.setKey(key);
					tempVar.setIsGlobal(isGlobal);
					Resource res = tempVar;
					_allocated.add(~n, res);
					resource.argvalue = res;
					return true;
				}
				else
				{
					return false;
				}


			}
			else
			{
				return false;
			}
		}
	}

	public final void release(Object resource) throws Exception
	{
		Resource res = (Resource)resource;
		boolean available = false;
		synchronized (_syncRoot)
		{
			int n = CollectionUtils.binarySearchBy(this._allocated, res.getKey(), new Selector<Resource, String>(){

				@Override
				public String select(Resource item) {
					return item.getKey();
				}}, String.CASE_INSENSITIVE_ORDER);
			if (n >= 0)
			{
				_allocated.remove(n);

				if (res.getIsGlobal() && _globalSvc != null)
				{
					_globalSvc.release(res.getKey());
				}
				available = true;
			}

		}

		if (available)
		{
			this.onAvailable();
		}
	}




	private static class Resource
	{
		private String privateKey;
		public final String getKey()
		{
			return privateKey;
		}
		public final void setKey(String value)
		{
			privateKey = value;
		}

		private boolean privateIsGlobal;
		public final boolean getIsGlobal()
		{
			return privateIsGlobal;
		}
		public final void setIsGlobal(boolean value)
		{
			privateIsGlobal = value;
		}
	}

}