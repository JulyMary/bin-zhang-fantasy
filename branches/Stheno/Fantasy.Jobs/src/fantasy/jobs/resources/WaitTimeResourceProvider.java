package fantasy.jobs.resources;

import Fantasy.Jobs.Scheduling.*;
import fantasy.servicemodel.*;

public class WaitTimeResourceProvider extends ObjectWithSite implements IResourceProvider
{

	private java.util.ArrayList<java.util.Date> _queue = new java.util.ArrayList<java.util.Date>();

	private Object _syncRoot = new Object();

	private IScheduleService _scheduleService;

	private long _scheduleCookie = -1;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IResourceProvider Members

	public final boolean CanHandle(String name)
	{
		return String.equals("WaitTime", name, StringComparison.OrdinalIgnoreCase);
	}

	public final void Initialize()
	{
		_scheduleService = this.Site.<IScheduleService>GetRequiredService();
	}

	public final boolean IsAvailable(ResourceParameter parameter)
	{
		java.util.Date time = new java.util.Date(java.util.Date.parse(parameter.getValues().get("time")));
		if (time.compareTo(new java.util.Date()) <= 0)
		{
			return true;
		}
		else
		{
			this.RegisterTime(time);
			return false;
		}
	}


	private void OnTime()
	{
		ILogger logger = this.Site.<ILogger>GetService();
		java.util.Date fireTime = java.util.Date.getMinValue();
		synchronized (_syncRoot)
		{
			while (this._queue.size() > 0 && this._queue.get(0).compareTo(new java.util.Date()) < 0)
			{
				fireTime = this._queue.get(0);
				this._queue.remove(0);
			}

			if (this._queue.size() > 0)
			{
				this._scheduleCookie = _scheduleService.Register(_queue.get(0), OnTime);
			}
			else
			{
				this._scheduleCookie = -1;
			}

		}
		logger.SafeLogMessage("WaitTime", "Requested WaitTime before {0} are available.", fireTime);
		this.OnAvailable();
	}

	public final boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{

		boolean rs;
		resource.argvalue = null;
		java.util.Date time = new java.util.Date(java.util.Date.parse(parameter.getValues().get("time")));
		if (time.compareTo(new java.util.Date()) > 0)
		{

			rs = false;
			RegisterTime(time);
		}
		else
		{
			rs = true;
		}
		return rs;
	}

	private void RegisterTime(java.util.Date time)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		synchronized (this._syncRoot)
		{
			int pos = this._queue.BinarySearch(time);
			if (pos < 0)
			{
				pos = ~pos;
				this._queue.add(pos, time);
				if (pos == 0)
				{
					if (this._scheduleCookie != -1)
					{
						_scheduleService.Unregister(this._scheduleCookie);
					}

					this._scheduleCookie = _scheduleService.Register(time, OnTime);

				}
			}

			logger.SafeLogMessage("WaitTime", "Register wait time {0}", time);
		}
	}

	public final void Release(Object resource)
	{

	}

	private void OnAvailable()
	{
		if (this.Available != null)
		{
			ILogger logger = this.Site.<ILogger>GetService();
			//logger.SafeLogMessage("WaitTime", "OnAvailable called.");
			this.Available(this, EventArgs.Empty);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler Available;

//C# TO JAVA CONVERTER TODO TASK: Events are not available in Java:
//	public event EventHandler<ProviderRevokeArgs> Revoke
//		{
//			add
//			{
//			}
//			remove
//			{
//			}
//		}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}