package fantasy.jobs.solar;

public class JobStartupSatelliteFilter extends ObjectWithSite implements IJobStartupFilter
{


	private double GetLoadFactor(SatelliteManager manager, String name)
	{
		double rs = 0;
		RefObject<T> tempRef_rs = new RefObject<T>(rs);
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		boolean tempVar = manager.SafeCallSatellite(name, satellite => satellite.GetLoadFactor(), tempRef_rs);
			rs = tempRef_rs.argvalue;
		if (tempVar)
		{
			return rs;
		}
		else
		{
			return 0;
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobStartupData> Filter(Iterable<JobStartupData> source)
	{
		SatelliteManager manager = this.Site.<SatelliteManager>GetRequiredService();


//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from site in manager.getSatellites() select new { site = site, load = this.GetLoadFactor(manager, site.getName()) };

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		String[] names = (from o in query where o.load > 0 orderby o.load descending select o.site.getName()).toArray();


		for (JobStartupData data : source)
		{
			for (String name : names)
			{
				JobStartupData tempVar = new JobStartupData();
				tempVar.setJobMetaData(data.getJobMetaData());
				tempVar.setSatellite(name);
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
				yield return tempVar;
			}
		}

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}