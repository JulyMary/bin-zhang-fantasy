package fantasy.jobs;

import java.rmi.RemoteException;

import fantasy.servicemodel.*;
import fantasy.tracking.*;

public class TrackProgressService extends AbstractService implements IProgressMonitor
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 5436485782003964921L;

	public TrackProgressService() throws RemoteException {
		super();
		// TODO Auto-generated constructor stub
	}
	private ITrackProvider _track;

	@Override
	public void initializeService() throws Exception
	{
		_track = this.getSite().getRequiredService(ITrackProvider.class);
		super.initializeService();
	}

	public final int getValue()
	{
		return _track.getProperty("progress.value", 0);
	}
	public final void setValue(int value)
	{
		_track.setProperty("progress.value" , value);
	}

	public final int getMaximum()
	{
		return _track.getProperty("progress.maximum", 100);
	}
	public final void setMaximum(int value)
	{
		_track.setProperty("progress.maximum" ,value);
	}

	public final int getMinimum()
	{
		return _track.getProperty("progress.minimum", 0);
	}
	public final void setMinimum(int value)
	{
		_track.setProperty("progress.minimum" ,value);
	}


	public final ProgressMonitorStyle getStyle()
	{
		return _track.getProperty("progress.style", ProgressMonitorStyle.Blocks);
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		_track.setProperty("progress.style",  value);
	}

}