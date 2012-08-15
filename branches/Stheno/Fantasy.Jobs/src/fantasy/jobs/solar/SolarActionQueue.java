package fantasy.jobs.solar;

import Fantasy.ServiceModel.*;

public class SolarActionQueue extends AbstractService implements ISolarActionQueue
{

	private java.util.ArrayList<Action<ISolar>> _queue = new java.util.ArrayList<Action<ISolar>>();

	private Thread _retryThread;

	@Override
	public void InitializeService()
	{
		_retryThread = ThreadFactory.CreateThread(Refresh).WithStart();

		super.InitializeService();
	}


	private void Refresh()
	{
		while (true)
		{
			Thread.sleep(15 * 1000);
			java.util.ArrayList<Action<ISolar>> list;
			synchronized (_queue)
			{
				list = new java.util.ArrayList<Action<ISolar>>(_queue);
			}
			try
			{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//				using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
				ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
				try
				{
					for (Action<ISolar> action : list)
					{
						action(client.Client);
						synchronized (_queue)
						{
							_queue.remove(action);
						}
					}
				}
				finally
				{
				}
			}
			catch (java.lang.Exception e)
			{
			}


		}
	}

	@Override
	public void UninitializeService()
	{
		_retryThread.stop();
		_retryThread.join();
		super.UninitializeService();
	}


	public final void Enqueue(Action<ISolar> action)
	{
		try
		{
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
			ClientRef<ISolar> client = ClientFactory.<ISolar>Create();
			try
			{
				action(client.Client);
			}
			finally
			{
			}
		}
		catch(RuntimeException error)
		{
			if (WCFExceptionHandler.CanCatch(error))
			{
				this.Site.<ILogger>GetService().SafeLogError("Solar", error, "WCF error");
				synchronized (_queue)
				{
					_queue.add(action);
				}
			}
			else
			{
				throw error;
			}
		}
	}
}