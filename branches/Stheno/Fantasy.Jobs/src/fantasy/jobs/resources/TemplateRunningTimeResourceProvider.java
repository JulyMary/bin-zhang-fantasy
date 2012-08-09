package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;

public class TemplateRunningTimeResourceProvider extends RunningTimeResourceProviderBase implements IPropertyChangedListener
{
	public TemplateRunningTimeResourceProvider()
	{

	}
	@Override
	public boolean canHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("RunJob", name);
	}

	@Override
	public void initialize() throws Exception
	{
		super.initialize();
		JobManagerSettings.getDefault().AddPropertyChangedListener(this);
	}


	
	
	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if(StringUtils2.isNullOrEmpty(e.getPropertyName())|| e.getPropertyName().equals("TemplateRunningTime"))
		{
			this.reschedule();
		}
		
	}

	@Override
	protected RunningTimeSetting getSetting(final String id) throws Exception
	{
		RunningTimeSetting rs = new Enumerable<TemplateRunningTimeSetting>(JobManagerSettings.getDefault().getTemplateRunningTime().getTemplates()).firstOrDefault(new Predicate<TemplateRunningTimeSetting>(){

			@Override
			public boolean evaluate(TemplateRunningTimeSetting s)
					throws Exception {
				
				return StringUtils.equalsIgnoreCase(s.getName(), id);
			}});
		if (rs == null)
		{
			rs = JobManagerSettings.getDefault().getTemplateRunningTime();
		}

		return rs;
	}

	@Override
	protected String getResourceId(ResourceParameter parameter)
	{
		return StringUtils.lowerCase(parameter.getValues().get("template"));
	}
	
}