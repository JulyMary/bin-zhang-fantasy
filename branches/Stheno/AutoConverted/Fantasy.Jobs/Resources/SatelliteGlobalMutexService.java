package Fantasy.Jobs.Resources;

import Fantasy.ServiceModel.*;

public class SatelliteGlobalMutexService extends AbstractService implements IGlobalMutexService
{
	public final boolean IsAvaiable(String key)
	{
		boolean rs = false;
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
		ClientRef<IGlobalMutexService> svc = ClientFactory.<IGlobalMutexService>Create();
		try
		{
			try
			{
				rs = svc.Client.IsAvaiable(key);
			}
			catch (RuntimeException error)
			{
				if (!WCFExceptionHandler.CanCatch(error))
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

	public final boolean Request(String key, TimeSpan timeout)
	{
		boolean rs = false;
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//		using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
		ClientRef<IGlobalMutexService> svc = ClientFactory.<IGlobalMutexService>Create();
		try
		{
			try
			{
				rs = svc.Client.Request(key, timeout);
			}
			catch (RuntimeException error)
			{
				if (!WCFExceptionHandler.CanCatch(error))
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

	public final void Release(String key)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			for (int i = 0; i < 10; i++)
			{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//				using (ClientRef<IGlobalMutexService> svc = ClientFactory.Create<IGlobalMutexService>())
				ClientRef<IGlobalMutexService> svc = ClientFactory.<IGlobalMutexService>Create();
				try
				{
					try
					{
						svc.Client.Release(key);
						break;
					}
					catch (RuntimeException error)
					{
						if (!WCFExceptionHandler.CanCatch(error))
						{
							throw error;
						}
					}
				}
				finally
				{
				}
				Thread.sleep(60 * 1000);
			}
		}
	   );
	}

}