package fantasy.jobs.scheduling;

import java.util.Calendar;

import org.apache.commons.lang3.time.DateUtils;
import org.apache.commons.lang3.time.FastDateFormat;

import fantasy.TimeSpan;
import fantasy.xserialization.*;

@XSerializable(name = "timeTrigger", namespaceUri = fantasy.jobs.Consts.ScheduleNamespaceURI)
public class TimeTrigger extends Trigger
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -6686895968828707760L;
	public TimeTrigger()
	{
		this.setDate(DateUtils.truncate(new java.util.Date(), Calendar.DATE));
	}

    @XAttribute(name = "date")
	private java.util.Date privateDate;
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
		java.util.Date t = TimeSpan.add(this.getDate(), this.getStartTime());
		
		FastDateFormat tf = FastDateFormat.getTimeInstance(FastDateFormat.SHORT);
		FastDateFormat df = FastDateFormat.getDateInstance(FastDateFormat.SHORT);
		return String.format("At %1$s on %2$s.", tf.format(t), df.format(t));
	}
}