package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.jobs.management.*;
import fantasy.*;

public class JobHostCapacityResourceProvider extends CapacityResourceProviderBase implements IPropertyChangedListener
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
	protected int getMaxCount(String key) throws Exception
	{
		 return JobManagerSettings.getDefault().getMaxDegreeOfParallelism();
	}

	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if (StringUtils2.isNullOrEmpty(e.getPropertyName()) || e.getPropertyName().equals("MaxDegreeOfParallelism") )
		{
			this.tryRevoke();
			this.onAvailable();
		}
		
		
	}
}