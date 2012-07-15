package Fantasy.Tracking;

public class TrackFactory
{

	private Uri _baseUri;
	private String _configurationName;

	private static final String ProviderName = "ITrackProvider";

	private static final String ListenerName = "ITrackListener";

	private static final String ManagerName = "ITrackManager";

	public TrackFactory(String configurationName, Uri baseUri)
	{
		this._configurationName = configurationName;
		this._baseUri = baseUri;
	}

	public TrackFactory()
	{
	}


	public final ITrackProvider CreateProvider(Guid id, String name, String category, java.util.Map<String, Object> values)
	{
		Uri uri = this._baseUri != null ? new Uri(String.format("%1$s/%2$s/", this._baseUri, ProviderName)) : null;
		TrackProvider rs = new TrackProvider(this, this._configurationName, uri, id, name, category, values);
		return rs;
	}

	public final ITrackListener CreateTrackListener(Guid id)
	{
		Uri uri = this._baseUri != null ? new Uri(String.format("%1$s/%2$s/", this._baseUri, ListenerName)) : null;
		TrackListener rs = new TrackListener(this, this._configurationName, uri, id);
		return rs;
	}

	public final ITrackManager CreateManager()
	{
		Uri uri = this._baseUri != null ? new Uri(String.format("%1$s/%2$s/", this._baseUri, ManagerName)) : null;
		TrackManager rs = new TrackManager(this, this._configurationName, uri);
		return rs;
	}
}