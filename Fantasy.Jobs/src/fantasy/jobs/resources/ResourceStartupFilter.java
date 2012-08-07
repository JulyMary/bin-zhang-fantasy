package fantasy.jobs.resources;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.management.*;

public class ResourceStartupFilter extends ObjectWithSite implements IJobStartupFilter
{

	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source) throws Exception
	{
		final IResourceManager mngr = this.getSite().getRequiredService(IResourceManager.class);
		final IResourceRequestQueue queue = this.getSite().getRequiredService(IResourceRequestQueue.class);
		
		return new Enumerable<JobMetaData>(source).where(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				ResourceParameter[] res = queue.GetRequiredResources(job.getId());
				return res.length == 0 || mngr.IsAvailable(res);
				
			}});

	}

}