package Fantasy.Jobs.Solar;

public class JobStartupPriorityFilter extends ObjectWithSite implements IJobStartupFilter
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return source.OrderByDescending(j => j.JobMetaData.Priority);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}