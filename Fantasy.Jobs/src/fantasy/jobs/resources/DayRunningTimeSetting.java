package fantasy.jobs.resources;

import java.text.ParseException;
import java.util.regex.*;

import org.jdom2.Element;
import fantasy.IServiceProvider;
import fantasy.xserialization.*;
import fantasy.*;

@XSerializable(name="DayRunningTime", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class DayRunningTimeSetting implements IXSerializable
{
	private java.util.ArrayList<TimeOfDayPeriod> _periods = new java.util.ArrayList<TimeOfDayPeriod>();
	public final java.util.ArrayList<TimeOfDayPeriod> getPeriods()
	{
		return _periods;
	}

	@Override
	public boolean equals(Object obj)
	{
		DayRunningTimeSetting other = (DayRunningTimeSetting)((obj instanceof DayRunningTimeSetting) ? obj : null);
		if (other != null && other.getPeriods().size() == this.getPeriods().size())
		{
			for (int i = 0; i < this.getPeriods().size(); i++)
			{
				if (!this.getPeriods().get(i).getStart().equals(other.getPeriods().get(i).getStart()) || !this.getPeriods().get(i).getEnd().equals(other.getPeriods().get(i).getEnd()))
				{
					return false;
				}
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	@Override
	public int hashCode()
	{
		return super.hashCode();
	}





	private static final Pattern _timeSpanRegex = Pattern.compile("^\\s*(?<h>\\d{1,2}):(?<m>\\d{1,2}):(?<s>\\d{1,2})\\s*$");
	private static final org.joda.time.Period _oneDay = org.joda.time.Period.days(1);
	private org.joda.time.Period paseDuration(String text) throws Exception
	{
		Matcher match = _timeSpanRegex.matcher(text);
		if (match.lookingAt())
		{
			int h = Integer.parseInt(match.group("h"));
			int m = Integer.parseInt(match.group("m"));
			int s = Integer.parseInt(match.group("s"));

			org.joda.time.Period rs = org.joda.time.Period.hours(h).withMinutes(m).withSeconds(s);

			if (rs.toStandardDuration().isLongerThan(_oneDay.toStandardDuration()))
			{
				throw new ParseException("Period time must between 00:00:00 to 24:00:00", 0);
			}

			return rs;

		}
		else
		{
			throw new  ParseException("Invalid period time format.", 0);
		}
	}

	
	@Override
	public void Load(IServiceProvider context, Element element)
			throws Exception {

       String value = element.getTextTrim();
       
       if (!StringUtils2.isNullOrWhiteSpace(value))
		{
			for (String periodText : StringUtils2.split(value, ";",true))
			{
				String[] times = StringUtils2.split(periodText, "-", true);
				if (times.length == 2)
				{
					TimeOfDayPeriod period = new TimeOfDayPeriod();
					period.setStart(paseDuration(times[0]));
					period.setEnd(paseDuration(times[1]));
					this.getPeriods().add(period);
				}
				else
				{
					throw new ParseException("Invalid period format.", 0);
				}

			}
		}
		
	}

	@Override
	public void Save(IServiceProvider context, Element element)
			throws Exception {
		StringBuilder text = new StringBuilder();
		for (TimeOfDayPeriod period : this.getPeriods())
		{
			if (text.length() > 0)
			{
				text.append(';');
			}
			text.append(String.format("%02d:%1.2d:%2.2d-%3.2d:%4.2d:%5.2d", period.getStart().getDays() * 24 + period.getStart().getHours(), period.getStart().getMinutes(), period.getStart().getSeconds(), period.getEnd().getDays() * 24 + period.getEnd().getHours(), period.getEnd().getMinutes(), period.getEnd().getSeconds()));
		}
		element.setText(text.toString());
		
	}
}