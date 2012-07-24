package fantasy.jobs.management;

import Fantasy.Tracking.*;

public class StartTrackingCommand implements ICommand
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ICommand Members

	public final Object Execute(Object args)
	{
		TrackingConfiguration.StartTrackingService();
		return null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}