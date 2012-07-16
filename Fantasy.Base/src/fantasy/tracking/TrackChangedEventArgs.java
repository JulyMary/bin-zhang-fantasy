package fantasy.tracking;

import java.io.*;
import java.util.EventObject;

public class TrackChangedEventArgs extends EventObject implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -8672071739911225336L;
	public TrackChangedEventArgs(Object source, String name, Object oldValue, Object newValue)
	{
		super(source);
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