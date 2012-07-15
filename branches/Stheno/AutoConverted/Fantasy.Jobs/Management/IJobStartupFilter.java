package Fantasy.Jobs.Management;

public interface IJobStartupFilter
{
	Iterable<JobMetaData> Filter(Iterable<JobMetaData> source);
}