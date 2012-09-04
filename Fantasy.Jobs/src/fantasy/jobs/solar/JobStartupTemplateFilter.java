package fantasy.jobs.solar;

import java.util.*;

import fantasy.*;
import fantasy.collections.*;
import fantasy.jobs.resources.*;

public class JobStartupTemplateFilter extends ObjectWithSite implements IJobStartupFilter
{


	public final Iterable<JobStartupData> filter(Iterable<JobStartupData> source) throws Exception
	{
		final SatelliteManager manager = this.getSite().getRequiredService(SatelliteManager.class);
		
		return new Enumerable<JobStartupData>(source).where(new Predicate<JobStartupData>(){

			@Override
			public boolean evaluate(JobStartupData data) throws Exception {
				
				TreeMap<String, String> params = new TreeMap<String, String>(); 
				params.put("template", data.getJobMetaData().getTemplate());
				final ResourceParameter res = new ResourceParameter("RunJob", params);
				
				
				boolean canRun = false;
				RefObject<Boolean> tempRef_canRun = new RefObject<Boolean>(canRun);
	
				boolean tempVar = manager.SafeCallSatellite(data.getSatellite(), new Func1<ISatellite, Boolean>(){

					@Override
					public Boolean call(ISatellite satellite) throws Exception {
						return satellite.isResourceAvailable(new ResourceParameter[]{ res});
					}}, tempRef_canRun);
					canRun = tempRef_canRun.argvalue;
				if (tempVar)
				{
					if (canRun)
					{
	
						return true;
					}
				}
				return false;
			}});
		
	
	}

}