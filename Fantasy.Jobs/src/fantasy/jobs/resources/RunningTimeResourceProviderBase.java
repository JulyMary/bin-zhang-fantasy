package fantasy.jobs.resources;

import org.apache.commons.lang3.time.DateUtils;

import fantasy.jobs.scheduling.*;
import fantasy.*;
import fantasy.Action;
import fantasy.collections.*;

public abstract class RunningTimeResourceProviderBase extends ResourceProvider
{

	private Object _syncRoot = new Object();



	


	protected IScheduleService _scheduleService;

	public void initialize() throws Exception
	{
		_scheduleService = this.getSite().getRequiredService(IScheduleService.class);

	}

//	private void SystemEvents_TimeChanged(Object sender, EventArgs e)
//	{
//		ILogger logger = this.Site.<ILogger>GetService();
//		if (logger != null)
//		{
//			logger.LogMessage("ScheduleResourceProvider", "System time changed. Rescheduling.");
//		}
//
//		this.Reschedule();
//	}


	protected abstract RunningTimeSetting getSetting(String id) throws Exception;
	protected abstract String getResourceId(ResourceParameter parameter);

	public final boolean isAvailable(ResourceParameter parameter) throws Exception
	{
		String id = this.getResourceId(parameter);
		RunningTimeSetting setting = this.getSetting(id);
		if (setting.IsDisabledOrInPeriod(new java.util.Date()))
		{
			return true;
		}
		else
		{
			return false;
		}

	}



	public final boolean request(ResourceParameter parameter, RefObject<Object> resource) throws Exception
	{

		boolean rs = false;
		synchronized (_syncRoot)
		{
			resource.argvalue = null;
			String id = this.getResourceId(parameter);
			RunningTimeSetting setting = this.getSetting(id);
			Resource res = this.findOrCreateResource(id);
			if (setting.IsDisabledOrInPeriod(new java.util.Date()))
			{
				rs = true;
				resource.argvalue = res;
			}
		}
		return rs;
	}

	private Resource findOrCreateResource(final String id) throws Exception
	{
		synchronized (_syncRoot)
		{
			Resource rs = new Enumerable<Resource>(this._resources).firstOrDefault(new Predicate<Resource>(){

				@Override
				public boolean evaluate(Resource obj) throws Exception {
					return obj.getId().equals(id);
				}});
			if (rs == null)
			{
				Resource tempVar = new Resource();
				tempVar.setId(id);
				rs = tempVar;

				this._resources.add(rs);

				createSchedule(id);
			}


			return rs;
		}
	}

	private ScheduledResource createSchedule(String id) throws Exception
	{
		RunningTimeSetting setting = this.getSetting(id);
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
					this.registerPeriodStart(rs, period);
				}
				else
				{
					this.registerPeriodEnd(rs, period.getEnd());
				}
			}
		}
		return rs;
	}

	private void registerPeriodStart(final ScheduledResource schedule, final Period period)
	{

		long cookie = this._scheduleService.register(period.getStart(), new Action(){

			@Override
			public void call() throws Exception {
				RunningTimeResourceProviderBase.this.registerPeriodEnd(schedule, period.getEnd());
				Thread.sleep(1);
				RunningTimeResourceProviderBase.this.onAvailable();
				
			}}); 
		
		schedule.setCookie(cookie);

	}
	
	

	private void registerPeriodEnd(final ScheduledResource schedule, final java.util.Date endTime)
	{


		final RunningTimeSetting setting = schedule.getScheduleSetting();
		long cookie = this._scheduleService.register(endTime, new Action(){

			@Override
			public void call() throws Exception {
				Period period = setting.GetPeriod(DateUtils.addMilliseconds(endTime,1));
				RunningTimeResourceProviderBase.this.registerPeriodStart(schedule, period);
				Resource res = RunningTimeResourceProviderBase.this.findOrCreateResource(schedule.getId());
				ProviderRevokeArgs e = new ProviderRevokeArgs(RunningTimeResourceProviderBase.this);
				e.setResource(res);
				RunningTimeResourceProviderBase.this.onRevoke(e);
				
			}}); 
		
		
	
		schedule.setCookie(cookie);
	}


	protected final void reschedule() throws Exception
	{

		boolean available = false;
		java.util.ArrayList<Resource> expiredResources = new java.util.ArrayList<Resource>();
		synchronized (_syncRoot)
		{
			for (ScheduledResource schedule : this._scheduledResources)
			{
				if (schedule.getCookie() > 0)
				{
					_scheduleService.unregister(schedule.getCookie());
					schedule.setCookie(0);
				}
			}

			this._scheduledResources.clear();


			java.util.Date now = new java.util.Date();

			for (Resource resource : this._resources)
			{
				ScheduledResource schedule = this.createSchedule(resource.getId());
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


	public void release(Object resource)
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

		private RunningTimeSetting privateScheduleSetting;
		public final RunningTimeSetting getScheduleSetting()
		{
			return privateScheduleSetting;
		}
		public final void setScheduleSetting(RunningTimeSetting value)
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