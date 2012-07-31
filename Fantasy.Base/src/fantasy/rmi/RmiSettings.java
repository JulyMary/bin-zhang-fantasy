package fantasy.rmi;

import fantasy.Consts;

import fantasy.configuration.*;
import fantasy.xserialization.*;


@XSerializable(name="rmi", namespaceUri=Consts.SettingsNameSpace)
public class RmiSettings extends SettingsBase {

	
private static RmiSettings _default;

	
	
	public static RmiSettings getDefault() throws Exception
	{
		if(_default == null)
		{
			_default = (RmiSettings)fantasy.configuration.SettingStorage.load(new RmiSettings());
		}
		
		return _default;
	}
	
	
	@XElement(name="services")
	private ServiceSettings _services = new ServiceSettings();
	
	public ServiceSettings getServices()
	{
		return _services;
	}
	
	@XElement(name="client")
	private ClientSettings _clients = new ClientSettings();
	
	public ClientSettings getClient()
	{
		return _clients;
	}
	
	
}
