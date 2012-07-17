package fantasy.tracking;

import java.util.EventObject;

public class TrackChangedEventObject extends EventObject
{
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 6171519096166110137L;
	public TrackChangedEventObject(Object source, String name, Object oldValue, Object newValue)
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