package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.*;
import fantasy.jobs.management.*;

public class JobRunningTimeResourceProvider extends RunningTimeResourceProviderBase implements IPropertyChangedListener
{


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
		if(StringUtils2.isNullOrEmpty(e.getPropertyName()) || e.getPropertyName().equals("JobRunningTime"))
		{
			this.reschedule();
		}
		
	}

	

	@Override
	protected RunningTimeSetting getSetting(String id) throws Exception
	{
		return JobManagerSettings.getDefault().getJobRunningTime();
	}

	@Override
	protected String getResourceId(ResourceParameter parameter)
	{
		return "";
	}

	
	

	
}