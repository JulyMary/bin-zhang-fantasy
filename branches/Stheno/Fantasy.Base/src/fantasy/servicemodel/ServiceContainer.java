﻿package fantasy.servicemodel;

import fantasy.*;

@SuppressWarnings({"rawtypes"})
public class ServiceContainer implements IServiceProvider
{
	private IServiceProvider _parentProvider;

	public ServiceContainer()
	{

	}

	private boolean _initialized = false;
	private java.util.ArrayList<Object> _list = new java.util.ArrayList<Object>();

	public final void initializeServices(Object[] services) throws Exception
	{
		this.initializeServices(null, services);
	}

	public void initializeServices(IServiceProvider parentProvider, Object[] services) throws Exception
	{
		this._parentProvider = parentProvider;

		
		for(Object s1 : services)
		{
		    this._list.add(s1);
		}

		ILogger logger = (ILogger)this.getService2(ILogger.class);


		for (Object o : this._list)
		{
			if(Thread.interrupted())
			{
				throw new InterruptedException();
			}
			if (o instanceof IObjectWithSite)
			{
				((IObjectWithSite)o).setSite(this);
			}
			if (o instanceof IService)
			{
				
				try
				{
					((IService)o).initializeService();
				}
				catch(Exception error)
				{
					Log.SafeLogError(logger, "Services", error, "Service {0} failed to initialize.", o.getClass().getName());
					throw error;
				}

				if (logger != null)
				{
					logger.LogMessage("Services", "Service {0} initialized", o.getClass().getName());
				}
			}
		}
		this._initialized = true;
	}

	public void AddService(Object service) throws Exception
	{
		if (service == null)
		{
			throw new IllegalArgumentException("service");
		}



		this._list.add(service);
		if (service instanceof IObjectWithSite)
		{
			((IObjectWithSite)service).setSite(this);
		}
		if (service instanceof IService && this._initialized)
		{
			((IService)service).initializeService();
		}

	}

	public void removeService(Object service) throws Exception
	{
		int index = this._list.indexOf(service);
		if (index >= 0)
		{
			this._list.remove(index);
			if (service instanceof IService)
			{
				((IService)service).uninitializeService();
			}
		}


	}

	public void uninitializeServices() throws Exception
	{
		ILogger logger = (ILogger)this.getService2(ILogger.class);
		for (int i = this._list.size() - 1; i >= 0; i--)
		{
			Object o = this._list.get(i);
			if (o instanceof IService)
			{
				if (logger != null)
				{
					logger.LogMessage("Services", "Service {0} uninitialized", o.getClass().getName());
				}
				((IService)o).uninitializeService();

			}
		}
	}

	


	public Object getService2(java.lang.Class serviceType) throws Exception
	{
		if (serviceType == null)
		{
			return new IllegalArgumentException("serviceType");
		}
		for (Object o : this._list)
		{
			if (serviceType.isInstance(o))
			{
				return o;
			}
		}

		if (this._parentProvider != null)
		{
			return this._parentProvider.getService2(serviceType);
		}

		return null;
	}

	@Override
	public Object getRequiredService2(Class serviceType) {
		Object rs = this.getRequiredService2(serviceType);
		if(rs == null)
		{
			throw new MissingRequiredServiceException(serviceType);
		}
		return null;
	}

	@SuppressWarnings("unchecked")
	@Override
	public <T> T getService(Class<T> serviceType) throws Exception {
		return (T)this.getService2(serviceType);
		
	}

	@SuppressWarnings("unchecked")
	@Override
	public <T> T getRequiredService(Class<T> serviceType) {
		return (T)this.getRequiredService2(serviceType);
	}


}