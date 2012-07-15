package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class ConsoleMessageService extends AbstractMessageService
{

	@Override
	public int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		System.out.println(text);
		return defaultResult;
	}

	@Override
	public void WriteLine(String text)
	{
		System.out.println(text);
	}
}