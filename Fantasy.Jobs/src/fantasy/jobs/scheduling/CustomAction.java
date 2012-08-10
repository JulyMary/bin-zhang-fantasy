package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

@XSerializable(name = "customAction", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class CustomAction extends ScheduleAction
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -6402181398370044024L;
	@XAttribute(name = "customType")
	private String privateCustomType;
	public final String getCustomType()
	{
		return privateCustomType;
	}
	public final void setCustomType(String value)
	{
		privateCustomType = value;
	}

	@Override
	public ActionType getType()
	{
		return ActionType.Custom;
	}
}