package Fantasy.ServiceModel;

import Microsoft.VisualBasic.*;

public class CompositeMessageService extends AbstractMessageService
{

	private IMessageService[] _services;
	public CompositeMessageService(IMessageService[] services)
	{
		this._services = (IMessageService[])services.clone();
	}
	@Override
	public int Show(int defaultResult, System.Windows.Forms.IWin32Window owner, String text, String caption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, System.Windows.Forms.MessageBoxDefaultButton defaultButton, System.Windows.Forms.MessageBoxOptions options)
	{
		int rs = defaultResult;
		for (IMessageService service : this._services)
		{
			int dr = service.Show(defaultResult, owner, text, caption, buttons, icon, defaultButton, options);
			if ((dr != defaultResult))
			{
				rs = dr;
			}
		}
		return rs;
	}

	@Override
	public void WriteLine(String text)
	{
		for (IMessageService service : this._services)
		{
			service.WriteLine(text);
		}
	}
}