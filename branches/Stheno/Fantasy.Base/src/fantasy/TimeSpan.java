package fantasy;

import java.io.Serializable;
import java.util.*;
import java.util.regex.*;



@SuppressWarnings("unused")
public final class TimeSpan implements Comparable<TimeSpan>, Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 2931588839914726112L;
	public static final long TicksPerMillisecond = 0x2710L;
	private static final double MillisecondsPerTick = 0.0001;
	public static final long TicksPerSecond = 0x989680L;
	private static final double SecondsPerTick = 1E-07;
	public static final long TicksPerMinute = 0x23c34600L;
	private static final double MinutesPerTick = 1.6666666666666667E-09;
	public static final long TicksPerHour = 0x861c46800L;
	private static final double HoursPerTick = 2.7777777777777777E-11;
	public static final long TicksPerDay = 0xc92a69c000L;
	private static final double DaysPerTick = 1.1574074074074074E-12;
	private static final int MillisPerSecond = 0x3e8;
	private static final int MillisPerMinute = 0xea60;
	private static final int MillisPerHour = 0x36ee80;
	
	private static final int MillisPerDay = 0x5265c00;
	public static final long MaxSeconds = 0xd6bf94d5e5L;
	public static final long MinSeconds = -922337203685L;
	public static final long MaxMilliSeconds = 0x346dc5d638865L;
	public static final long MinMilliSeconds = -922337203685477L;
	public static final long TicksPerTenthSecond = 0xf4240L;
	public static TimeSpan Zero;
	public static TimeSpan MaxValue;
	public static TimeSpan MinValue;
	public long _ticks;
	private static boolean _legacyConfigChecked;
	private static boolean _legacyMode;
	
	public TimeSpan()
	{
		this._ticks = 0;
	}
	
	public TimeSpan(long ticks)
	{
		this._ticks = ticks;
	}

	public TimeSpan(int hours, int minutes, int seconds)
	{
		this._ticks = timeToTicks(hours, minutes, seconds);
	}

	public TimeSpan(int days, int hours, int minutes, int seconds)
	{
		this(days, hours, minutes, seconds, 0);
	}

	public TimeSpan(int days, int hours, int minutes, int seconds, int milliseconds)
	{
		long num = ((((((days * 0xe10L) * 0x18L) + (hours * 0xe10L)) + (minutes * 60L)) + seconds) * 0x3e8L) + milliseconds;
		if ((num > 0x346dc5d638865L) || (num < -922337203685477L))
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		this._ticks = num * 0x2710L;
	}

	public long getTicks()
	{
		return this._ticks;
	}
	public int getDays()
	{
		return (int)(this._ticks / 0xc92a69c000L);
	}
	public int getHours()
	{
		return (int)((this._ticks / 0x861c46800L) % 0x18L);
	}
	public int getMilliseconds()
	{
		return (int)((this._ticks / 0x2710L) % 0x3e8L);
	}
	public int getMinutes()
	{
		return (int)((this._ticks / 0x23c34600L) % 60L);
	}
	public int getSeconds()
	{
		return (int)((this._ticks / 0x989680L) % 60L);
	}
	public double getTotalDays()
	{
		return (this._ticks * 1.1574074074074074E-12);
	}
	public double getTotalHours()
	{
		return (this._ticks * 2.7777777777777777E-11);
	}
	public double getTotalMilliseconds()
	{
		double num = this._ticks * 0.0001;
		if (num > 922337203685477L)
		{
			return 922337203685477L;
		}
		if (num < -922337203685477L)
		{
			return -922337203685477L;
		}
		return num;
	}
	public double getTotalMinutes()
	{
		return (this._ticks * 1.6666666666666667E-09);
	}
	 private double getTotalSeconds()
	{
		return (this._ticks * 1E-07);
	}
	public TimeSpan Add(TimeSpan ts)
	{
		long ticks = this._ticks + ts._ticks;
		if (((this._ticks >> 0x3f) == (ts._ticks >> 0x3f)) && ((this._ticks >> 0x3f) != (ticks >> 0x3f)))
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan(ticks);
	}

	public static int compare(TimeSpan t1, TimeSpan t2)
	{
		if(t1 == null && t2 == null)
		{
			return 0;
		}
		else if(t1 == null)
		{
			return -1;
		}
		else if(t2 == null)
		{
			return 1;
		}
		else if (t1._ticks > t2._ticks)
		{
			return 1;
		}
		else if (t1._ticks < t2._ticks)
		{
			return -1;
		}
		else
		{
		    return 0;
		}
	}

	

	public int compareTo(TimeSpan value)
	{
		return TimeSpan.compare(this, value);
	}

	public static TimeSpan fromDays(double value)
	{
		return interval(value, 0x5265c00);
	}

	public TimeSpan duration()
	{
		if (this.getTicks() == MinValue.getTicks())
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan((this._ticks >= 0L) ? this._ticks : -this._ticks);
	}

	@Override
	public boolean equals(Object value)
	{
		return ((value instanceof TimeSpan) && (this._ticks == ((TimeSpan) value)._ticks));
	}

	public boolean equals(TimeSpan obj)
	{
		return compare(this, obj) == 0;
	}

	public static boolean equals(TimeSpan t1, TimeSpan t2)
	{
		return compare(t1, t2) == 0;
	}

	@Override
	public int hashCode()
	{
		return (((int) this._ticks) ^ ((int)(this._ticks >> 0x20)));
	}

	public static TimeSpan fromHours(double value)
	{
		return interval(value, 0x36ee80);
	}

	private static TimeSpan interval(double value, int scale)
	{
		if (Double.isNaN(value))
		{
			throw new IllegalArgumentException("Value cannot be NaN");
		}
		double num = value * scale;
		double num2 = num + ((value >= 0.0) ? 0.5 : -0.5);
		if ((num2 > 922337203685477L) || (num2 < -922337203685477L))
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan(((long) num2) * 0x2710L);
	}

	public static TimeSpan fromMilliseconds(double value)
	{
		return interval(value, 1);
	}

	public static TimeSpan fromMinutes(double value)
	{
		return interval(value, 0xea60);
	}

	public TimeSpan negate()
	{
		if (this.getTicks() == MinValue.getTicks())
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan(-this._ticks);
	}

	public static TimeSpan fromSeconds(double value)
	{
		return interval(value, 0x3e8);
	}

	public TimeSpan subtract(TimeSpan ts)
	{
		long ticks = this._ticks - ts._ticks;
		if (((this._ticks >> 0x3f) != (ts._ticks >> 0x3f)) && ((this._ticks >> 0x3f) != (ticks >> 0x3f)))
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan(ticks);
	}

	public static TimeSpan fromTicks(long value)
	{
		return new TimeSpan(value);
	}

	public static long timeToTicks(int hour, int minute, int second)
	{
		long num = ((hour * 0xe10L) + (minute * 60L)) + second;
		if ((num > 0xd6bf94d5e5L) || (num < -922337203685L))
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return (num * 0x989680L);
	}

	
	private static final Pattern _timeSpanRegex = Pattern.compile("^(?<u>[\\+\\-])?((?<d>\\d+)\\.)?(?<h>\\d{1,2}):(?<m>\\d{1,2})(:(?<s>\\d{1,2})(\\.(?<mil>\\d+))?)?$");
	
	public static TimeSpan parse(String text)
	{
		Matcher matcher = _timeSpanRegex.matcher(text);
		if(matcher.lookingAt())
		{
			int d = getInt(matcher.group("d"));
			int h = getInt(matcher.group("h"));
			int m = getInt(matcher.group("m"));
			int s = getInt(matcher.group("s"));
			int mil = getInt(matcher.group("mil"));
			
			TimeSpan rs = new TimeSpan(d,h,m,s, mil);
			
			if("-".equals(matcher.group("u")))
			{
				rs._ticks = -rs._ticks;
			}
			
			return rs;
		}
		else
		{
			throw new TimeSpanParseException("Invalid TimeSpan string format.");
		}
	}
	
	private static int getInt(String s)
	{
		if(s == null)
		{
			return 0;
		}
		else
		{
			return Integer.parseInt(s);
		}
	}

	

	@Override
	public String toString()
	{
		StringBuilder rs = new StringBuilder();
		if(this.getDays() > 0)
		{
			rs.append(this.getDays()).append('.');
		}
		rs.append(String.format("%1$02d:%2$02d:%3$02d", this.getHours(), this.getMilliseconds(), this.getSeconds()));
		if(this.getMilliseconds() > 0)
		{
			rs.append('.').append(this.getMilliseconds());
		}
		
		return rs.toString();
	}

	

	public static TimeSpan negation(TimeSpan t)
	{
		if (t._ticks == MinValue._ticks)
		{
			throw new IllegalArgumentException("TimeSpan is too long");
		}
		return new TimeSpan(-t._ticks);
	}

	
	public boolean isLessThan(TimeSpan t2)
	{
		return (this._ticks < t2._ticks);
	}

	public boolean isLessThanOrEqual(TimeSpan t2)
	{
		return (this._ticks <= t2._ticks);
	}

	public boolean isGreaterThan(TimeSpan t2)
	{
		return (this._ticks > t2._ticks);
	}

	public boolean isGreaterThanOrEqual(TimeSpan t2)
	{
		return (this._ticks >= t2._ticks);
	}

	static
	{
		Zero = new TimeSpan(0L);
		MaxValue = new TimeSpan(0x7fffffffffffffffL);
		MinValue = new TimeSpan(-9223372036854775808L);
	}

	public TimeSpan clone()
	{
		TimeSpan rs = new TimeSpan(this._ticks);
		return rs;
	}
	
	public Date add(Date time)
	{
		return new Date(time.getTime() + (long)this.getTotalMilliseconds());
	}
	
	public static Date add(Date time, TimeSpan timespan)
	{
		return timespan.add(time);
	}
	
	public static TimeSpan substract(Date t1, Date t2)
	{
		return TimeSpan.fromMilliseconds(t1.getTime() - t2.getTime());
	}
	
	public static TimeSpan timeOfDay(Date time)
	{
		long rs = time.getTime() % MillisPerDay;
		return TimeSpan.fromMilliseconds(rs);
	}
	
	
	
}


