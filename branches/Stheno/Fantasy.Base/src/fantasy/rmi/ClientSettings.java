package fantasy.rmi;

import java.net.URL;
import java.util.*;


import fantasy.*;
import fantasy.collections.Enumerable;
import fantasy.collections.Predicate;
import fantasy.xserialization.*;

@XSerializable(name="client", namespaceUri=Consts.SettingsNameSpace)
public class ClientSettings {
	@XArray(items=@XArrayItem(name="endpoint", type=EndPoint.class))
	private ArrayList<EndPoint> _endPoints = new ArrayList<EndPoint>();
	
	public List<EndPoint> getEndPoints()
	{
		return _endPoints;
	}
	
	@XAttribute(name = "baseAddress")
	private String _baseAddress;
	public String getBaseAddress()
	{
		return this._baseAddress;
	}
	
	public URL GetUrl(@SuppressWarnings("rawtypes") final Class type) throws Exception
	{
		EndPoint point = new Enumerable<EndPoint>(this._endPoints).single(new Predicate<EndPoint>(){

			@Override
			public boolean evaluate(EndPoint obj) throws Exception {
				
				return obj.getTypeName() == type.getName();
			}});
		
		
		if(!StringUtils2.isNullOrEmpty(this._baseAddress))
		{
			return new URL(new URL(this._baseAddress), point.getAddress());
		}
		else
		{
			return new URL(point.getAddress());
		}
		
		
	}
	
}
