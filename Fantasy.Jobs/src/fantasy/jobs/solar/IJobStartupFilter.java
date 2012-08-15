package fantasy.jobs.solar;

public interface IJobStartupFilter
{
	Iterable<JobStartupData> Filter(Iterable<JobStartupData> source);
}