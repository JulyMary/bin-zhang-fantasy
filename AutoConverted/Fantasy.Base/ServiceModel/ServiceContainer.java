package Fantasy.ServiceModel;

public class ServiceContainer implements IServiceProvider
{
	private IServiceProvider _parentProvider;

	public ServiceContainer()
	{

	}

	private boolean _initialized = false;
	private java.util.ArrayList<Object> _list = new java.util.ArrayList<Object>();

	public final void InitializeServices(Object[] services)
	{
		this.InitializeServices(null, services);
	}

	public void InitializeServices(IServiceProvider parentProvider, Object[] services)
	{
		this._parentProvider = parentProvider;

		this._list.addAll(services);

		ILogger logger = this.<ILogger>GetService();


		for (Object o : this._list)
		{
			if (o instanceof IObjectWithSite)
			{
				((IObjectWithSite)o).setSite(this);
			}
			if (o instanceof IService)
			{
				try
				{
					((IService)o).InitializeService();
				}
				catch(RuntimeException error)
				{
					logger.SafeLogError("Services", error, "Service {0} failed to initialize.", o.getClass().FullName);
					throw error;
				}

				if (logger != null)
				{
					logger.LogMessage("Services", "Service {0} initialized", o.getClass().FullName);
				}
			}
		}
		this._initialized = true;
	}

	public void AddService(Object service)
	{
		if (service == null)
		{
			throw new ArgumentNullException("service");
		}



		this._list.add(service);
		if (service instanceof IObjectWithSite)
		{
			((IObjectWithSite)service).setSite(this);
		}
		if (service instanceof IService && this._initialized)
		{
			((IService)service).InitializeService();
		}

	}

	public void RemoveService(Object service)
	{
		int index = this._list.indexOf(service);
		if (index >= 0)
		{
			this._list.remove(index);
			if (service instanceof IService)
			{
				((IService)service).UninitializeService();
			}
		}


	}

	public void UninitializeServices()
	{
		ILogger logger = this.<ILogger>GetService();
		for (int i = this._list.size() - 1; i >= 0; i--)
		{
			Object o = this._list.get(i);
			if (o instanceof IService)
			{
				if (logger != null)
				{
					logger.LogMessage("Services", "Service {0} uninitialized", o.getClass().FullName);
				}
				((IService)o).UninitializeService();

			}
		}
	}

	public final <T> T GetService()
	{
		return (T)this.GetService(T.class);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IServiceProvider Members

	public Object GetService(java.lang.Class serviceType)
	{
		if (serviceType == null)
		{
			return new ArgumentNullException("serviceType");
		}
		for (Object o : this._list)
		{
			if (serviceType.IsInstanceOfType(o))
			{
				return o;
			}
		}

		if (this._parentProvider != null)
		{
			return this._parentProvider.GetService(serviceType);
		}

		return null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}