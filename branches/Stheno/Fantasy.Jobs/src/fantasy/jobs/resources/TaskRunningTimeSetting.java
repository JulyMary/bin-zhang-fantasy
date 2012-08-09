package fantasy.jobs.resources;

import fantasy.xserialization.*;

@XSerializable(name="TaskRuntimeScheduleSetting", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TaskRunningTimeSetting extends RunningTimeSetting
{
	@XAttribute(name="name")
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	@XAttribute(name="namespace")
	private String privateNamespace;
	public final String getNamespace()
	{
		return privateNamespace;
	}
	public final void setNamespace(String value)
	{
		privateNamespace = value;
	}


}