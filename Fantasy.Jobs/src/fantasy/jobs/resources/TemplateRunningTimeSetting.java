package fantasy.jobs.resources;

import fantasy.xserialization.*;



@XSerializable(name="templateRunningTime", namespaceUri = fantasy.jobs.Consts.XNamespaceURI)
public class TemplateRunningTimeSetting extends RunningTimeSetting
{
	@XAttribute(name = "name")
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}
}