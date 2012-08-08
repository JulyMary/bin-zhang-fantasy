package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;

import fantasy.jobs.management.*;


public class TemplateCapacityResourceProvider extends CapacityResourceProviderBase implements IPropertyChangedListener
{

	@Override
	public boolean canHandle(String name)
	{
	
		return StringUtils.equalsIgnoreCase("RunJob", name);
	}

	@Override
	public void initialize() throws Exception
	{
		JobManagerSettings.getDefault().AddPropertyChangedListener(this);
	}

	@Override
	protected int getMaxCount(final String key) throws Exception
	{
		TemplateCapacitySetting setting = new Enumerable<TemplateCapacitySetting>(JobManagerSettings.getDefault().getTemplateCapacity().getTemplates()).firstOrDefault(new Predicate<TemplateCapacitySetting>(){

			@Override
			public boolean evaluate(TemplateCapacitySetting s) throws Exception {
				
				return StringUtils.equals(s.getName() ,key);
			}});

		return setting != null ? setting.getCapacity() : JobManagerSettings.getDefault().getTemplateCapacity().getCapacity();

	}



	@Override
	protected String getKey(ResourceParameter parameter)
	{
		return StringUtils.lowerCase(parameter.getValues().get("template"));
	}

	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if (StringUtils2.isNullOrEmpty(e.getPropertyName()) || e.getPropertyName().equals("TemplateCapacity"))
		{
			this.tryRevoke();
			
			this.onAvailable();
		}
		
	}



}