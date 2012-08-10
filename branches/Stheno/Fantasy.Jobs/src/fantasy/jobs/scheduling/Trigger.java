package fantasy.jobs.scheduling;

import java.io.Serializable;
import java.util.*;

import org.apache.commons.lang3.time.DateUtils;


import fantasy.*;
import fantasy.xserialization.*;

@XSerializable(name = "trigger", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class Trigger implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -8438411265484228219L;
	public Trigger()
	{
	    Date now = new Date();
		
		this.setStartBoundary(now);
		this.setStartTime(TimeSpan.timeOfDay(DateUtils.addHours(now, 1)));

	}

	@XAttribute(name = "startBoundary")
	private java.util.Date privateStartBoundary = new java.util.Date(Long.MIN_VALUE);
	public final java.util.Date getStartBoundary()
	{
		return privateStartBoundary;
	}
	public final void setStartBoundary(java.util.Date value)
	{
		privateStartBoundary = value;
	}

    @XAttribute(name = "endBoundary")
	private java.util.Date privateEndBoundary;
	public final java.util.Date getEndBoundary()
	{
		return privateEndBoundary;
	}
	public final void setEndBoundary(java.util.Date value)
	{
		privateEndBoundary = value;
	}

    @XAttribute(name="executionTimeLimit")
	private Integer privateExecutionTimeLimit;
	public final Integer getExecutionTimeLimit()
	{
		return privateExecutionTimeLimit;
	}
	public final void setExecutionTimeLimit(Integer value)
	{
		privateExecutionTimeLimit = value;
	}

    @XElement(name = "repetition")
	private Repetition privateRepetition;
	public final Repetition getRepetition()
	{
		return privateRepetition;
	}
	public final void setRepetition(Repetition value)
	{
		privateRepetition = value;
	}

	private TriggerType privateType = TriggerType.Time;
	public TriggerType getType()
	{
		return privateType;
	}
	

    @XAttribute(name="startTime")
	private TimeSpan privateStartTime;
	public final TimeSpan getStartTime()
	{
		return privateStartTime;
	}
	public final void setStartTime(TimeSpan value)
	{
		privateStartTime = value;
	}

	private java.util.Date _nextRunTime;

	public final java.util.Date getNextRunTime()
	{
		return _nextRunTime;
	}
	public final void setNextRunTime(java.util.Date value)
	{
		_nextRunTime = value;
	}


    @XAttribute(name = "lastRunTime")
	private java.util.Date _lastRunTime;

	public final java.util.Date getLastRunTime()
	{
		return _lastRunTime;
	}
	public final void setLastRunTime(java.util.Date value)
	{
		_lastRunTime = value;
	}



	private String privateTriggerTimeDescription = "";
	protected String getTriggerTimeDescription()
	{
		return privateTriggerTimeDescription;
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
		switch (timespan.getDays())
		{
			case 1:
				day = " 1 day";
				break;
			case 0:
				day = "";
				break;
			default:
				day = Integer.toString(timespan.getDays()) + " days";
				break;
		}

		String hour;
		switch (timespan.getHours())
		{
			case 1:
				hour = "1 hour";
				break;
			case 0:
				hour = "";
				break;

			default:
				hour = Integer.toString(timespan.getHours()) + " hours";
				break;
		}


		String minutes;
		switch (timespan.getMinutes())
		{
			case 1:
				minutes = "1 minute";
				break;
			case 0:
				minutes = "";
				break;
			default:
				minutes = Integer.toString(timespan.getMinutes()) + " minutes";
				break;
		}


		String seconds;
		switch (timespan.getSeconds())
		{
			case 1:
				seconds = "1 second";
				break;
			case 0:
				seconds = "";
				break;
			default:
				seconds = Integer.toString(timespan.getSeconds()) + " seconds";
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