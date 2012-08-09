package fantasy.jobs.scheduling;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract(Name="Time"), XSerializable("timeTrigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class TimeTrigger extends Trigger
{

	public TimeTrigger()
	{
		this.setDate(new java.util.Date().Date);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("date")]
	private java.util.Date privateDate = new java.util.Date(0);
	public final java.util.Date getDate()
	{
		return privateDate;
	}
	public final void setDate(java.util.Date value)
	{
		privateDate = value;
	}

	@Override
	public TriggerType getType()
	{
		return TriggerType.Time;
	}



	@Override
	protected String getTriggerTimeDescription()
	{
		java.util.Date t = this.getDate() + this.getStartTime();
//C# TO JAVA CONVERTER TODO TASK: The 't' format specifier is not converted to Java:
//ORIGINAL LINE: return string.Format("At {0:t} on {0:d}.", t);
		return String.format("At %0s on %d.", t);
	}
}