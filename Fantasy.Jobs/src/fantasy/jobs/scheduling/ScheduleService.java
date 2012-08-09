package fantasy.jobs.scheduling;

import Fantasy.ServiceModel.*;

public class ScheduleService extends AbstractService implements IScheduleService
{

	@Override
	public void InitializeService()
	{

		this._logger = this.Site.<ILogger>GetService();
		_workTread = ThreadFactory.CreateThread(this.Scheduling).WithStart();



		super.InitializeService();
	}

	private ILogger _logger;

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
	}



	private void Scheduling()
	{

		ThreadTaskScheduler taskScheduler = new ThreadTaskScheduler(20);
		TaskFactory taskfactory = new TaskFactory(taskScheduler);
		try
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
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
						taskfactory.StartNew((a) =>
						{
							try
							{
								((ScheduledAction)a).Action();
							}
							catch (ThreadAbortException e)
							{
							}
							catch (RuntimeException err)
							{
								if (_logger != null)
								{
									_logger.LogError("Schedule", err, "An exception was thrown when executed a scheduled action.");
								}
							}
						}
					   , action);
					}

				} while (action != null);


				Thread.sleep(1000);

			} while (true);
		}
		finally
		{
			taskScheduler.AbortAll(false, TimeSpan.Zero);
		}
	}

	public final long Register(java.util.Date timeToExecute, System.Action action)
	{

		synchronized (_syncRoot)
		{
			ScheduledAction tempVar = new ScheduledAction();
			tempVar.Cookie = _seed ++;
			tempVar.TimeToExecute = timeToExecute;
			tempVar.Action = action;
			ScheduledAction item = tempVar;

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			int pos = _queue.BinarySearchBy(timeToExecute, i => i.TimeToExecute);
			if (pos < 0)
			{
				pos = ~pos;
			}
			_queue.add(pos, item);

			return item.Cookie;

		}
	}

	public final void Unregister(long cookie)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			ScheduledAction item = _queue.Find(i => i.Cookie == cookie);
			if (item != null)
			{
				_queue.remove(item);

			}
		}
	}

	private Thread _workTread;
   // private AutoResetEvent _waitHandle = new AutoResetEvent(false);
	private long _seed = 0;

	private java.util.ArrayList<ScheduledAction> _queue = new java.util.ArrayList<ScheduledAction>();

	private Object _syncRoot = new Object();


	private static class ScheduledAction
	{
		public long Cookie;
		public java.util.Date TimeToExecute = new java.util.Date(0);
		public System.Action Action;
	}


}