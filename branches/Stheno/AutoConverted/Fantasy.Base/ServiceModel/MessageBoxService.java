package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class MessageBoxService extends AbstractMessageService
{


	private Control _owner = null;
	public MessageBoxService()
	{
		//_owner = owner;
	}

	private int _result;
	@Override
	public int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{

		if ((this._owner != null) && this._owner.InvokeRequired)
		{
			ShowMessageHandler del = new ShowMessageHandler(ShowMessage);
			this._owner.invoke(del, owner, text, caption, buttons, icon, defaultButton, options);
		}
		else
		{
			this.ShowMessage(owner, text, caption, buttons, icon, defaultButton, options);
		}

		return _result;
	}

	private void ShowMessage(System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		this._result = MessageBox.Show(owner, text, caption, buttons, icon, defaultButton, options);
	}

//C# TO JAVA CONVERTER TODO TASK: Delegates are not available in Java:
//	private delegate void ShowMessageHandler(System.Windows.Forms.IWin32Window owner, string text, string caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options);


	@Override
	public void WriteLine(String text)
	{
	}
}