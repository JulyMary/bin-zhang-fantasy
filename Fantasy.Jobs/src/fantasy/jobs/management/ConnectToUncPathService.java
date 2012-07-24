package fantasy.jobs.management;

import java.rmi.RemoteException;

import fantasy.servicemodel.*;

public class ConnectToUncPathService extends AbstractService
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 1555321187703356023L;

	public ConnectToUncPathService() throws RemoteException {
		super();
		
	}

	@Override
	public void initializeService() throws Exception
	{
		this.ConnectTo(JobManagerSettings.getDefault().getJobDirectoryFullPath(), JobManagerSettings.getDefault().getJobDirectoryUser(), JobManagerSettings.getDefault().getJobDirectoryPassword());

		super.initializeService();
	}

	private void ConnectTo(String path, String user, String password)
	{
		//TODO: find java connect to unc path method;
	/*	ILogger logger = this.getSite().getService(ILogger.class);
		java.nio.file.Path uri = java.nio.file.Paths.get(path);
		if ()
		{
			boolean connected = false;
			try
			{
				connected =Directory.exists(path);
			}
			catch (java.lang.Exception e)
			{
			}
			if (!connected)
			{
				if (logger != null)
				{
					logger.LogMessage("Unc", "Connect to UNC path %1$s", path);
				}
				password = Encryption.Decrypt(password);
				//TODO: add UNC connection;
				//WNet.AddConnection(path, user, password);
			}
			else
			{
				if (logger != null)
				{
					logger.LogMessage("Unc", "UNC path %1$s has already been connected.", path);
				}
			}
		}*/
	}
}