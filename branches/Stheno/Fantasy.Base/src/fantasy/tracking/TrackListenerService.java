package fantasy.tracking;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Reentrant, Namespace=Consts.NamespaceUri)]
public class TrackListenerService implements ITrackListenerService, IRemoteTrackHandler, IRefreshable, ITrackManagerServiceHandler
{
	private ITrackListenerServiceHandler _handler;
	private RemoteTrack _track;
	private UUID _id = new UUID();

	public TrackListenerService()
	{
		_handler = OperationContext.Current.<ITrackListenerServiceHandler>GetCallbackChannel();
	}



	public final TrackProperty[] GetProperties()
	{
		return this._track.GetTrackProperties();
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListenerService Members


	public final void Echo()
	{
		if (this._track == null || this._track.HandlerAdded(this) == false)
		{
			throw new FaultException<CallbackExpiredException>(new CallbackExpiredException(), new FaultReason("Handle has been removed."));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListenerService Members


	public final TrackMetaData GetMetaData(UUID id)
	{
		TrackMetaData rs = null;
		this._id = id;


		RefreshManager.Register(this);

		this._track = RemoteTrackManager.getManager().FindTrack(id);
		if (this._track != null)
		{
			this._track.AddHandler(this);
			TrackMetaData tempVar = new TrackMetaData();
			tempVar.setId(id);
			tempVar.setName(this._track.getName());
			tempVar.setCategory(this._track.getCategory());
			rs = tempVar;
		}

		RemoteTrackManager.getManager().AddHandler(this);
		return rs;

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion




//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IRefreshable Members

	public final void Refresh()
	{
		if (_handler == null)
		{
			throw new ObjectDisposedException("TrackListenerService");
		}
		try
		{
			_handler.Echo();
		}
		catch (java.lang.Exception e)
		{
			this._handler = null;
			this.dispose();

		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IDisposable Members

	public final void dispose()
	{
		RefreshManager.Unregister(this);
		if (_track != null)
		{
			_track.RemoveHandler(this);
			RemoteTrackManager.getManager().RemoveHandler(this);
			_track = null;
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackManagerServiceHandler Members

	public final void HandleTrackActived(TrackMetaData meta)
	{
		if (meta.getId().equals(this._id))
		{
			if (this._track != null)
			{
				this._track.RemoveHandler(this);
			}

			this._track = RemoteTrackManager.getManager().FindTrack(this._id);
			this._track.AddHandler(this);
			try
			{
				this._handler.HandleActived(meta, this._track.GetTrackProperties());
			}
			catch (java.lang.Exception e)
			{
				this.dispose();
			}
		}

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion


	private void HandleChanged(TrackChangedEventArgs e)
	{
		TrackProperty prop = TrackProperty.Create(e.getName(), e.getNewValue());
		try
		{
			this._handler.HandlePropertyChanged(prop);
		}
		catch (java.lang.Exception e)
		{
			this.dispose();
		}
	}

}