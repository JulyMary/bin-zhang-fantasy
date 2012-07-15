package Fantasy.Jobs;

import Fantasy.ServiceModel.*;
import Fantasy.Tracking.*;

public class TrackProgressService extends AbstractService implements IProgressMonitor
{

	private ITrackProvider _track;

	@Override
	public void InitializeService()
	{
		_track = this.Site.<ITrackProvider>GetService();
		super.InitializeService();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IProgressMonitor Members

	public final int getValue()
	{
		return _track.GetProperty("progress.value", 0);
	}
	public final void setValue(int value)
	{
		_track["progress.value"] = value;
	}

	public final int getMaximum()
	{
		return _track.GetProperty("progress.maximum", 100);
	}
	public final void setMaximum(int value)
	{
		_track["progress.maximum"] = value;
	}

	public final int getMinimum()
	{
		return _track.GetProperty("progress.minimum", 0);
	}
	public final void setMinimum(int value)
	{
		_track["progress.minimum"] = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IProgressMonitor Members


	public final ProgressMonitorStyle getStyle()
	{
		return _track.GetProperty("progress.style", ProgressMonitorStyle.Blocks);
	}
	public final void setStyle(ProgressMonitorStyle value)
	{
		_track["progress.style"] = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}