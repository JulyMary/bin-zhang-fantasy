package fantasy.jobs.resources;

import org.apache.commons.lang3.StringUtils;

import fantasy.jobs.management.*;
import fantasy.*;

public class JobHostCapacityResourceProvider extends CapacityResourceProviderBase implements IPropertyChangedListener
{


	@Override
	public boolean CanHandle(String name)
	{
		return StringUtils.equalsIgnoreCase("RunJob", name);
	}

	@Override
	public void Initialize() throws Exception
	{

		JobManagerSettings.getDefault().AddPropertyChangedListener(this);
	}

	@Override
	protected int GetMaxCount(String key) throws Exception
	{
		 return JobManagerSettings.getDefault().getMaxDegreeOfParallelism();
	}

	@Override
	public void propertyChanged(PropertyChangedEvent e) throws Exception {
		if (e.getPropertyName().equals("MaxDegreeOfParallelism"))
		{
			this.TryRevoke();
		}
		this.onAvailable();
		
	}
}