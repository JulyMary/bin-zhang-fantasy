package fantasy.jobs.management;

public interface IJobStartupFilter
{
	Iterable<JobMetaData> Filter(Iterable<JobMetaData> source) throws Exception;
}