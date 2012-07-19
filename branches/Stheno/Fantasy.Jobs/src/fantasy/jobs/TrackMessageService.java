package fantasy.jobs;

import fantasy.servicemodel.*;
import Fantasy.Tracking.*;

public class TrackMessageService extends AbstractMessageService implements IService, IObjectWithSite
{
	@Override
	public int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		if (_track != null)
		{
			_track["message"] = text;

		}

		return defaultResult;
	}

	@Override
	public void WriteLine(String text)
	{
		_track["message"] = text;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IService Members

	private ITrackProvider _track;

	public final void InitializeService()
	{
		_track = this.getSite().<ITrackProvider>GetService();
		if (Initialize != null)
		{
			Initialize(this, EventArgs.Empty);
		}
	}

	public final void UninitializeService()
	{
		if (Uninitialize != null)
		{
			Uninitialize(this, EventArgs.Empty);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Initialize;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Uninitialize;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IObjectWithSite Members

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}