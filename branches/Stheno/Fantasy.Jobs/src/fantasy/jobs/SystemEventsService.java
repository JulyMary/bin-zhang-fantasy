package fantasy.jobs;

import Microsoft.Win32.*;
import fantasy.servicemodel.*;

public class SystemEventsService extends AbstractService
{

	private Thread _hiddenFormThread;

	@Override
	public void InitializeService()
	{
		if (Application.OpenForms.size() == 0)
		{
			_waitHandler = new ManualResetEvent(false);
			_hiddenFormThread = ThreadFactory.CreateThread(this.StartApplication).WithStart();

			_waitHandler.WaitOne();
			_waitHandler.dispose();
		}

		super.InitializeService();


	}

	private ManualResetEvent _waitHandler;

	private void StartApplication()
	{
		HiddenForm form = new HiddenForm() { };
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		form.HandleCreated += new EventHandler(form_HandleCreated);
		Application.Run(form);
	}

	private void form_HandleCreated(Object sender, EventArgs e)
	{
		_waitHandler.Set();
	}

	@Override
	public void UninitializeService()
	{

		super.UninitializeService();
		if (this._hiddenFormThread != null)
		{
			Application.Exit();
		}
	}

	private static class HiddenForm extends Form
	{

	}
}