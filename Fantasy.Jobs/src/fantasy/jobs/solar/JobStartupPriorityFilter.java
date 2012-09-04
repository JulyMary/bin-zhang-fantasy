package fantasy.jobs.solar;


import fantasy.*;
import fantasy.collections.*;

public class JobStartupPriorityFilter extends ObjectWithSite implements IJobStartupFilter
{


	public final Iterable<JobStartupData> filter(Iterable<JobStartupData> source)
	{
		
		return new Enumerable<JobStartupData>(source).orderByDescending(new Selector<JobStartupData, Integer>(){

			@Override
			public Integer select(JobStartupData item) {
				return item.getJobMetaData().getPriority();
			}});

	}


}