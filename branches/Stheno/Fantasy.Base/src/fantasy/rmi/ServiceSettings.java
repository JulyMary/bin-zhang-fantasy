package fantasy.rmi;

import java.net.URL;
import java.util.*;

import fantasy.*;
import fantasy.collections.*;
import fantasy.xserialization.*;


@XSerializable(name="services", namespaceUri=Consts.SettingsNameSpace)
public class ServiceSettings {

	@XArray(items=@XArrayItem(name="service", type=ServicePoint.class))
	private ArrayList<ServicePoint> _services = new ArrayList<ServicePoint>();
	
	public List<ServicePoint> getServices()
	{
		return _services;
	}
	
	@XAttribute(name = "baseAddress")
	private String _baseAddress;
	public String getBaseAddress()
	{
		return this._baseAddress;
	}
	
	public URL GetUrl(@SuppressWarnings("rawtypes") final Class type) throws Exception
	{
		ServicePoint point = new Enumerable<ServicePoint>(this._services).single(new Predicate<ServicePoint>(){

			@Override
			public boolean evaluate(ServicePoint obj) throws Exception {
				
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
