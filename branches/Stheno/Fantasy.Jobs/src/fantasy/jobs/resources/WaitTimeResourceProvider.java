package fantasy.jobs.resources;

import java.text.SimpleDateFormat;
import java.util.Collections;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.Action;
import fantasy.jobs.scheduling.*;
import fantasy.servicemodel.*;

public class WaitTimeResourceProvider extends ResourceProvider implements IResourceProvider
{

	private java.util.ArrayList<java.util.Date> _queue = new java.util.ArrayList<java.util.Date>();

	private Object _syncRoot = new Object();

	private IScheduleService _scheduleService;

	private long _scheduleCookie = -1;

	public final boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("WaitTime", name);
		
	}

	public final void initialize() throws Exception
	{
		_scheduleService = this.getSite().getRequiredService(IScheduleService.class);
	}

	private SimpleDateFormat _format = new SimpleDateFormat();
	public final boolean isAvailable(ResourceParameter parameter) throws Exception
	{
		
		java.util.Date time = _format.parse(parameter.getValues().get("time"));
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


	private void onTime() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		java.util.Date fireTime = new java.util.Date(Long.MIN_VALUE);
		synchronized (_syncRoot)
		{
			while (this._queue.size() > 0 && this._queue.get(0).compareTo(new java.util.Date()) < 0)
			{
				fireTime = this._queue.get(0);
				this._queue.remove(0);
			}

			if (this._queue.size() > 0)
			{
				this._scheduleCookie = _scheduleService.register(_queue.get(0), new Action(){

					@Override
					public void act() throws Exception {
						 onTime();
						
					}});
			}
			else
			{
				this._scheduleCookie = -1;
			}

		}
		Log.SafeLogMessage(logger, "WaitTime", "Requested WaitTime before %1$s are available.", _format.format(fireTime));
		this.onAvailable();
	}

	public final boolean request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{

		boolean rs;
		resource.argvalue = null;
		java.util.Date time = _format.parse(parameter.getValues().get("time"));
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

	private void RegisterTime(java.util.Date time) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		synchronized (this._syncRoot)
		{
			int pos = Collections.binarySearch(this._queue,time);
			if (pos < 0)
			{
				pos = ~pos;
				this._queue.add(pos, time);
				if (pos == 0)
				{
					if (this._scheduleCookie != -1)
					{
						_scheduleService.unregister(this._scheduleCookie);
					}

					this._scheduleCookie = _scheduleService.register(time, new Action(){

						@Override
						public void act() throws Exception {
							onTime();
							
						}});

				}
			}

			Log.SafeLogMessage(logger, "WaitTime", "Register wait time %1$s", _format.format(time));
		}
	}

	public final void release(Object resource)
	{

	}
}

	

