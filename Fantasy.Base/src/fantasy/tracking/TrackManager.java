package fantasy.tracking;

import java.util.*;
import java.rmi.*;
import java.rmi.server.*;



class TrackManager extends UnicastRemoteObject implements ITrackManager, IRefreshable, ITrackManagerServiceHandler
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -5466040119213635893L;


	private ITrackManagerService _remoteManager;

	ITrackManagerService getRemoteManager()
	{
		return this._remoteManager;
	}

	private String _uri;
	private Object _syncRoot = new Object();

	private UUID _token = UUID.randomUUID();

	public TrackManager(String uri) throws RemoteException
	{
		super();

		this._uri = uri;
		this.tryCreateRemote();
		RefreshManager.register(this);
	}

	private void tryCreateRemote()
	{
		synchronized (_syncRoot)
		{
			try
			{

				this._remoteManager = (ITrackManagerService)Naming.lookup(this._uri);

				this._remoteManager.addHandler(this._token,this);

				this._remoteManager.echo();

			}
			catch (Exception error)
			{
				_remoteManager = null;
			}
		}
	}


	public final TrackMetaData[] getActiveTrackMetaData()
	{


		TrackMetaData[] rs = new TrackMetaData[0];

		try
		{
			if (this._remoteManager!= null)
			{
				rs = _remoteManager.getActiveTrackMetaData();
			}
		}
		catch(Exception error)
		{

			_remoteManager = null;
		}

		return rs;
	}

	@Override
	public final void refresh()
	{
		if(_disposed) return;
		if (this._remoteManager != null)
		{
			try
			{
				this._remoteManager.echo();
			}
			catch(Exception error)
			{

				this._remoteManager = null;
			}
		}

		if (this._remoteManager == null)
		{
			this.tryCreateRemote();
		}
		
	}

	@Override
	public final void handleTrackActived(TrackMetaData track)
	{
		TrackActiveEventObject e = new TrackActiveEventObject(this, track);
		synchronized(this._handlers)
		{
			for(ITrackActiveEventListener handler : this._handlers)
			{
				handler.HandleActive(e);
			}
		}
	}




	public boolean echo() {

		return true;
	}


	private HashSet<ITrackActiveEventListener> _handlers = new HashSet<ITrackActiveEventListener>();

	@Override
	public void addHandler(ITrackActiveEventListener handler) {
		synchronized(this._handlers)
		{
			_handlers.add(handler);
		}

	}

	@Override
	public void removeHandler(ITrackActiveEventListener handler) {
		synchronized(this._handlers)
		{
			_handlers.remove(handler);
		}

	}


	private boolean _disposed = false;

	@Override
	public void Dispose() {
		if(_disposed)
		{
			_disposed = true;
			RefreshManager.unregister(this);
			ITrackManagerService tm = this._remoteManager;
			if(tm != null)
			{
				try
				{
					tm.removeHandler(this._token);

				}
				catch(Exception e)
				{
				}

				this._remoteManager = null;
			}

		}

	}

}