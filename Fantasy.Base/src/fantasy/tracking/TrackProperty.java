package fantasy.tracking;

import java.io.Serializable;

public class TrackProperty implements Serializable
{
    /**
	 * 
	 */
	private static final long serialVersionUID = -3175591714300737523L;
	
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}


	private Object privateValue;
	public final Object getValue()
	{
		return privateValue;
	}
	public final void setValue(Object value)
	{
		privateValue = value;
	}


	public static TrackProperty Create(String name, Object value)
	{
		TrackProperty rs = new TrackProperty();
		rs.setName(name);
		rs.setValue(value);
		
		return rs;

	}

	
}