package fantasy.jobs;

import fantasy.*;

public class JobPropertyReader extends ObjectWithSite implements ITagValueProvider
{


	public final char getPrefix()
	{
		return '$';
	}

	public final String GetTagValue(String tag, java.util.Map<String, Object> context) throws Exception
	{
		IJob job = this.getSite().getService(IJob.class);
		return job.GetProperty(tag);

	}

	public final boolean HasTag(String tag, java.util.Map<String, Object> context) throws Exception
	{
		IJob job = this.getSite().getService(IJob.class);
		return job != null ? job.HasProperty(tag) : false;
	}

	public final boolean IsEnabled(java.util.Map<String, Object> context) throws Exception
	{
		return this.getSite() != null && this.getSite().getService(IJob.class) != null;
	}

}