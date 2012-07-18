package fantasy.servicemodel;

import java.util.*;

public interface IServiceListener  {

	
	void serviceInitialized(EventObject e);
	void serviceUninitialized(EventObject e);
}
