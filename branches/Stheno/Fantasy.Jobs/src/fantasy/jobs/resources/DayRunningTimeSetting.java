package fantasy.jobs.resources;

import java.text.ParseException;
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





	private static final TimeSpan _oneDay = TimeSpan.fromDays(1);
	private TimeSpan paseDuration(String text) throws Exception
	{

		TimeSpan rs = TimeSpan.parse(text);

		if (rs.isGreaterThan(_oneDay))
		{
			throw new ParseException("Period time must between 00:00:00 to 24:00:00", 0);
		}

		return rs;

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
			text.append(period.getStart());
			text.append("-");
			text.append(period.getEnd());
		}
		element.setText(text.toString());

	}
}