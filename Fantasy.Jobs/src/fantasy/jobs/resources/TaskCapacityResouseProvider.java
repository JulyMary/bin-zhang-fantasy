package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.IPropertyChangedListener;
import fantasy.PropertyChangedEvent;
import fantasy.StringUtils2;
import fantasy.collections.*;
import fantasy.jobs.management.*;

public class TaskCapacityResouseProvider extends CapacityResourceProviderBase implements IPropertyChangedListener
{
	@Override
	public boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("RunTask", name);
	}

	@Override
	protected String getKey(ResourceParameter parameter)
	{
		return parameter.getValues().get("namespace") + "|" + parameter.getValues().get("taskname");
	}

	@Override
	public void initialize() throws Exception
	{
		JobManagerSettings.getDefault().AddPropertyChangedListener(this);
	}
	
	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if (StringUtils2.isNullOrEmpty(e.getPropertyName()) || e.getPropertyName().equals("TaskCapacity"))
		{
			this.tryRevoke();
			
			this.onAvailable();
		}

		
		
	}

	


	@Override
	protected int getMaxCount(String key) throws Exception
	{
		String[] strs = StringUtils2.split(key, "|", 2, false);

		final String ns = strs[0];
		final String name = strs[1];
       
		TaskCapacitySetting setting = new Enumerable<TaskCapacitySetting>(JobManagerSettings.getDefault().getTaskCapacity().getTasks()).firstOrDefault(new Predicate<TaskCapacitySetting>(){

			@Override
			public boolean evaluate(TaskCapacitySetting s) throws Exception {
				
				return s.getName().equals(name) && s.getNamespace().equals(ns);
			}});
		
		

		return setting != null ? setting.getCapacity() : JobManagerSettings.getDefault().getTaskCapacity().getCapacity();
	}

	

}