package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Resources.*;

public class JobStartupTemplateFilter extends ObjectWithSite implements IJobStartupFilter
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
		SatelliteManager manager = this.Site.<SatelliteManager>GetRequiredService();
		for (JobStartupData data : source)
		{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			ResourceParameter[] res = new ResourceParameter[] {new ResourceParameter("RunJob", new {template=data.getJobMetaData().getTemplate() })};
			boolean canRun = false;
			RefObject<T> tempRef_canRun = new RefObject<T>(canRun);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			boolean tempVar = manager.SafeCallSatellite(data.getSatellite(), satellite => satellite.IsResourceAvailable(res), tempRef_canRun);
				canRun = tempRef_canRun.argvalue;
			if (tempVar)
			{
				if (canRun)
				{
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
					yield return data;
				}
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}