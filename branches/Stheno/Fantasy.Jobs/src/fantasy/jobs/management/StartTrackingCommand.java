package fantasy.jobs.management;

import fantasy.servicemodel.*;
import fantasy.tracking.*;

public class StartTrackingCommand implements ICommand
{


	public final Object Execute(Object args) throws Exception
	{
		TrackingConfiguration.StartTrackingService();
		return null;
	}

}