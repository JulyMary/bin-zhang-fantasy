package Fantasy.Jobs.Resources;

import Fantasy.Configuration.*;

public class Period
{
	private java.util.Date privateStart = new java.util.Date(0);
	public final java.util.Date getStart()
	{
		return privateStart;
	}
	public final void setStart(java.util.Date value)
	{
		privateStart = value;
	}

	private java.util.Date privateEnd = new java.util.Date(0);
	public final java.util.Date getEnd()
	{
		return privateEnd;
	}
	public final void setEnd(java.util.Date value)
	{
		privateEnd = value;
	}

	@Override
	public String toString()
	{
		return String.format("%1$s, %2$s", getStart(), getEnd());
	}
}