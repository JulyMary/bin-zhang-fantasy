package fantasy.jobs.management;
import fantasy.collections.*;
import fantasy.jobs.*;
public class JobStartupStateFilter implements IJobStartupFilter
{


	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source) throws Exception
	{
        return new Enumerable<JobMetaData>(source).where(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				return (job.getState() & (JobState.Unstarted | JobState.Suspended)) > 0;
			}})
			.orderBy(new Selector<JobMetaData, Integer>(){

				@Override
				public Integer select(JobMetaData item) {
					return item.getState();
				}
				
			});
		
	}

}