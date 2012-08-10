package fantasy;

import java.util.*;

import org.apache.commons.lang3.time.DateUtils;

public final class DateUtils2 {
   
	private DateUtils2()
	{
		
	}
	
	public static Date getDate(Date datetime)
	{
		return DateUtils.truncate(datetime, Calendar.DATE);
	}

	public static int getDay(Date dateTime)
	{
		return toCalendar(dateTime).get(Calendar.DAY_OF_MONTH);
	}
	
	public static DayOfWeek getDayOfWeek(Date dateTime)
	{
		return DayOfWeek.forValue(toCalendar(dateTime).get(Calendar.DAY_OF_WEEK));
	}
	
	public static int getMonth(Date dateTime)
	{
		return toCalendar(dateTime).get(Calendar.MONTH);
	}
	
	public static Calendar toCalendar(Date dateTime)
	{
		Calendar rs = Calendar.getInstance();
		rs.setTime(dateTime);
		return rs;
	
	}
	
	
	public static Date thisMonth(Date dateTime)
	{
        Calendar cal = toCalendar(DateUtils2.getDate(dateTime));
		cal.set(Calendar.DATE, 1);
		return cal.getTime();
	}
	
	public static Date nextMonth(Date dateTime)
	{
		Calendar cal = toCalendar(DateUtils2.getDate(dateTime));
		
		cal.set(Calendar.DATE, 1);
		cal.add(Calendar.MONTH, 1);
		return cal.getTime();
	}
	
	public static Date nextDay(Date dateTime)
	{
		Calendar cal = toCalendar(DateUtils2.getDate(dateTime));
		cal.add(Calendar.DATE, 1);
		return cal.getTime();
	}
	
	public static Date nextSunday(Date dateTime)
	{
		Calendar cal = toCalendar(DateUtils2.getDate(dateTime));
		int dow = cal.get(Calendar.DAY_OF_WEEK);
		
		int days = 7 - dow;
		cal.add(Calendar.DATE, days);
		
		return cal.getTime();
	}
	
	public static Date thisSunday(Date dateTime)
	{
		Calendar cal = toCalendar(DateUtils2.getDate(dateTime));
		int dow = cal.get(Calendar.DAY_OF_WEEK);
		
		
		cal.add(Calendar.DATE, -dow);
		
		return cal.getTime();
	}
	
}
