package Fantasy.Jobs.Management;

public class JobStartupProcessCountFilter implements IJobStartupFilter, IObjectWithSite
{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobStartupFilter Members

	public final Iterable<JobMetaData> Filter(Iterable<JobMetaData> source)
	{
		IJobController ctrl = this.getSite().<IJobController>GetRequiredService();
		if (ctrl.GetRunningJobs().length < JobManagerSettings.getDefault().getJobProcessCount())
		{
			return source;
		}
		else
		{
			return new JobMetaData[] { };
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IObjectWithSite Members

	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}