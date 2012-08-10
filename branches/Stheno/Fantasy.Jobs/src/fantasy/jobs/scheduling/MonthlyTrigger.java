package fantasy.jobs.scheduling;

import java.text.DateFormatSymbols;
import java.text.SimpleDateFormat;
import java.util.*;

import org.apache.commons.lang3.time.FastDateFormat;

import fantasy.xserialization.*;
import fantasy.*;

@XSerializable(name = "monthlyTrigger", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class MonthlyTrigger extends Trigger
{
    /**
	 * 
	 */
	private static final long serialVersionUID = -6883477019001524142L;
	@XAttribute(name = "daysOfMonth")
	private int[] privateDaysOfMonth;
	public final int[] getDaysOfMonth()
	{
		return privateDaysOfMonth;
	}
	public final void setDaysOfMonth(int[] value)
	{
		privateDaysOfMonth = value;
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

    @XAttribute(name = "runOnLastDayOfMonth")
	private boolean privateRunOnLastDayOfMonth;
	public final boolean getRunOnLastDayOfMonth()
	{
		return privateRunOnLastDayOfMonth;
	}
	public final void setRunOnLastDayOfMonth(boolean value)
	{
		privateRunOnLastDayOfMonth = value;
	}


	public MonthlyTrigger()
	{
		this.setDaysOfMonth(new int[0]);
		this.setMonthsOfYear(new int[0]);
	}


	@Override
	public TriggerType getType()
	{
		return TriggerType.Monthly;
	}


	private Iterable<String> getDayNames()
	{
		
		ArrayList<String> rs = new ArrayList<String>();
		for (int d : getDaysOfMonth())
		{

			rs.add(new Integer(d).toString());
		}

		if (this.getRunOnLastDayOfMonth())
		{
			rs.add("last");
		}
		
		return rs;
	}
	
	private static String[] _monthNames = new DateFormatSymbols().getMonths();

	private Iterable<String> getMonthNames()
	{
		ArrayList<String> rs = new ArrayList<String>();
		for (int m : getMonthsOfYear())
		{
			rs.add(_monthNames[m - 1]);
		}
		return rs;
	}

	@Override
	protected String getTriggerTimeDescription()
	{
		java.util.Date time = new java.util.Date((long)this.getStartTime().getTotalMilliseconds());
		FastDateFormat tf = FastDateFormat.getTimeInstance(FastDateFormat.SHORT);
		SimpleDateFormat sf = new SimpleDateFormat();
		return String.format("At %1$s on day %2$s of %3$s, starting %4$s.",tf.format(time), StringUtils2.join(",", getDayNames()),StringUtils2.join(",", getMonthNames()), sf.format(this.getStartBoundary()));
	}
}