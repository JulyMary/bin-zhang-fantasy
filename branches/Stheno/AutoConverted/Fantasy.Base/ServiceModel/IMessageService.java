package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public interface IMessageService
{
	int Show(int defaultResult, IWin32Window owner, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);
	int Show(int defaultResult, String text);
	int Show(int defaultResult, IWin32Window owner, String text);
	int Show(int defaultResult, String text, String caption);
	int Show(int defaultResult, IWin32Window owner, String text, String caption);
	int Show(int defaultResult, String text, String caption, MessageBoxButtons buttons);
	int Show(int defaultResult, IWin32Window owner, String text, String caption, MessageBoxButtons buttons);
	int Show(int defaultResult, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon);
	int Show(int defaultResult, IWin32Window owner, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon);
	int Show(int defaultResult, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
	int Show(int defaultResult, IWin32Window owner, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton);
	int Show(int defaultResult, String text, String caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton, MessageBoxOptions options);


	void WriteLine(String text);
}