package fantasy.tracking;

import Fantasy.ServiceModel.*;

public class TrackManager implements ITrackManager, IRefreshable, ITrackManagerServiceHandler
{
	private ClientRef<ITrackManagerService> _wcfManager;
	private Uri _uri;
	private String _configurationName;
	private Object _syncRoot = new Object();

	public TrackManager(TrackFactory connection, String configurationName, Uri uri)
	{
		this.setConnection(connection);
		this._configurationName = configurationName;
		this._uri = uri;
		this.TryCreateWCF();
		RefreshManager.Register(this);
	}

	private void TryCreateWCF()
	{
		synchronized (_syncRoot)
		{
			try
			{
				//InstanceContext context = new InstanceContext(this);
				//if (this._configurationName != null && this._uri != null)
				//{
				//    this._wcfManager = new TrackManagerClient(context, this._configurationName, this._uri.ToString());
				//}
				//else
				//{
				//    this._wcfManager = new TrackManagerClient(context);
				//}
				this._wcfManager = ClientFactory.<ITrackManagerService>CreateDuplex(this);

				this._wcfManager.getClient().Echo();

			}
			catch (RuntimeException error)
			{
				WCFExceptionHandler.CatchKnowns(error);
				this._wcfManager = null;
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackManager Members

	public final TrackMetaData[] GetActivedTrackMetaData()
	{

		try
		{
			if (this._wcfManager!= null)
			{
				return _wcfManager.getClient().GetActivedTrackMetaData();
			}
		}
		catch(RuntimeException error)
		{
			WCFExceptionHandler.CatchKnowns(error);
			_wcfManager = null;
		}

		return new TrackMetaData[0];
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<TrackActivedEventArgs> TrackActived;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IRefreshable Members

	public final void Refresh()
	{
		if (this._wcfManager != null)
		{
			try
			{
				this._wcfManager.getClient().Echo();
			}
			catch(RuntimeException error)
			{
				WCFExceptionHandler.CatchKnowns(error);
				this._wcfManager = null;
			}
		}

		if (this._wcfManager == null)
		{
			this.TryCreateWCF();
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IDisposable Members

	public final void dispose()
	{
		RefreshManager.Unregister(this);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackManagerServiceHandler Members

	public final void HandleTrackActived(TrackMetaData track)
	{
		if (this.TrackActived != null)
		{
			this.TrackActived(this, new TrackActivedEventArgs(track));
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	private TrackFactory privateConnection;
	public final TrackFactory getConnection()
	{
		return privateConnection;
	}
	private void setConnection(TrackFactory value)
	{
		privateConnection = value;
	}
}