﻿package Fantasy.Tracking;

public class TrackChangedEventArgs extends EventArgs
{
	public TrackChangedEventArgs(String name, Object oldValue, Object newValue)
	{
		this.setName(name);
		this.setOldValue(oldValue);
		this.setNewValue(newValue);
	}

	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	private void setName(String value)
	{
		privateName = value;
	}
	private Object privateOldValue;
	public final Object getOldValue()
	{
		return privateOldValue;
	}
	private void setOldValue(Object value)
	{
		privateOldValue = value;
	}
	private Object privateNewValue;
	public final Object getNewValue()
	{
		return privateNewValue;
	}
	private void setNewValue(Object value)
	{
		privateNewValue = value;
	}

}