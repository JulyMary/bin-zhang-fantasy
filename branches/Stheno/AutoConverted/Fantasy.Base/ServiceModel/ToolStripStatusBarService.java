package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class ToolStripStatusBarService implements IStatusBarService
{

	private ToolStripStatusLabel _label;
	public ToolStripStatusBarService(ToolStripStatusLabel label)
	{
		if ((label == null))
		{
			throw new ArgumentNullException("label");
		}
		this._label = label;
	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IStatusBarService Members

	public final void SetStatus(String status)
	{
		if (_label.Owner.InvokeRequired)
		{
			MethodInvoker<String> del = new MethodInvoker<String>(SetStatus);
			_label.Owner.invoke(del, status);
		}
		else
		{
			this._label.setText(status);

		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}