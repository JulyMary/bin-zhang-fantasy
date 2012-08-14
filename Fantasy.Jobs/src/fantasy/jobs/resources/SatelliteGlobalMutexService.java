package fantasy.jobs.resources;

import java.rmi.*;

import fantasy.ThreadFactory;
import fantasy.TimeSpan;
import fantasy.servicemodel.*;

public class SatelliteGlobalMutexService extends AbstractService implements IGlobalMutexService
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -8898188263129690878L;

	public SatelliteGlobalMutexService() throws RemoteException {
		super();
		
	}

	public final boolean isAvaiable(String key) throws Exception
	{
		boolean rs = false;

		IGlobalMutexService svc = ClientFactory.create(IGlobalMutexService.class);
		try
		{
			try
			{
				rs = svc.isAvaiable(key);
			}
			catch (Exception error)
			{
				if (!WCFExceptionHandler.canCatch(error))
				{
					throw error;
				}
			}
		}
		finally
		{
		}
		return rs;
	}

	public final boolean request(String key, TimeSpan timeout) throws Exception
	{
		boolean rs = false;

		IGlobalMutexService svc = ClientFactory.create(IGlobalMutexService.class);
		try
		{
			try
			{
				rs = svc.request(key, timeout);
			}
			catch (RuntimeException error)
			{
				if (!WCFExceptionHandler.canCatch(error))
				{
					throw error;
				}
			}
		}
		finally
		{
		}
		return rs;
	}

	public final void release(final String key)
	{

		ThreadFactory.queueUserWorkItem(new Runnable(){

			@Override
			public void run() {
				for (int i = 0; i < 10; i++)
				{
					try
					{
						IGlobalMutexService svc = ClientFactory.create(IGlobalMutexService.class);
						svc.release(key);
						return;
					}
					catch (Exception error)
					{
						if (!WCFExceptionHandler.canCatch(error))
						{
							return;
						}
						else
						{
							try {
								Thread.sleep(10 * 1000);
							} catch (InterruptedException e) {
								return;
							}
						}
					}

				}
			}});

	}

}