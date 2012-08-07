package fantasy.jobs.resources;

import Fantasy.Configuration.*;

public class DayRuntimeSetting implements IXmlSerializable
{
	private java.util.ArrayList<TimeOfDayPeriod> _periods = new java.util.ArrayList<TimeOfDayPeriod>();
	public final java.util.ArrayList<TimeOfDayPeriod> getPeriods()
	{
		return _periods;
	}

	@Override
	public boolean equals(Object obj)
	{
		DayRuntimeSetting other = (DayRuntimeSetting)((obj instanceof DayRuntimeSetting) ? obj : null);
		if (other != null && other.getPeriods().size() == this.getPeriods().size())
		{
			for (int i = 0; i < this.getPeriods().size(); i++)
			{
				if (this.getPeriods().get(i).getStart() != other.getPeriods().get(i).getStart() || this.getPeriods().get(i).getEnd() != other.getPeriods().get(i).getEnd())
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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXmlSerializable Members

	public final System.Xml.Schema.XmlSchema GetSchema()
	{
		return null;
	}

	public final void ReadXml(System.Xml.XmlReader reader)
	{
		this.getPeriods().clear();
		reader.MoveToContent();
		String value = reader.ReadElementString();


		if (!String.IsNullOrWhiteSpace(value))
		{
			for (String periodText : value.split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				String[] times = periodText.split("[-]", -1);
				if (times.length == 2)
				{
					TimeOfDayPeriod period = new TimeOfDayPeriod();
					period.setStart(PaseTimeSpan(times[0]));
					period.setEnd(PaseTimeSpan(times[1]));
					this.getPeriods().add(period.clone());
				}
				else
				{
					throw new FormatException("Invalid period format.");
				}

			}
		}
	}


	private static final Regex _timeSpanRegex = new Regex("^\\s*(?<h>\\d{1,2}):(?<m>\\d{1,2}):(?<s>\\d{1,2})\\s*$");
	private static final TimeSpan _oneDay = new TimeSpan(1, 0, 0, 0);
	private TimeSpan PaseTimeSpan(String text)
	{
		Match match = _timeSpanRegex.Match(text);
		if (match.Success)
		{
			int h = Integer.parseInt(match.Groups["h"].getValue());
			int m = Integer.parseInt(match.Groups["m"].getValue());
			int s = Integer.parseInt(match.Groups["s"].getValue());

			TimeSpan rs = new TimeSpan(h, m, s);

			if (rs > _oneDay)
			{
				throw new OverflowException("Period time must between 00:00:00 to 24:00:00");
			}

			return rs;

		}
		else
		{
			throw new FormatException("Invalid period time format.");
		}
	}

	public final void WriteXml(System.Xml.XmlWriter writer)
	{
		StringBuilder text = new StringBuilder();
		for (TimeOfDayPeriod period : this.getPeriods())
		{
			if (text.length() > 0)
			{
				text.append(';');
			}
			text.append(String.format("%02d:%1.2d:%2.2d-%3.2d:%4.2d:%5.2d", period.getStart().Days * 24 + period.getStart().Hours, period.getStart().Minutes, period.getStart().Seconds, period.getEnd().Days * 24 + period.getEnd().Hours, period.getEnd().Minutes, period.getEnd().Seconds));
		}
		writer.WriteString(text.toString());
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}