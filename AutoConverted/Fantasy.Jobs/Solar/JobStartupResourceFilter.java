package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Resources.*;

public class JobStartupResourceFilter extends ObjectWithSite implements IJobStartupFilter
{

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
		SatelliteManager manager = this.Site.<SatelliteManager>GetRequiredService();
		IResourceRequestQueue resQueue = this.Site.<IResourceRequestQueue>GetRequiredService();
		for (JobStartupData data : source)
		{
			ResourceParameter[] res = resQueue.GetRequiredResources(data.getJobMetaData().getId());
			if (res.length > 0)
			{
				boolean hasRes = false;
				RefObject<T> tempRef_hasRes = new RefObject<T>(hasRes);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
				boolean tempVar = manager.SafeCallSatellite(data.getSatellite(), satellite => satellite.IsResourceAvailable(res), tempRef_hasRes);
					hasRes = tempRef_hasRes.argvalue;
				if (tempVar)
				{
					if (hasRes)
					{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
						yield return data;
					}
				}
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