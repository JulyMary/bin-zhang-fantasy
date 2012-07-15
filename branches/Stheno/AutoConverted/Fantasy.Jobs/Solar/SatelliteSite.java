package Fantasy.Jobs.Solar;

import Fantasy.Jobs.Management.*;
import Fantasy.ServiceModel.*;

public class SatelliteSite
{
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	private ISatellite privateSatellite;
	public final ISatellite getSatellite()
	{
		return privateSatellite;
	}
	public final void setSatellite(ISatellite value)
	{
		privateSatellite = value;
	}

	private java.util.Date privateLastEchoTime = new java.util.Date(0);
	public final java.util.Date getLastEchoTime()
	{
		return privateLastEchoTime;
	}
	public final void setLastEchoTime(java.util.Date value)
	{
		privateLastEchoTime = value;
	}
}