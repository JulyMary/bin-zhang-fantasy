package fantasy.jobs.solar;

import java.rmi.RemoteException;
import java.util.concurrent.*;

import fantasy.*;
import fantasy.servicemodel.*;

public class SolarActionQueue extends AbstractService implements ISolarActionQueue
{

	/**
	 * 
	 */
	private static final long serialVersionUID = 8743714117640391329L;

	public SolarActionQueue() throws RemoteException {
		super();
	
	}


	private java.util.ArrayList<Action1<ISolar>> _queue = new java.util.ArrayList<Action1<ISolar>>();

	ScheduledExecutorService _executor;

	@Override
	public void initializeService() throws Exception
	{
		_executor =  Executors.newScheduledThreadPool(1);
		_executor.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				SolarActionQueue.this.Refresh();
			}}, 15, 15, TimeUnit.SECONDS);

		super.initializeService();
	}


	private void Refresh()
	{
		
			java.util.ArrayList<Action1<ISolar>> list;
			synchronized (_queue)
			{
				list = new java.util.ArrayList<Action1<ISolar>>(_queue);
			}
			
				
				try
				{
					ISolar client = ClientFactory.create(ISolar.class);
					for (Action1<ISolar> action : list)
					{
						action.call(client);
						synchronized (_queue)
						{
							_queue.remove(action);
						}
					}
				
				
			}
			catch (java.lang.Exception e)
			{
			}
	}

	@Override
	public void uninitializeService() throws Exception
	{
		_executor.shutdown();
		_executor.awaitTermination(Long.MAX_VALUE, TimeUnit.MILLISECONDS);
		
		super.uninitializeService();
	}


	public final void enqueue(Action1<ISolar> action) throws Exception
	{
		try
		{			
				ISolar client = ClientFactory.create(ISolar.class);
				
				action.call(client);
			
		}
		catch(Exception error)
		{
			if (WCFExceptionHandler.canCatch(error))
			{
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