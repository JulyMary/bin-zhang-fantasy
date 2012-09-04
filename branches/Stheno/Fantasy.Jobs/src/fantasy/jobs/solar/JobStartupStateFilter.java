package fantasy.jobs.solar;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.*;

public class JobStartupStateFilter extends ObjectWithSite implements IJobStartupFilter
{
	public final Iterable<JobStartupData> filter(Iterable<JobStartupData> source) throws Exception
	{

		return new Enumerable<JobStartupData>(source).where(new Predicate<JobStartupData>(){

			@Override
			public boolean evaluate(JobStartupData j) throws Exception {
				return (j.getJobMetaData().getState() & (JobState.Unstarted | JobState.Suspended)) > 0;
			}}).orderBy(new Selector<JobStartupData, Integer>(){

				@Override
				public Integer select(JobStartupData j) {
					return j.getJobMetaData().getState();
				}});

	}

}