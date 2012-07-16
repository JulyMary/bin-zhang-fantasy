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
}