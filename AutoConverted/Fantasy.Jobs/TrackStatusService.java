package Fantasy.Jobs;

import Fantasy.ServiceModel.*;
import Fantasy.Tracking.*;

public class TrackStatusService extends AbstractService implements IStatusBarService
{

	private ITrackProvider _track;

	@Override
	public void InitializeService()
	{
		_track = this.Site.<ITrackProvider>GetService();
		super.InitializeService();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IStatusBarService Members

	public final void SetStatus(String status)
	{
		_track["status"] = status;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}