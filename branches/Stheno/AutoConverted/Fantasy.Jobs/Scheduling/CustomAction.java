package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("customAction", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class CustomAction extends Action
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("customType")]
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