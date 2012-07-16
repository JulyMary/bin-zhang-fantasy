package fantasy.tracking;

import java.util.*;
import fantasy.*;

public interface ITrackListener extends  IDisposable
{
	UUID getId();
	String getName();
	String getCategory();

	Object getItem(String name);

	String[] getPropertyNames();

	TrackFactory getConnection();

	boolean getIsActived();

	//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
	//	event EventHandler ActiveStateChanged;

	//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
	//	event EventHandler<TrackChangedEventArgs> Changed;
}