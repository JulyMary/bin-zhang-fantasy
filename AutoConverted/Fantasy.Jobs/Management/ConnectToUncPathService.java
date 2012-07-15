package Fantasy.Jobs.Management;

import Fantasy.IO.*;
import Fantasy.ServiceModel.*;

public class ConnectToUncPathService extends AbstractService
{

	@Override
	public void InitializeService()
	{
		this.ConnectTo(JobManagerSettings.getDefault().getJobDirectoryFullPath(), JobManagerSettings.getDefault().getJobDirectoryUser(), JobManagerSettings.getDefault().getJobDirectoryPassword());

		super.InitializeService();
	}

	private void ConnectTo(String path, String user, String password)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		Uri uri = new Uri(path);
		if (uri.IsUnc)
		{
			boolean connected = false;
			try
			{
				connected = LongPathDirectory.Exists(path);
			}
			catch (java.lang.Exception e)
			{
			}
			if (!connected)
			{
				if (logger != null)
				{
					logger.LogMessage("Unc", "Connect to UNC path {0}", path);
				}
				password = Encryption.Decrypt(password);
				WNet.AddConnection(path, user, password);
			}
			else
			{
				if (logger != null)
				{
					logger.LogMessage("Unc", "UNC path {0} has already been connected.", path);
				}
			}
		}
	}
}