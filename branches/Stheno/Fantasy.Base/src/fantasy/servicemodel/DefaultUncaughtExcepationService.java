package fantasy.servicemodel;

import java.lang.Thread.*;
import java.rmi.RemoteException;

import fantasy.properties.Resources;


public class DefaultUncaughtExcepationService extends AbstractService implements UncaughtExceptionHandler
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 5331709356676821476L;




	public DefaultUncaughtExcepationService() throws RemoteException
	{
        
	}



	private boolean isKnownError(Throwable error)
	{
		boolean rs = false;
		if (error != null)
		{
			if (error instanceof InterruptedException)
			{
				rs = true;
			}
			else if(WCFExceptionHandler.canCatch(error))
			{
				rs = true;
			}
		}
		return rs;
	}






	@Override
	public void uncaughtException(Thread t, Throwable error) {
		
		if (! isKnownError(error))
		{
			if (this.getSite() != null)
			{
				ILogger logger;
				try {
					logger = (ILogger)this.getSite().getService2(ILogger.class);
				
				if (logger != null)
				{

					logger.LogError(Resources.getSystemCategory(), error, Resources.getUnhandledExceptionMessage());

				}
				
				} catch (Exception e) {
					
					e.printStackTrace();
				}
			}
		}
		
	}
}