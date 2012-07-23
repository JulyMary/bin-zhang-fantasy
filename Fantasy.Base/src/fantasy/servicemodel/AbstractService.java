package fantasy.servicemodel;

import java.rmi.RemoteException;
import java.rmi.server.*;
import java.util.*;

import fantasy.*;

public abstract class AbstractService extends UnicastRemoteObject implements IService, IObjectWithSite
{



	public AbstractService() throws RemoteException {
		super();
	}


	private static final long serialVersionUID = 2406667265617843257L;
	public void initializeService() throws Exception
	{
		EventObject e = new EventObject(this);
		for(IServiceListener listener : this._listeners)
		{
			listener.serviceInitialized(e);
		}
	}

	public void uninitializeService() throws Exception
	{
		EventObject e = new EventObject(this);
		for(IServiceListener listener : this._listeners)
		{
			listener.serviceUninitialized(e);
		}
	}
	
	
	private HashSet<IServiceListener> _listeners = new  HashSet<IServiceListener>();
	

	public void addServiceListener(IServiceListener listener)
	{
		this._listeners.add(listener);
	}
	
	public void removeServiceListener(IServiceListener listener)
	{
		this._listeners.remove(listener);
	}

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

}