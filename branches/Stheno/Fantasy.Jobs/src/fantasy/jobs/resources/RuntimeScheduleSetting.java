package fantasy.jobs.resources;

import java.util.*;

import fantasy.configuration.*;
import fantasy.xserialization.*;

@XSerializable(name="runningTimeSchedule", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class RuntimeScheduleSetting
{
	
	

	public final DayRuntimeSetting[] getDaySettings()
	{
		return new DayRuntimeSetting[] { this.getSunday(), this.getMonday(), this.getTuesday(), this.getWednesday(), this.getThursday(), this.getFriday(), this.getSaturday() };
	}





    @XAttribute(name = "enabled")
	private boolean privateEnabled = false;
	public final boolean getEnabled()
	{
		return privateEnabled;
	}
	public final void setEnabled(boolean value)
	{
		privateEnabled = value;
	}

	@XElement(name = "Monday")
	private DayRuntimeSetting privateMonday;
	public final DayRuntimeSetting getMonday()
	{
		return privateMonday;
	}
	public final void setMonday(DayRuntimeSetting value)
	{
		privateMonday = value;
	}

	@XElement(name = "Tuesday")
	private DayRuntimeSetting privateTuesday;
	public final DayRuntimeSetting getTuesday()
	{
		return privateTuesday;
	}
	public final void setTuesday(DayRuntimeSetting value)
	{
		privateTuesday = value;
	}

	@XElement(name = "Wednesday")
	
	private DayRuntimeSetting privateWednesday;
	public final DayRuntimeSetting getWednesday()
	{
		return privateWednesday;
	}
	public final void setWednesday(DayRuntimeSetting value)
	{
		privateWednesday = value;
	}

	@XElement(name = "Thursday")
	private DayRuntimeSetting privateThursday;
	public final DayRuntimeSetting getThursday()
	{
		return privateThursday;
	}
	public final void setThursday(DayRuntimeSetting value)
	{
		privateThursday = value;
	}

	@XElement(name = "Friday")
	private DayRuntimeSetting privateFriday;
	public final DayRuntimeSetting getFriday()
	{
		return privateFriday;
	}
	public final void setFriday(DayRuntimeSetting value)
	{
		privateFriday = value;
	}

	@XElement(name = "Saturday")
	private DayRuntimeSetting privateSaturday;
	public final DayRuntimeSetting getSaturday()
	{
		return privateSaturday;
	}
	public final void setSaturday(DayRuntimeSetting value)
	{
		privateSaturday = value;
	}

	@XElement(name = "Sunday")
	private DayRuntimeSetting privateSunday;
	public final DayRuntimeSetting getSunday()
	{
		return privateSunday;
	}
	public final void setSunday(DayRuntimeSetting value)
	{
		privateSunday = value;
	}




	public final boolean IsDisabledOrInPeriod(java.util.Date datetime)
	{
		if (this.getEnabled())
		{
			
			Calendar cal=Calendar.getInstance();
			 cal.setTime(datetime);

			
			DayRuntimeSetting settings = this.getDaySettings()[cal.get(Calendar.DAY_OF_WEEK)];
			if (settings != null)
			{
				TimeSpan time = datetime.TimeOfDay;
				for (TimeOfDayPeriod period : settings.getPeriods())
				{
					if (time >= period.getStart() && time <= period.getEnd())
					{
						return true;
					}
				}
			}
			return false;
		}
		else
		{
			return true;
		}
	}

	public final Period GetPeriod(java.util.Date baseTime)
	{

		Period rs = null;
		TimeSpan aWeek = new TimeSpan(7, 0, 0, 0);
		for(Period period : GetPeriods(baseTime))
		{
			if (rs == null)
			{
				Period tempVar = new Period();
				tempVar.setStart(period.getStart());
				tempVar.setEnd(period.getEnd());
				rs = tempVar;
			}
			else
			{
				if(period.getStart().equals(rs.getEnd()))
				{
					rs.setEnd(period.getEnd());

					if (rs.getEnd() - rs.getStart() >= aWeek)
					{
						rs.setEnd(java.util.Date.getMaxValue());
						break;
					}
				}
				else
				{
					break;
				}
			}

		}
		return rs;
	}

	private Iterable<Period> GetPeriods(java.util.Date baseTime)
	{
		java.util.Date baseDay = baseTime.Date;

		for (int i = 0;; i++)
		{
			DayRuntimeSetting settings = this.getDaySettings()[((int)baseTime.DayOfWeek + i) % 7];
			if (settings != null)
			{
				java.util.Date day = baseDay.AddDays(i);
				for (TimeOfDayPeriod tp : settings.getPeriods())
				{
					java.util.Date end = day + tp.getEnd();
					if (end.compareTo(baseTime) > 0)
					{
						Period tempVar = new Period();
						tempVar.setStart(day + tp.getStart());
						tempVar.setEnd(end);
//C# TO JAVA CONVERTER TODO TASK: Java does not have an equivalent to the C# 'yield' keyword:
						yield return tempVar;
					}
				}
			}
		}
	}
}