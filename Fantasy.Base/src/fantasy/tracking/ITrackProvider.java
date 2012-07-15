package fantasy.tracking;
import fantasy.*;
import java.util.*;
public interface ITrackProvider extends IDisposable
{
	UUID getId();
	String getName();
	String getCategory();

	Object getItem(String name);
	void setItem(String name, Object value);

	String[] getPropertyNames();

	TrackFactory getConnection();
}