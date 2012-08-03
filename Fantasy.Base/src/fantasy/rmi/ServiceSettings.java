package fantasy.rmi;

import java.net.URI;
import java.util.*;

import org.apache.commons.lang3.StringUtils;

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
	
	public URI GetUrl(@SuppressWarnings("rawtypes") Class type) throws Exception
	{
		
		final String name = type.getName();
		
		ServicePoint point = new Enumerable<ServicePoint>(this.getServices()).single(new Predicate<ServicePoint>(){

			@Override
			public boolean evaluate(ServicePoint obj) throws Exception {
				
				return StringUtils.equals(obj.getTypeName(), name);
			}});
		
		
		if(!StringUtils2.isNullOrEmpty(this._baseAddress))
		{
			URI root = new URI(this._baseAddress);
			return root.resolve( point.getAddress());
			
		
		}
		else
		{
			return new URI(point.getAddress());
		}
		
		
	}
}
