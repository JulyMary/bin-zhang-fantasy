package Fantasy.Jobs.Solar;

public interface IJobStartupFilter
{
	Iterable<JobStartupData> Filter(Iterable<JobStartupData> source);
}