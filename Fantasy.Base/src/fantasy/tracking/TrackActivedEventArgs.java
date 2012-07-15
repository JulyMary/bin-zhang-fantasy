package fantasy.tracking;

public class TrackActivedEventArgs extends EventArgs
{
	public TrackActivedEventArgs(TrackMetaData data)
	{
		this.setMetaData(data);
	}

	private TrackMetaData privateMetaData;
	public final TrackMetaData getMetaData()
	{
		return privateMetaData;
	}
	private void setMetaData(TrackMetaData value)
	{
		privateMetaData = value;
	}
}