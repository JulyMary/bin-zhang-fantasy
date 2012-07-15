package fantasy.tracking;

import Fantasy.ServiceModel.*;

public class TrackProvider extends TrackBase implements ITrackProvider, IRefreshable
{
	private TrackFactory privateConnection;
	public final TrackFactory getConnection()
	{
		return privateConnection;
	}
	private void setConnection(TrackFactory value)
	{
		privateConnection = value;
	}

	private ClientRef<ITrackProviderService> _wcfProvider;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning disable 0414
	private boolean _created = false;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 0414

	private Uri _uri;
	private String _configurationName;

	private Object _syncRoot = new Object();

	public TrackProvider(TrackFactory connection, String configurationName, Uri uri, UUID id, String name, String category, java.util.Map<String, Object> values)
	{
		this.setConnection(connection);
		this._configurationName = configurationName;
		this._uri = uri;
		this.setId(id);
		this.setName(name);
		this.setCategory(category);
		this.InitializeData(values);

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task task = Task.Factory.StartNew(() =>
		{
			this.TryCreateWCF();
			RefreshManager.Register(this);
		}
	   );
		task.Wait(100);
	}

	private void TryCreateWCF()
	{
		synchronized (_syncRoot)
		{
			try
			{


				this._wcfProvider = ClientFactory.<ITrackProviderService>Create();
				java.util.ArrayList<java.util.Map.Entry<String, Object>> values;
				synchronized (this.Data)
				{
					 values = new java.util.ArrayList<java.util.Map.Entry<String, Object>>(this.Data);
				}
				TrackProperty[] props = new TrackProperty[values.size()];
				for (int i = 0; i < values.size(); i++)
				{
				   props[i] = TrackProperty.Create(values.get(i).getKey(), values.get(i).getValue());
				}

				this._wcfProvider.getClient().CreateTrackProvider(this.getId(), this.getName(), this.getCategory(), props, true);
				this._created = true;

			}
			catch (RuntimeException error)
			{
				if (!WCFExceptionHandler.CanCatch(error))
				{
					throw error;
				}
				this._wcfProvider = null;
			}
		}
	}

	@Override
	protected void OnChanged(TrackChangedEventArgs e)
	{
		if (this._wcfProvider != null)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			MethodInvoker invoker = new MethodInvoker(() =>
			{
				try
				{
					TrackProperty prop = TrackProperty.Create(e.getName(), e.getNewValue());
					this._wcfProvider.getClient().SetProperty(prop);
				}
				catch (RuntimeException err)
				{
					WCFExceptionHandler.CatchKnowns(err);
					_wcfProvider = null;
				}
			}
		   );

			invoker.BeginInvoke(null, null);

		}

		super.OnChanged(e);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IRefreshable Members

	public final void Refresh()
	{
		if (this._wcfProvider != null)
		{

				try
				{
					this._wcfProvider.getClient().Echo();
				}
				catch (RuntimeException error)
				{
					WCFExceptionHandler.CatchKnowns(error);
					this._wcfProvider = null;
				}



		}
		if (this._wcfProvider == null)
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
}