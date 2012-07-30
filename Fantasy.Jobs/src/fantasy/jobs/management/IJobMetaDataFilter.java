package fantasy.jobs.management;

public interface IJobMetaDataFilter
{
	Iterable<JobMetaData> Filter(Iterable<JobMetaData> source);
}