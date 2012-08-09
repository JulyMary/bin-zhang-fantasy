package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

@XSerializable(name = "templateAction", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class TemplateAction extends Action
{

	@Override
	public ActionType getType()
	{
		return ActionType.Template;
	}

	@XAttribute(name = "template")
	private String privateTemplate;
	public final String getTemplate()
	{
		return privateTemplate;
	}
	public final void setTemplate(String value)
	{
		privateTemplate = value;
	}
}