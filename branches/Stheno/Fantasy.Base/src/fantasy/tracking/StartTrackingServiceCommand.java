package fantasy.tracking;

import fantasy.servicemodel.ICommand;

public class StartTrackingServiceCommand implements ICommand{

	@Override
	public Object Execute(Object args) throws Exception {
		TrackingConfiguration.StartTrackingService();
		return null;
	}

}
