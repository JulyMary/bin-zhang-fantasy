package fantasy.jobs.scheduling;

import java.rmi.RemoteException;
import java.util.*;

import fantasy.Action;
import fantasy.ThreadFactory;
import fantasy.collections.*;

import fantasy.servicemodel.*;

public class ScheduleService extends AbstractService implements IScheduleService
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -4069536653163338614L;

	public ScheduleService() throws RemoteException {
		super();
		
	}



	@Override
	public void initializeService() throws Exception
	{

		this._logger = this.getSite().getService(ILogger.class);
		_workTread = ThreadFactory.createAndStart(new Runnable(){

			@Override
			public void run() {
				try {
					Scheduling();
				} catch (Exception e) {
					
				}
				
			}});
		
		super.initializeService();
	}

	private ILogger _logger;

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		_workTread.interrupt();
	}



	private void Scheduling() throws Exception
	{

	
		
		  
			do
			{

				ScheduledAction action;

				do
				{
					action = null;
					synchronized (_syncRoot)
					{
						if (_queue.size() > 0 && _queue.get(0).TimeToExecute.compareTo(new java.util.Date()) <= 0)
						{
							action = _queue.get(0);
							_queue.remove(0);
						}
					}
					if (action != null)
					{
						final ScheduledAction act2 = action;
						
						ThreadFactory.queueUserWorkItem(new Runnable(){

							@Override
							public void run() {
								try
								{
								act2.Action.act();
								}
								catch(InterruptedException e)
								{
									
								}
								catch(Exception error)
								{
									if (_logger != null)
									{
										_logger.LogError("Schedule", error, "An exception was thrown when executed a scheduled action.");
									}
								}
								
							}});
						
						
					}

				}while (action != null);


				Thread.sleep(1000);

			}while (true);
		
		
	}

	public final long register(java.util.Date timeToExecute, Action action)
	{

		synchronized (_syncRoot)
		{
			ScheduledAction tempVar = new ScheduledAction();
			tempVar.Cookie = _seed ++;
			tempVar.TimeToExecute = timeToExecute;
			tempVar.Action = action;
			ScheduledAction item = tempVar;

			int pos =  CollectionUtils.binarySearchBy(_queue, timeToExecute,  new Selector<ScheduledAction, Date>(){

				@Override
				public Date select(ScheduledAction item) {
					return item.TimeToExecute;
				}});
			if (pos < 0)
			{
				pos = ~pos;
			}
			_queue.add(pos, item);

			return item.Cookie;

		}
	}

	public final void unregister(final long cookie) throws Exception
	{
		synchronized (_syncRoot)
		{
			ScheduledAction item = new Enumerable<ScheduledAction>(_queue).firstOrDefault(new Predicate<ScheduledAction>(){

				@Override
				public boolean evaluate(ScheduledAction obj) throws Exception {
					return obj.Cookie == cookie;
				}});  
			if (item != null)
			{
				_queue.remove(item);

			}
		}
	}

	private Thread _workTread;
	private long _seed = 0;

	private java.util.ArrayList<ScheduledAction> _queue = new java.util.ArrayList<ScheduledAction>();

	private Object _syncRoot = new Object();


	private static class ScheduledAction
	{
		public long Cookie;
		public java.util.Date TimeToExecute = new java.util.Date(0);
		public fantasy.Action Action;
	}


}