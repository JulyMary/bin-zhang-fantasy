package Fantasy.Jobs.Resources;

import Fantasy.Jobs.Management.*;

public class ResourceStartupFilter extends ObjectWithSite implements IJobStartupFilter
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source)
	{
		IResourceManager mngr = this.Site.<IResourceManager>GetRequiredService();
		IResourceRequestQueue queue = this.Site.<IResourceRequestQueue>GetRequiredService();
		for (JobMetaData job : source)
		{
			ResourceParameter[] res = queue.GetRequiredResources(job.getId());
			if (res.length == 0 || mngr.IsAvailable(res))
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return job;
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}