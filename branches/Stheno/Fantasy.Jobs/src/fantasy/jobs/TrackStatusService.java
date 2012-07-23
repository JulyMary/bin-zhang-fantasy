package fantasy.jobs;

import java.rmi.RemoteException;

import fantasy.servicemodel.*;
import fantasy.tracking.ITrackProvider;
;

public class TrackStatusService extends AbstractService implements IStatusBarService
{

	public TrackStatusService() throws RemoteException {
		super();
	
	}

	/**
	 * 
	 */
	private static final long serialVersionUID = -4750200750461639959L;
	private ITrackProvider _track;

	@Override
	public void initializeService() throws Exception
	{
		_track = this.getSite().getRequiredService(ITrackProvider.class);
		super.initializeService();
	}

	public final void setStatus(String status)
	{
		_track.setProperty("status",status);
		
	}

}