package Fantasy.Tracking;

public class TrackProviderService implements ITrackProviderService
{

	private RemoteTrack _remoteTrack;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITrackProviderService Members

	public final void CreateTrackProvider(Guid id, String name, String category, TrackProperty[] properties, boolean reconnect)
	{
		java.util.HashMap<String, Object> values = new java.util.HashMap<String, Object>();
		if(properties != null)
		{
			for(TrackProperty prop : properties)
			{
				values.put(prop.getName(), TrackProperty.ToObject(prop));
			}

		}

		RefObject<RemoteTrack> tempRef__remoteTrack = new RefObject<RemoteTrack>(_remoteTrack);
		boolean tempVar = !RemoteTrackManager.getManager().CreateTrack(id, name, category, values, reconnect, tempRef__remoteTrack);
			_remoteTrack = tempRef__remoteTrack.argvalue;
		if (tempVar)
		{
			throw new RuntimeException();
		}
	}

	public final void SetProperty(TrackProperty property)
	{
		if (_remoteTrack != null)
		{
			_remoteTrack[property.getName()] = TrackProperty.ToObject(property);
		}
	}


	public final void Echo()
	{

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}