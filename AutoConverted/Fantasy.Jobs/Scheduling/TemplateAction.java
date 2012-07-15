package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("templateAction", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class TemplateAction extends Action
{

	@Override
	public ActionType getType()
	{
		return ActionType.Template;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("template")]
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