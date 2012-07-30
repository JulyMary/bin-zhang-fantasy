package fantasy.jobs.management;

import java.util.*;
import fantasy.*;

public class JobStartupProcessCountFilter extends ObjectWithSite implements IJobStartupFilter
{

	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source) throws Exception
	{
		IJobController ctrl = this.getSite().getRequiredService(IJobController.class);
		if (ctrl.GetRunningJobs().length < JobManagerSettings.getDefault().getJobProcessCount())
		{
			return source;
		}
		else
		{
			return new ArrayList<JobMetaData>();
		}
	}

}