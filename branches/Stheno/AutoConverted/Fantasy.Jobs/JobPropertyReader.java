package Fantasy.Jobs;

public class JobPropertyReader extends ObjectWithSite implements ITagValueProvider
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region ITagValueProvider Members

	public final char getPrefix()
	{
		return '$';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context)
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		return job.GetProperty(tag);

	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context)
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		return job != null ? job.HasProperty(tag) : false;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context)
	{
		return this.Site != null && this.Site.GetService(IJob.class) != null;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}