package fantasy.jobs.resources;

import fantasy.jobs.scheduling.*;
import fantasy.jobs.management.*;
import fantasy.servicemodel.*;
import fantasy.*;

public abstract class RunningTimeResourceProviderBase extends ResourceProvider
{

	private Object _syncRoot = new Object();



	public final boolean CanHandle(String name)
	{
		return InternalCanHandle(name);
	}


	protected abstract boolean InternalCanHandle(String name);


	protected IScheduleService _scheduleService;

	public void Initialize()
	{
		_scheduleService = this.Site.<IScheduleService>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);
	}

	private void SystemEvents_TimeChanged(Object sender, EventArgs e)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		if (logger != null)
		{
			logger.LogMessage("ScheduleResourceProvider", "System time changed. Rescheduling.");
		}

		this.Reschedule();
	}


	protected abstract RuntimeScheduleSetting GetSetting(String id);
	protected abstract String GetResourceId(ResourceParameter parameter);

	public final boolean IsAvailable(ResourceParameter parameter)
	{
		String id = this.GetResourceId(parameter);
		RuntimeScheduleSetting setting = this.GetSetting(id);
		if (setting.IsDisabledOrInPeriod(new java.util.Date()))
		{
			return true;
		}
		else
		{
			return false;
		}

	}



	public final boolean Request(ResourceParameter parameter, RefObject<Object> resource)
	{

		boolean rs = false;
		synchronized (_syncRoot)
		{
			resource.argvalue = null;
			String id = this.GetResourceId(parameter);
			RuntimeScheduleSetting setting = this.GetSetting(id);
			Resource res = this.FindOrCreateResource(id);
			if (setting.IsDisabledOrInPeriod(new java.util.Date()))
			{
				rs = true;
				resource.argvalue = res;
			}
		}
		return rs;
	}

	private Resource FindOrCreateResource(String id)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			Resource rs = this._resources.Find(r => id.equals(r.Id));
			if (rs == null)
			{
				Resource tempVar = new Resource();
				tempVar.setId(id);
				rs = tempVar;

				this._resources.add(rs);

				CreateSchedule(id);
			}


			return rs;
		}
	}

	private ScheduledResource CreateSchedule(String id)
	{
		RuntimeScheduleSetting setting = this.GetSetting(id);
		ScheduledResource tempVar = new ScheduledResource();
		tempVar.setId(id);
		tempVar.setScheduleSetting(setting);
		ScheduledResource rs = tempVar;
		this._scheduledResources.add(rs);
		if (setting.getEnabled())
		{
			java.util.Date now = new java.util.Date();
			Period period = setting.GetPeriod(new java.util.Date());
			if (period != null)
			{
				if (now.compareTo(period.getStart()) < 0)
				{
					this.RegisterPeriodStart(rs, period);
				}
				else
				{
					this.RegisterPeriodEnd(rs, period.getEnd());
				}
			}
		}
		return rs;
	}

	private void RegisterPeriodStart(ScheduledResource schedule, Period period)
	{

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		long cookie = this._scheduleService.Register(period.getStart(), () =>
		{
			this.RegisterPeriodEnd(schedule, period.getEnd());
			Thread.sleep(1);
			this.OnAvailable();
		}
	   );
		schedule.setCookie(cookie);

	}

	private void RegisterPeriodEnd(ScheduledResource schedule, java.util.Date endTime)
	{


		RuntimeScheduleSetting setting = schedule.getScheduleSetting();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		long cookie = this._scheduleService.Register(endTime, () =>
		{
			Period period = setting.GetPeriod(endTime.AddTicks(1));
			this.RegisterPeriodStart(schedule, period);
			Resource res = this.FindOrCreateResource(schedule.getId());
			this.OnRevoke(res);
		}
	   );
		schedule.setCookie(cookie);
	}


	protected final void Reschedule()
	{

		boolean available = false;
		java.util.ArrayList<Resource> expiredResources = new java.util.ArrayList<Resource>();
		synchronized (_syncRoot)
		{
			for (ScheduledResource schedule : this._scheduledResources)
			{
				if (schedule.getCookie() > 0)
				{
					_scheduleService.Unregister(schedule.getCookie());
					schedule.setCookie(0);
				}
			}

			this._scheduledResources.clear();


			java.util.Date now = new java.util.Date();

			for (Resource resource : this._resources)
			{
				ScheduledResource schedule = this.CreateSchedule(resource.getId());
				if (schedule.getScheduleSetting().IsDisabledOrInPeriod(now))
				{
					available = true;
				}
				else
				{
					expiredResources.add(resource);
				}
			}
		}

		for (Resource res : expiredResources)
		{
			ProviderRevokeArgs e = new ProviderRevokeArgs(this);
			e.setResource(res);
			this.onRevoke(e);
		}

		if (available)
		{
			this.onAvailable();
		}


	}


	public void Release(Object resource)
	{

	}

	



	private java.util.ArrayList<Resource> _resources = new java.util.ArrayList<Resource>();

	private java.util.ArrayList<ScheduledResource> _scheduledResources = new java.util.ArrayList<ScheduledResource>();


	protected static class Resource
	{
		private String privateId;
		public final String getId()
		{
			return privateId;
		}
		public final void setId(String value)
		{
			privateId = value;
		}
	}

	protected static class ScheduledResource
	{
		private long privateCookie;
		public final long getCookie()
		{
			return privateCookie;
		}
		public final void setCookie(long value)
		{
			privateCookie = value;
		}

		private RuntimeScheduleSetting privateScheduleSetting;
		public final RuntimeScheduleSetting getScheduleSetting()
		{
			return privateScheduleSetting;
		}
		public final void setScheduleSetting(RuntimeScheduleSetting value)
		{
			privateScheduleSetting = value;
		}

		private String privateId;
		public final String getId()
		{
			return privateId;
		}
		public final void setId(String value)
		{
			privateId = value;
		}
	}
}