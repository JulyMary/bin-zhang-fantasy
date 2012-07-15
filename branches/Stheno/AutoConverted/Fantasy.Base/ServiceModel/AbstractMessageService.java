package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public abstract class AbstractMessageService implements IMessageService
{

	public final int Show(int defaultResult, String text)
	{
		return this.Show(defaultResult, null, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, String text, String caption)
	{
		return this.Show(defaultResult, null, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons)
	{
		return this.Show(defaultResult, null, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
	{
		return this.Show(defaultResult, null, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton)
	{
		return this.Show(defaultResult, null, text, caption, buttons, icon, defaultButton, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		return this.Show(defaultResult, null, text, caption, buttons, icon, defaultButton, options);
	}

	public final int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text)
	{
		return this.Show(defaultResult, owner, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
	}

	public final int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption)
	{
		return this.Show(defaultResult, owner, text, null, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

	}

	public final int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons)
	{
		return this.Show(defaultResult, owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

	}

	public final int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
	{
		return this.Show(defaultResult, owner, text, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

	}

	public final int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton)
	{
		return this.Show(defaultResult, owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);

	}

	public abstract int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options);

	public abstract void WriteLine(String text);

}