package fantasy;

import java.util.EventObject;

public class PropertyChangedEvent extends EventObject {
    
	/**
	 * 
	 */
	private static final long serialVersionUID = -7564574819720773669L;

	public PropertyChangedEvent(Object source, String propertyName)
	{
		super(source);
		this._propertyName = propertyName;
	}
	
	private String _propertyName;
	
	public String getPropertyName()
	{
		return this._propertyName;
	}
}
