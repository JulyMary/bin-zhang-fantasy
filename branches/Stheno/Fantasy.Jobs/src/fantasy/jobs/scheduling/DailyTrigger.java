package fantasy.jobs.scheduling;

import java.text.SimpleDateFormat;

import org.apache.commons.lang3.time.FastDateFormat;

import fantasy.xserialization.*;

@XSerializable(name = "dailyTrigger", namespaceUri=fantasy.jobs.Consts.ScheduleNamespaceURI)
public class DailyTrigger extends Trigger
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 1893138374613804067L;
	public DailyTrigger()
	{
		setDaysInterval(1);
	}

    @XAttribute(name = "daysInterval")
	private int privateDaysInterval;
	public final int getDaysInterval()
	{
		return privateDaysInterval;
	}
	public final void setDaysInterval(int value)
	{
		privateDaysInterval = value;
	}

	@Override
	public TriggerType getType()
	{
		return TriggerType.Daily;
	}

	@Override
	protected String getTriggerTimeDescription()
	{
		java.util.Date time = new java.util.Date((long)this.getStartTime().getTotalMilliseconds());
		FastDateFormat tf = FastDateFormat.getTimeInstance(FastDateFormat.SHORT);
		SimpleDateFormat ff = new SimpleDateFormat();
		return String.format("At %1$s %2$s, starting %3s.", tf.format(time), getDaysInterval() > 1 ? String.format("every %1$s days", getDaysInterval()) : "everyday", ff.format(this.getStartBoundary()));
	}
}