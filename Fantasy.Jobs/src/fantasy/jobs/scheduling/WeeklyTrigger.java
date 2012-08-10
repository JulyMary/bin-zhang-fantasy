package fantasy.jobs.scheduling;

import java.text.DateFormatSymbols;
import java.text.SimpleDateFormat;

import org.apache.commons.lang3.time.FastDateFormat;


import fantasy.*;
import fantasy.collections.*;
import fantasy.xserialization.*;

@XSerializable(name = "weeklyTrigger", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class WeeklyTrigger extends Trigger
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 5003088522985743724L;

	@XAttribute(name = "daysOfWeek")
	private DayOfWeek[] privateDaysOfWeek;
	public final DayOfWeek[] getDaysOfWeek()
	{
		return privateDaysOfWeek;
	}
	public final void setDaysOfWeek(DayOfWeek[] value)
	{
		privateDaysOfWeek = value;
	}

	@XAttribute(name = "weeksInterval")
	private int privateWeeksInterval;
	public final int getWeeksInterval()
	{
		return privateWeeksInterval;
	}
	public final void setWeeksInterval(int value)
	{
		privateWeeksInterval = value;
	}

	public WeeklyTrigger()
	{
		this.setDaysOfWeek(new DayOfWeek[0]);
		setWeeksInterval(1);
	}

	@Override
	public TriggerType getType()
	{
		return TriggerType.Weekly;
	}
	
	private static String[] _dayNames = new DateFormatSymbols().getWeekdays();

	@Override
	protected String getTriggerTimeDescription()
	{

		java.util.Date time = new java.util.Date(this.getStartTime().getMilliseconds());
		FastDateFormat tf = FastDateFormat.getTimeInstance(FastDateFormat.SHORT);
		SimpleDateFormat sf = new SimpleDateFormat();
		return String.format("At %1$s every %2$s of every %3$s %4$s, starting %$5s.",tf.format(time), StringUtils2.join(",", new Enumerable<DayOfWeek>(this.getDaysOfWeek()).select(new Selector<DayOfWeek, String>(){

			@Override
			public String select(DayOfWeek item) {
				return _dayNames[item.getValue()];
			}})), getWeeksInterval(), getWeeksInterval() > 1 ? "weeks" : "week", sf.format(this.getStartBoundary()));
	}
}