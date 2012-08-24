package fantasy.jobs.solar;

import fantasy.jobs.management.*;

public class JobStartupData
{
	private String privateSatellite;
	public final String getSatellite()
	{
		return privateSatellite;
	}
	public final void setSatellite(String value)
	{
		privateSatellite = value;
	}

	private JobMetaData privateJobMetaData;
	public final JobMetaData getJobMetaData()
	{
		return privateJobMetaData;
	}
	public final void setJobMetaData(JobMetaData value)
	{
		privateJobMetaData = value;
	}
}