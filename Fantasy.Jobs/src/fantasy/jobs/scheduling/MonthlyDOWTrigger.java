package fantasy.jobs.scheduling;

import java.text.DateFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.Arrays;

import org.apache.commons.lang3.ArrayUtils;
import org.apache.commons.lang3.time.FastDateFormat;

import fantasy.*;
import fantasy.collections.*;
import fantasy.xserialization.*;

@XSerializable(name = "monthlyDOWTrigger", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class MonthlyDOWTrigger extends Trigger
{



	/**
	 * 
	 */
	private static final long serialVersionUID = 6799711610222635965L;
	@Override
	public TriggerType getType()
	{
		return TriggerType.MonthlyDayOfWeek;
	}

	public MonthlyDOWTrigger()
	{
		this.setMonthsOfYear(new int[0]);
		this.setWeeksOfMonth(new WeekOfMonth[0]);
		this.setDaysOfWeek(new DayOfWeek[0]);
	}

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

	@XAttribute(name = "monthsOfYear")
	private int[] privateMonthsOfYear;
	public final int[] getMonthsOfYear()
	{
		return privateMonthsOfYear;
	}
	public final void setMonthsOfYear(int[] value)
	{
		privateMonthsOfYear = value;
	}


	@XAttribute(name = "runOnLastWeekOfMonth")
	private boolean privateRunOnLastWeekOfMonth;
	public final boolean getRunOnLastWeekOfMonth()
	{
		return privateRunOnLastWeekOfMonth;
	}
	public final void setRunOnLastWeekOfMonth(boolean value)
	{
		privateRunOnLastWeekOfMonth = value;
	}


	@XAttribute(name = "weeksOfMonth")
	private WeekOfMonth[] privateWeeksOfMonth;
	public final WeekOfMonth[] getWeeksOfMonth()
	{
		return privateWeeksOfMonth;
	}
	public final void setWeeksOfMonth(WeekOfMonth[] value)
	{
		privateWeeksOfMonth = value;
		
		
	}

	private static String[] _monthNames = new DateFormatSymbols().getMonths();
	private static String[] _dayNames = new DateFormatSymbols().getWeekdays();
	@Override
	protected String getTriggerTimeDescription()
	{
		
		java.util.Date time = new java.util.Date(this.getStartTime().getMilliseconds());
		FastDateFormat tf = FastDateFormat.getTimeInstance(FastDateFormat.SHORT);
		SimpleDateFormat sf = new SimpleDateFormat();
		
		
		Arrays.asList(getMonthsOfYear());
		return String.format("At %1$s on the %2$s %3$s each %4$s, starting %5$s.",tf.format(time), 
			StringUtils2.join(",", new Enumerable<WeekOfMonth>(getWeeksOfMonth()).select(new Selector<WeekOfMonth, String>(){

			@Override
			public String select(WeekOfMonth item) {
				return item.name();
			}})),
			StringUtils2.join(",", new Enumerable<DayOfWeek>(getDaysOfWeek()).select(new Selector<DayOfWeek, String>(){

			@Override
			public String select(DayOfWeek item) {
				return _dayNames[item.getValue()];
			}})), 
			StringUtils2.join(",", new Enumerable<Integer>(ArrayUtils.toObject(this.getMonthsOfYear())).select(new Selector<Integer, String>(){

			@Override
			public String select(Integer month) {
				return _monthNames[month- 1];
			}})), sf.format(this.getStartBoundary()));
	}
}