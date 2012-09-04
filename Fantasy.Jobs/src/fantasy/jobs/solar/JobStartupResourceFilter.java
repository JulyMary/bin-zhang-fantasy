package fantasy.jobs.solar;

import fantasy.jobs.resources.*;
import fantasy.*;
import fantasy.collections.*;

public class JobStartupResourceFilter extends ObjectWithSite implements IJobStartupFilter
{

	public final Iterable<JobStartupData> filter(Iterable<JobStartupData> source) throws Exception
	{
		final SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		final IResourceRequestQueue resQueue = this.getSite().getRequiredService(IResourceRequestQueue.class);
		
		
		return new Enumerable<JobStartupData>(source).where(new Predicate<JobStartupData>(){

			@Override
			public boolean evaluate(JobStartupData data) throws Exception {
				final ResourceParameter[] res = resQueue.getRequiredResources(data.getJobMetaData().getId());
				if(res.length > 0)
				{
					
					boolean hasRes = false;
					RefObject<Boolean> tempRef_hasRes = new RefObject<Boolean>(hasRes);
					boolean tempVar = manager.SafeCallSatellite(data.getSatellite(), new Func1<ISatellite, Boolean>(){

						@Override
						public Boolean call(ISatellite satellite) throws Exception {
							 return satellite.isResourceAvailable(res);
							
						}}, tempRef_hasRes);
						hasRes = tempRef_hasRes.argvalue;
					if (tempVar)
					{
						if (hasRes)
						{
							return true;
						}
					}
					
					return false;
				}
				else
				{
					return true;
				}
				
				
			}});
		
		
	}
}