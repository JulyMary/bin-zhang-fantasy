package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("action", NamespaceUri=Consts.ScheduleNamespaceURI)]
public abstract class Action
{
	public abstract ActionType getType();
}