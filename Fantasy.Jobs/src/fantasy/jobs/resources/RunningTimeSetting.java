package fantasy.jobs.resources;

import java.util.*;

import org.joda.time.DateTime;
import org.joda.time.LocalDate;
import org.joda.time.LocalTime;

import fantasy.xserialization.*;

@XSerializable(name="runningTimeSchedule", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class RunningTimeSetting
{
	
	

	public final DayRunningTimeSetting[] getDaySettings()
	{
		return new DayRunningTimeSetting[] { this.getSunday(), this.getMonday(), this.getTuesday(), this.getWednesday(), this.getThursday(), this.getFriday(), this.getSaturday() };
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
	private DayRunningTimeSetting privateMonday;
	public final DayRunningTimeSetting getMonday()
	{
		return privateMonday;
	}
	public final void setMonday(DayRunningTimeSetting value)
	{
		privateMonday = value;
	}

	@XElement(name = "Tuesday")
	private DayRunningTimeSetting privateTuesday;
	public final DayRunningTimeSetting getTuesday()
	{
		return privateTuesday;
	}
	public final void setTuesday(DayRunningTimeSetting value)
	{
		privateTuesday = value;
	}

	@XElement(name = "Wednesday")
	
	private DayRunningTimeSetting privateWednesday;
	public final DayRunningTimeSetting getWednesday()
	{
		return privateWednesday;
	}
	public final void setWednesday(DayRunningTimeSetting value)
	{
		privateWednesday = value;
	}

	@XElement(name = "Thursday")
	private DayRunningTimeSetting privateThursday;
	public final DayRunningTimeSetting getThursday()
	{
		return privateThursday;
	}
	public final void setThursday(DayRunningTimeSetting value)
	{
		privateThursday = value;
	}

	@XElement(name = "Friday")
	private DayRunningTimeSetting privateFriday;
	public final DayRunningTimeSetting getFriday()
	{
		return privateFriday;
	}
	public final void setFriday(DayRunningTimeSetting value)
	{
		privateFriday = value;
	}

	@XElement(name = "Saturday")
	private DayRunningTimeSetting privateSaturday;
	public final DayRunningTimeSetting getSaturday()
	{
		return privateSaturday;
	}
	public final void setSaturday(DayRunningTimeSetting value)
	{
		privateSaturday = value;
	}

	@XElement(name = "Sunday")
	private DayRunningTimeSetting privateSunday;
	public final DayRunningTimeSetting getSunday()
	{
		return privateSunday;
	}
	public final void setSunday(DayRunningTimeSetting value)
	{
		privateSunday = value;
	}


    private long MILLIS_PER_DAY = 24 * 60 * 60 * 1000; 


	public final boolean IsDisabledOrInPeriod(java.util.Date datetime)
	{
		if (this.getEnabled())
		{
			
			Calendar cal=Calendar.getInstance();
			cal.setTime(datetime);

			
			DayRunningTimeSetting settings = this.getDaySettings()[cal.get(Calendar.DAY_OF_WEEK)];
			if (settings != null)
			{
				long time = datetime.getTime() % MILLIS_PER_DAY;
				for (TimeOfDayPeriod period : settings.getPeriods())
				{
					if (time >= period.getStart().toStandardDuration().getMillis() && time <= period.getEnd().toStandardDuration().getMillis())
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

	
	private static long A_WEEK = 7 * 24 * 60 * 60 * 1000;
	
	public final Period GetPeriod(java.util.Date baseTime)
	{

		Period rs = null;
	
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

					if (rs.getEnd().getTime() - rs.getStart().getTime() >= A_WEEK)
					{
						rs.setEnd(new java.util.Date(Long.MAX_VALUE));
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
		DateTime dt = new DateTime(baseTime.getTime());
		LocalDate baseDay = dt.toLocalDate();

	    ArrayList<Period> rs = new ArrayList<Period>();
		
		for (int i = 0; i < 7; i++)
		{
			DayRunningTimeSetting settings = this.getDaySettings()[(dt.getDayOfWeek() + i) % 7];
			if (settings != null)
			{
				LocalDate day = baseDay.plusDays(1);
				for (TimeOfDayPeriod tp : settings.getPeriods())
				{
					DateTime end = day.toDateTime(LocalTime.fromMillisOfDay(tp.getEnd().toStandardDuration().getMillis()));
					if (end.compareTo(dt) > 0)
					{
						Period tempVar = new Period();
						tempVar.setStart(day.toDateTime(LocalTime.fromMillisOfDay(tp.getStart().toStandardDuration().getMillis())).toDate());
						tempVar.setEnd(end.toDate());
						rs.add(tempVar);
					}
				}
			}
		}
		return rs;
	}
}