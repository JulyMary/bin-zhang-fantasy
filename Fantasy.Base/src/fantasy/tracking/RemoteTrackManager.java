package fantasy.tracking;

public class RemoteTrackManager implements IRefreshable
{

	private RemoteTrackManager()
	{
		RefreshManager.Register(this);
	}


	private static RemoteTrackManager _manager = new RemoteTrackManager();

	public static RemoteTrackManager getManager()
	{
		return _manager;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IRefreshable Members

	private Object _syncroot = new Object();

	public final void Refresh()
	{
		synchronized (_syncroot)
		{
			for (java.util.Map.Entry<UUID, WeakReference> pair : new java.util.ArrayList<java.util.Map.Entry<UUID, WeakReference>>(this._remoteTracks))
			{
				if (!pair.getValue().IsAlive)
				{
					this._remoteTracks.remove(pair.getKey());
				}
			}
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

	private java.util.HashMap<UUID, WeakReference> _remoteTracks = new java.util.HashMap<UUID, WeakReference>();

	public final RemoteTrack FindTrack(UUID id)
	{
		RemoteTrack rs = null;
		synchronized (_syncroot)
		{
			WeakReference wr = null;
			if ((wr = _remoteTracks.get(id)) != null)
			{
				if (wr.IsAlive)
				{
					rs = (RemoteTrack)wr.Target;
				}
			}
		}

		return rs;
	}

	public final boolean CreateTrack(UUID id, String name, String category, java.util.Map<String, Object> values, boolean reconnect, RefObject<RemoteTrack> track)
	{

		synchronized (_syncroot)
		{
			RemoteTrack t = FindTrack(id);
			if (!reconnect && t != null)
			{
				track.argvalue = null;
				return false;
			}

			if (t != null)
			{
				if (values != null)
				{
					for (java.util.Map.Entry<String, Object> pair : values)
					{
						t[pair.getKey()] = pair.getValue();
					}
				}
			}
			else
			{
				t = new RemoteTrack(id, name, category, values);
				_remoteTracks.put(id, new WeakReference(t, true));
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				Task.Factory.StartNew(() =>
				{
					OnAdded(t);
				}
			   );
			}

			track.argvalue = t;

		}


		return true;
	}



	private java.util.ArrayList<WeakReference> _handlers = new java.util.ArrayList<WeakReference>();

	public final void AddHandler(ITrackManagerServiceHandler handler)
	{
		synchronized (_handlers)
		{
			this._handlers.add(new WeakReference(handler, true));
		}
	}

	public final void RemoveHandler(ITrackManagerServiceHandler handler)
	{
		synchronized (_handlers)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			WeakReference wr = this._handlers.Find(w => (ITrackManagerServiceHandler)w.Target == handler);
			if (wr != null)
			{
				this._handlers.remove(wr);
			}

		}
	}

	public final TrackMetaData[] GetAllTrackMetaData()
	{
		java.util.ArrayList<TrackMetaData> rs = new java.util.ArrayList<TrackMetaData>();
		java.util.ArrayList<WeakReference> tracks;
		synchronized (this._remoteTracks)
		{
			tracks = new java.util.ArrayList<WeakReference>(this._remoteTracks.values());
		}


		for(WeakReference wr : tracks)
		{
			if (wr.IsAlive)
			{
				RemoteTrack t = (RemoteTrack)wr.Target;
				TrackMetaData tempVar = new TrackMetaData();
				tempVar.setId(t.getId());
				tempVar.setName(t.getName());
				tempVar.setCategory(t.getCategory());
				rs.add(tempVar);
			}
		}


		return rs.toArray(new TrackMetaData[]{});
	}

	private void OnAdded(RemoteTrack track)
	{
		TrackMetaData tempVar = new TrackMetaData();
		tempVar.setId(track.getId());
		tempVar.setName(track.getName());
		tempVar.setCategory(track.getCategory());
		TrackMetaData data = tempVar;
		ConcurrentQueue<WeakReference> expires = new ConcurrentQueue<WeakReference>();
		java.util.ArrayList<WeakReference> handlers;
		synchronized (_handlers)
		{
			handlers = new java.util.ArrayList<WeakReference>(this._handlers);
		}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Parallel.ForEach(handlers, wr =>
			{
				try
				{
					if (wr.IsAlive)
					{
						ITrackManagerServiceHandler handler = (ITrackManagerServiceHandler)wr.Target;
						handler.HandleTrackActived(data);
					}
					else
					{
						expires.Enqueue(wr);
					}
				}
				catch (java.lang.Exception e)
				{
					expires.Enqueue(wr);
				}
			}
		   );
			synchronized (_handlers)
			{
				for (WeakReference wr : expires)
				{
					this._handlers.remove(wr);
				}
			}
		}
	   );
	}

	public final void RemoveTrack(RemoteTrack remoteTrack)
	{
		synchronized (_remoteTracks)
		{
			this._remoteTracks.remove(remoteTrack.getId());
		}
	}
}