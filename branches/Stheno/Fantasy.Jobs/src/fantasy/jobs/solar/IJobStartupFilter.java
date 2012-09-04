package fantasy.jobs.solar;

public interface IJobStartupFilter
{
	Iterable<JobStartupData> filter(Iterable<JobStartupData> source) throws Exception;
}