package fantasy.rmi;

import fantasy.Consts;
import fantasy.xserialization.*;

@XSerializable(name="service", namespaceUri=Consts.SettingsNameSpace)
public class ServicePoint {
	
	@XAttribute(name="type")
    public String _typeName;
	
	public String getTypeName()
	{
		return this._typeName;
	}
	
	@XAttribute(name="address")
	private String _address;
	public String getAddress()
	{
		return _address;
	}
	
}
