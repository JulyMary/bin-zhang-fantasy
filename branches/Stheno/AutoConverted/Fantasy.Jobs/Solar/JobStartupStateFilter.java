package Fantasy.Jobs.Solar;

public class JobStartupStateFilter extends ObjectWithSite implements IJobStartupFilter
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return source.Where(j => (j.JobMetaData.State & (JobState.Unstarted | JobState.Suspended)) > 0).OrderBy(j => j.JobMetaData.State);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}