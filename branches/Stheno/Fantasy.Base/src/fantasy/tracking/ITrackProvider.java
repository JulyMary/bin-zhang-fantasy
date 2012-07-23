package fantasy.tracking;
import java.util.*;

import fantasy.*;
public interface ITrackProvider extends IDisposable
{
	UUID getId();
	String getName();
	String getCategory();

	Object getProperty(String name);
	<T> T getProperty(String name, T defaultValue);
	void setProperty(String name, Object value);

	String[] getPropertyNames();

	
}