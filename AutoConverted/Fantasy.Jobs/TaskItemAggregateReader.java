package Fantasy.Jobs;

public class TaskItemAggregateReader extends ObjectWithSite implements ITagValueProvider
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final char getPrefix()
	{
		return '@';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		String[] expr = tag.split(new char[] {'.'}, 2, StringSplitOptions.RemoveEmptyEntries);

		IJob job = (IJob)this.Site.GetService(IJob.class);

		TaskItem[] items = job.GetEvaluatedItemsByCatetory(expr[0]);

		Iterable<String> values;
		if(expr.length > 1)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			values = items.Select(x => x[expr[1]]);
		}
		else
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			values = items.Select(x=>x.getName());
		}

		return DotNetToJavaStringHelper.join(";", values.toArray());
	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		return true;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return (boolean)context.GetValueOrDefault("EnableTaskItemReader", true) && this.Site != null && this.Site.GetService(IJob.class) != null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}