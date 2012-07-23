package fantasy.tracking;

import java.util.*;
import fantasy.*;

public interface ITrackListener extends  IDisposable
{
	UUID getId();
	String getName();
	String getCategory();

	Object getProperty(String name);
	
	<T> T getProperty(String name, T defaultValue);

	String[] getPropertyNames();
	
	void addHandler(ITrackListenerHandler handler);
	
	void removeHandler(ITrackListenerHandler handler);
}