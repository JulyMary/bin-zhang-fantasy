package Fantasy.Tracking;

public interface ITrackProvider extends IDisposable
{
	Guid getId();
	String getName();
	String getCategory();

Object getItem(String name);
	void setItem(String name, Object value);

	String[] getPropertyNames();

	TrackFactory getConnection();
}