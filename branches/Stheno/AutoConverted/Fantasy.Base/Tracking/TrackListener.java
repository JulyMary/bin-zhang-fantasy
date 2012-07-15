package Fantasy.Tracking;

import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
public class TrackListener extends TrackBase implements ITrackListener, IRefreshable, ITrackListenerServiceHandler
{
	private ClientRef<ITrackListenerService> _wcfListener;
	private Uri _uri;
	private String _configurationName;

	private boolean _isActive = false;

	private Object _syncRoot = new Object();

	public TrackListener(TrackFactory connection, String configurationName, Uri uri, Guid id)
	{
		this.setConnection(connection);
		this._configurationName = configurationName;
		this._uri = uri;
		this.setId(id);

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task task = Task.Factory.StartNew(() =>
		{
			this.TryCreateWCF(true);
			RefreshManager.Register(this);
		}
	   );
		task.Wait(500);

	}

	private void TryCreateWCF(boolean init)
	{
		synchronized (_syncRoot)
		{
			try
			{
				//InstanceContext context = new InstanceContext(this);
				//if (this._configurationName != null && this._uri != null)
				//{
				//    this._wcfListener = new TrackListenerClient(context, this._configurationName, this._uri.ToString());
				//}
				//else
				//{
				//    this._wcfListener = new TrackListenerClient(context);
				//}
				this._wcfListener = ClientFactory.<ITrackListenerService>CreateDuplex(this);


				TrackMetaData data = this._wcfListener.getClient().GetMetaData(this.getId());
				if (data != null)
				{
					this.setName(data.getName());
					this.setCategory(data.getCategory());

					for (TrackProperty prop : this._wcfListener.getClient().GetProperties())
					{
						Object value = TrackProperty.ToObject(prop);
						if (init)
						{
							this.Data.put(prop.getName(), value);
						}
						else
						{
							this.setItem(prop.getName(), value);
						}
					}
				}
				this.setIsActived(true);

			}
			catch (RuntimeException error)
			{

				if (WCFExceptionHandler.CanCatch(error))
				{
					this._wcfListener = null;
					this.setIsActived(false);
				}
				else
				{
					throw error;
				}

			}

		}
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IRefreshable Members

	public final void Refresh()
	{
		if (this._wcfListener != null)
		{

			try
			{
				this._wcfListener.getClient().Echo();
			}
			catch (java.lang.Exception e)
			{
				this._wcfListener = null;
			}

		}
		if (this._wcfListener == null)
		{
			this.TryCreateWCF(false);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IDisposable Members

	public final void dispose()
	{
		RefreshManager.Unregister(this);
		if (this._wcfListener != null)
		{
			try
			{

				this._wcfListener.dispose();
			}
			catch (java.lang.Exception e)
			{
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListenerServiceHandler Members

	public final void HandlePropertyChanged(TrackProperty property)
	{
		Object value = TrackProperty.ToObject(property);
		//System.Diagnostics.Debug.WriteLine("{0}: {1}", property.Name, value); 
		this.setItem(property.getName(), value);
	}

	@Override
	public Object getItem(String name)
	{
		return super[name];
	}
	@Override
	public void setItem(String name, Object value)
	{
		super[name] = value;
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListenerServiceHandler Members

	public final void Connect()
	{
	}

	public final void Echo()
	{

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListenerServiceHandler Members


	public final void HandleActived(TrackMetaData metaData, TrackProperty[] properties)
	{
		this.setName(metaData.getName());
		this.setCategory(metaData.getCategory());

		for (TrackProperty prop : properties)
		{
			Object value = TrackProperty.ToObject(prop);
			this.setItem(prop.getName(), value);
		}
		this.setIsActived(true);

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackListener Members


	public final boolean getIsActived()
	{
		return this._isActive;
	}
	public final void setIsActived(boolean value)
	{
		if (this._isActive != value)
		{
			this._isActive = value;
			if (this.ActiveStateChanged != null)
			{
				this.ActiveStateChanged(this, EventArgs.Empty);
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler ActiveStateChanged;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}