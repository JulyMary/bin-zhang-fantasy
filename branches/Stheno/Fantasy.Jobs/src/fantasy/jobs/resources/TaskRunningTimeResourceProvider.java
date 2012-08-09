package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;


import fantasy.*;
import fantasy.collections.*;

import fantasy.jobs.management.*;

public class TaskRunningTimeResourceProvider extends RunningTimeResourceProviderBase implements IPropertyChangedListener
{
	@Override
	public boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("RunTask", name);
		
	}

	@Override
	public void initialize() throws Exception
	{
		super.initialize();
		JobManagerSettings.getDefault().AddPropertyChangedListener(this);
	}

	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if(StringUtils2.isNullOrEmpty(e.getPropertyName())|| e.getPropertyName().equals("TaskRunningTime"))
		{
			this.reschedule();
		}
		
	}

	

	@Override
	protected RunningTimeSetting getSetting(final String id) throws Exception
	{
		RunningTimeSetting rs = new Enumerable<TaskRunningTimeSetting>(JobManagerSettings.getDefault().getTaskRunningTime().getTasks()).firstOrDefault(new Predicate<TaskRunningTimeSetting>(){

			@Override
			public boolean evaluate(TaskRunningTimeSetting setting)
					throws Exception {
				return StringUtils.equals(setting.getNamespace() + "|" + setting.getName(),id);
				
			}});
		if (rs == null)
		{
			rs = JobManagerSettings.getDefault().getTaskRunningTime();
		}
		return rs;
	}

	@Override
	protected String getResourceId(ResourceParameter parameter)
	{
		return String.format("%1$s|%2$s", parameter.getValues().get("namespace"), parameter.getValues().get("taskname"));
	}
}