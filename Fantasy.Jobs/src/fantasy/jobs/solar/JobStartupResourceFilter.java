package fantasy.jobs.solar;

import fantasy.jobs.*;
import fantasy.jobs.resources.*;
import fantasy.*;
import fantasy.collections.*;

public class JobStartupResourceFilter extends ObjectWithSite implements IJobStartupFilter
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
		final SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		final IResourceRequestQueue resQueue = this.getSite().getRequiredService(IResourceRequestQueue.class);
		
		
		return new Enumerable<JobStartupData>(source).where(new Predicate<JobStartupData>(){

			@Override
			public boolean evaluate(JobStartupData data) throws Exception {
				ResourceParameter[] res = resQueue.GetRequiredResources(data.getJobMetaData().getId());
				if(res.length > 0)
				{
					
					RefObject<Boolean> hasRes = new RefObject<Boolean>(false);
				}
				else
				{
					return true;
				}
				
				
			}});
		
		for (JobStartupData data : source)
		{
			ResourceParameter[] res = resQueue.GetRequiredResources(data.getJobMetaData().getId());
			if (res.length > 0)
			{
				boolean hasRes = false;
				RefObject<Boolean> tempRef_hasRes = new RefObject<Boolean>(hasRes);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				boolean tempVar = manager.SafeCallSatellite(data.getSatellite(), satellite => satellite.IsResourceAvailable(res), tempRef_hasRes);
					hasRes = tempRef_hasRes.argvalue;
				if (tempVar)
				{
					if (hasRes)
					{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
						return true;
					}
				}
				
				return false;
			}
			else
			{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return data;
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}