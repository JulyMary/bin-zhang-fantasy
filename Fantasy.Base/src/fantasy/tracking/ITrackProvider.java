package fantasy.tracking;
import java.util.*;

import fantasy.*;
public interface ITrackProvider extends IDisposable
{
	UUID getId();
	String getName();
	String getCategory();

	Object getItem(String name);
	void setItem(String name, Object value);

	String[] getPropertyNames();

	
}