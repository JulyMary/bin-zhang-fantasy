package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class TextFileMessageService extends AbstractMessageService
{


	private String _fileName;
	public TextFileMessageService(String fileName, boolean overwrite)
	{
		if ((overwrite))
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
		}

		if (!File.Exists(fileName))
		{
			File.Create(fileName).Close();
		}

		this._fileName = fileName;

	}


	private void AppendLine(String text)
	{
		StreamWriter w = File.AppendText(this._fileName);
		w.WriteLine(text);
		w.Close();
	}

	@Override
	public int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		AppendLine(text);
		return defaultResult;
	}

	@Override
	public void WriteLine(String text)
	{
		AppendLine(text);
	}
}