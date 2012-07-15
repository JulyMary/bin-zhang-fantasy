package System.Threading;

public class ThreadTaskScheduler extends TaskScheduler
{
	public ThreadTaskScheduler(int maximumConcurrencyLevel)
	{
		if (maximumConcurrencyLevel <= 0)
		{
			throw new ArgumentOutOfRangeException("maximumConcurrencyLevel");
		}
		this._maximumConcurrencyLevel = maximumConcurrencyLevel;
	}

	private int _maximumConcurrencyLevel = -1;
	@Override
	public int getMaximumConcurrencyLevel()
	{
		return _maximumConcurrencyLevel;
	}



	private Object _syncRoot = new Object();
	private java.util.ArrayList<Task> _scheduledTasks = new java.util.ArrayList<Task>();
	private java.util.ArrayList<Thread> _threads = new java.util.ArrayList<Thread>();

	private boolean _aborting = false;

	public final boolean AbortAll(boolean waitForExit, TimeSpan timeout)
	{
		synchronized (_syncRoot)
		{
			_aborting = true;
			_scheduledTasks.clear();
		}

		for (Thread thread : this._threads)
		{
			thread.stop();
		}

		if (waitForExit)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Thread waitThread = ThreadFactory.CreateThread(() =>
			{
				Thread[] threads;
				synchronized (_syncRoot)
				{
					threads = this._threads.toArray(new Thread[]{});
				}
				for (Thread thread : threads)
				{
					thread.join(timeout);
				}
			}
		   ).WithStart();

			return waitThread.join(timeout);

		}
		else
		{
			return true;
		}
	}

	@Override
	protected Iterable<Task> GetScheduledTasks()
	{
		synchronized (_syncRoot)
		{
			return this._scheduledTasks.toArray(new Task[]{});
		}
	}

	@Override
	protected boolean TryDequeue(Task task)
	{
		synchronized (_syncRoot)
		{
			boolean rs = super.TryDequeue(task);
			if (rs)
			{
				this._scheduledTasks.remove(task);
			}
			return rs;
		}
	}

	@Override
	protected void QueueTask(Task task)
	{
		synchronized (_syncRoot)
		{
			if (!_aborting)
			{
				if (this._threads.size() < this.getMaximumConcurrencyLevel())
				{
					this.RunTask(task);
				}
				else
				{
					this._scheduledTasks.add(task);
				}
			}

		}
	}

	private void RunTask(Task task)
	{
		synchronized (_syncRoot)
		{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Thread thread = ThreadFactory.CreateThread((currentTask) =>
			{
				try
				{
					super.TryExecuteTask((Task)currentTask);
				}
				finally
				{
					synchronized (_syncRoot)
					{
						this._threads.remove(Thread.currentThread());
						if (! _aborting && this._scheduledTasks.size() > 0 && this._threads.size() < this.getMaximumConcurrencyLevel())
						{
							Task next = this._scheduledTasks.get(0);
							this._scheduledTasks.remove(0);
							this.RunTask(next);
						}
					}
				}
			}
		   );
			this._threads.add(thread);

			thread.Start(task);
		}
	}

	@Override
	protected boolean TryExecuteTaskInline(Task task, boolean taskWasPreviouslyQueued)
	{
		return false;
	}
}