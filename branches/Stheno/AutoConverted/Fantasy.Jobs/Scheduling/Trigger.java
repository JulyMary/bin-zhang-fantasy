package Fantasy.Jobs.Scheduling;

import Fantasy.XSerialization.*;

//KnownType(typeof(DailyTrigger)) ]
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[DataContract, XSerializable("trigger", NamespaceUri = Consts.ScheduleNamespaceURI)]
public class Trigger
{
	public Trigger()
	{
		this.setStartBoundary(new java.util.Date());
		this.setStartTime(new java.util.Date().AddHours(1).TimeOfDay);

	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("startBoundary")]
	private java.util.Date privateStartBoundary = new java.util.Date(0);
	public final java.util.Date getStartBoundary()
	{
		return privateStartBoundary;
	}
	public final void setStartBoundary(java.util.Date value)
	{
		privateStartBoundary = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("endBoundary")]
	private java.util.Date privateEndBoundary;
	public final java.util.Date getEndBoundary()
	{
		return privateEndBoundary;
	}
	public final void setEndBoundary(java.util.Date value)
	{
		privateEndBoundary = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("executionTimeLimit")]
	private Integer privateExecutionTimeLimit;
	public final Integer getExecutionTimeLimit()
	{
		return privateExecutionTimeLimit;
	}
	public final void setExecutionTimeLimit(Integer value)
	{
		privateExecutionTimeLimit = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XElement("repetition")]
	private Repetition privateRepetition;
	public final Repetition getRepetition()
	{
		return privateRepetition;
	}
	public final void setRepetition(Repetition value)
	{
		privateRepetition = value;
	}

	private TriggerType privateType = TriggerType.forValue(0);
	public TriggerType getType()
	{
		return privateType;
	}
	private void setType(TriggerType value)
	{
		privateType = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember, XAttribute("startTime")]
	private TimeSpan privateStartTime = new TimeSpan();
	public final TimeSpan getStartTime()
	{
		return privateStartTime;
	}
	public final void setStartTime(TimeSpan value)
	{
		privateStartTime = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember(Name="NextRunTime")]
	private java.util.Date _nextRunTime;

	public final java.util.Date getNextRunTime()
	{
		return _nextRunTime;
	}
	public final void setNextRunTime(java.util.Date value)
	{
		_nextRunTime = value;
	}



//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DataMember(Name="LastRunTime"), XAttribute("lastRunTime")]
	private java.util.Date _lastRunTime;

	public final java.util.Date getLastRunTime()
	{
		return _lastRunTime;
	}
	public final void setLastRunTime(java.util.Date value)
	{
		_lastRunTime = value;
	}



	private String privateTriggerTimeDescription;
	protected String getTriggerTimeDescription()
	{
		return privateTriggerTimeDescription;
	}
	private void setTriggerTimeDescription(String value)
	{
		privateTriggerTimeDescription = value;
	}


	public String getDescription()
	{
		StringBuilder rs = new StringBuilder();
		for (String s : new String[] { getTriggerTimeDescription(), getRepetitionDescription(), getExpriedDescription() })
		{

			if (!s.equals(""))
			{
				if (rs.length() > 0)
				{
					rs.append(" ");
				}
				rs.append(s);
			}
		}

		return rs.toString();
	}

	protected String getRepetitionDescription()
	{
		StringBuilder rs = new StringBuilder();
		if (this.getRepetition() != null)
		{
			rs.append(String.format("After triggered repeat every %1$s", ToDurationDescription(getRepetition().getInterval())));
			if (getRepetition().getDuration() != null)
			{
				rs.append(String.format(" for a duration of  %1$s", ToDurationDescription((TimeSpan)getRepetition().getDuration())));
			}
			rs.append(".");
		}

		return rs.toString();
	}

	protected String getExpriedDescription()
	{
		return this.getEndBoundary() != null ? String.format("Trigger expires at %1$s.", this.getEndBoundary()) : "";
	}


	protected final String ToDurationDescription(TimeSpan timespan)
	{

		String day;
		switch (timespan.Days)
		{
			case 1:
				day = " 1 day";
				break;
			case 0:
				day = "";
				break;
			default:
				day = timespan.Days.toString() + " days";
				break;
		}

		String hour;
		switch (timespan.Hours)
		{
			case 1:
				hour = "1 hour";
				break;
			case 0:
				hour = "";
				break;

			default:
				hour = timespan.Hours.toString() + " hours";
				break;
		}


		String minutes;
		switch (timespan.Minutes)
		{
			case 1:
				minutes = "1 minute";
				break;
			case 0:
				minutes = "";
				break;
			default:
				minutes = timespan.Minutes.toString() + " minutes";
				break;
		}


		String seconds;
		switch (timespan.Seconds)
		{
			case 1:
				seconds = "1 second";
				break;
			case 0:
				seconds = "";
				break;
			default:
				seconds = timespan.Seconds.toString() + " seconds";
				break;
		}

		StringBuilder rs = new StringBuilder();
		for (String s : new String[] {day, hour, minutes, seconds })
		{

			if (!s.equals(""))
			{
				if (rs.length() > 0)
				{
					rs.append(" ");
				}
				rs.append(s);
			}
		}

		return rs.toString();
	}


}