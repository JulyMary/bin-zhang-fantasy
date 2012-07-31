package fantasy.rmi;

import fantasy.Consts;
import fantasy.xserialization.XAttribute;
import fantasy.xserialization.XSerializable;


@XSerializable(name="endPoint", namespaceUri=Consts.SettingsNameSpace)
public class EndPoint {

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
