package fantasy.jobs.resources;

public class MutexResourceProvider extends ObjectWithSite implements IResourceProvider
{

	private java.util.ArrayList<Resource> _allocated = new java.util.ArrayList<Resource>();
	private Object _syncRoot = new Object();
	private IGlobalMutexService _globalSvc;

	public final boolean CanHandle(String name)
	{
		return String.equals(name, "mutex", StringComparison.OrdinalIgnoreCase);
	}

	public final void Initialize()
	{
		_globalSvc = this.Site.<IGlobalMutexService>GetService();
	}

	public final boolean IsAvailable(ResourceParameter parameter)
	{
		String key = parameter.getValues().get("key");
		boolean isGlobal = Boolean.parseBoolean(parameter.getValues().GetValueOrDefault("global", "true"));

		boolean globalAvaile;

		if(isGlobal && _globalSvc != null)
		{
			globalAvaile = _globalSvc.IsAvaiable(key);
		}
		else
		{
			globalAvaile = true;
		}

		if (globalAvaile)
		{
			synchronized (_syncRoot)
			{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				return _allocated.BinarySearchBy(key, r => r.getKey(), StringComparer.OrdinalIgnoreCase) < 0;
			}
		}
		else
		{
			return false;
		}
	}

	public final boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{
		resource.argvalue = null;
		String key = parameter.getValues().get("key");


		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			int n = _allocated.BinarySearchBy(key, r => r.getKey(), StringComparer.OrdinalIgnoreCase);
			if (n < 0)
			{

				boolean isGlobal = Boolean.parseBoolean(parameter.getValues().GetValueOrDefault("global", "true"));

				boolean globalAvaile;

				TimeSpan timeout = TimeSpan.Parse(parameter.getValues().GetValueOrDefault("timeout", "00:15:00"));

				if (isGlobal && _globalSvc != null)
				{
					globalAvaile = _globalSvc.Request(key, timeout);
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

	public final void Release(Object resource)
	{
		Resource res = (Resource)resource;
		boolean available = false;
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			int n = _allocated.BinarySearchBy(res.getKey(), r => r.getKey(), StringComparer.OrdinalIgnoreCase);
			if (n >= 0)
			{
				_allocated.remove(n);

				if (res.getIsGlobal() && _globalSvc != null)
				{
					_globalSvc.Release(res.getKey());
				}
				available = true;
			}

		}

		if (available)
		{
			this.OnAvailable(EventArgs.Empty);
		}
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


//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<ProviderRevokeArgs> Revoke;

	protected void OnRevoke(ProviderRevokeArgs e)
	{
		if (this.Revoke != null)
		{
			this.Revoke(this, e);
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