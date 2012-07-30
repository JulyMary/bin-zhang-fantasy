package fantasy.jobs.management;

import fantasy.collections.*;

public class JobStartupPriorityFilter implements IJobStartupFilter
{


	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source)
	{
		
		return new Enumerable<JobMetaData>(source).orderByDescending(new Selector<JobMetaData, Integer>(){

			@Override
			public Integer select(JobMetaData item) {
				return item.getPriority();
			}});

	}

}