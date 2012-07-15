package Fantasy.Tracking;

public class RemoteTrack extends TrackBase
{
	public RemoteTrack(Guid id, String name, String category, java.util.Map<String, Object> values)
	{
		this.setId(id);
		this.setName(name);
		this.setCategory(category);
		this.InitializeData(values);
	}

	private java.util.ArrayList<IRemoteTrackHandler> _handlers = new java.util.ArrayList<IRemoteTrackHandler>();

	public final void AddHandler(IRemoteTrackHandler handler)
	{
		synchronized (_handlers)
		{
			if (this._handlers.indexOf(handler) <= 0)
			{
				this._handlers.add(handler);
			}
		}
	}

	public final void RemoveHandler(IRemoteTrackHandler handler)
	{
		synchronized (_handlers)
		{
			this._handlers.remove(handler);
		}
	}


	public final boolean HandlerAdded(IRemoteTrackHandler handler)
	{
		synchronized (_handlers)
		{
			return this._handlers.indexOf(handler) >= 0;
		}
	}



	@Override
	protected void OnChanged(TrackChangedEventArgs e)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			java.util.ArrayList<IRemoteTrackHandler> handlers;
			synchronized (_handlers)
			{
				handlers = new java.util.ArrayList<IRemoteTrackHandler>(this._handlers);
			}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Parallel.ForEach(handlers, handler =>
			{
				try
				{
					handler.HandleChanged(e);
				}
				catch (java.lang.Exception e)
				{
					synchronized (_handlers)
					{
						this._handlers.remove(handler);
					}
				}
			}
		   );
		}
	   );
		super.OnChanged(e);
	}

	public final TrackProperty[] GetTrackProperties()
	{
		TrackProperty[] rs;
		java.util.ArrayList<java.util.Map.Entry<String, Object>> props;
		synchronized (this.Data)
		{
			props = new java.util.ArrayList<java.util.Map.Entry<String, Object>>(this.Data);
		}

		rs = new TrackProperty[props.size()];
		for (int i = 0; i < props.size(); i++)
		{

			TrackProperty prop = TrackProperty.Create(props.get(i).getKey(), props.get(i).getValue());
			rs[i] = prop;
		}


		return rs;
	}




}