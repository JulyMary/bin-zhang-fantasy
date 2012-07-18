package fantasy.servicemodel;

import java.rmi.RemoteException;

import fantasy.*;

public class LogConsoleService extends AbstractService implements ILogListener
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 5727061828771044198L;

	public LogConsoleService() throws RemoteException {
		super();
	
	}

	@Override
	public void initializeService()
	{
		ILogger logger = (ILogger)((IServiceProvider)this.getSite()).getService(ILogger.class);
		logger.AddListener(this);
		super.initializeService();
	}

	@Override
	public void uninitializeService()
	{
		super.uninitializeService();
		ILogger logger = (ILogger)((IServiceProvider)this.getSite()).getService(ILogger.class);
		logger.RemoveListener(this);
	}


	public final void onMessage(String category, MessageImportance importance, String message)
	{
		System.out.println("Message: " + message);
	}

	public final void onWaring(String category, Throwable exception, MessageImportance importance, String message)
	{
		System.out.println("Warning: " + message);
	}

	public final void onError(String category, Throwable exception, String message)
	{
		System.out.println("Error: " + message);
	}

}