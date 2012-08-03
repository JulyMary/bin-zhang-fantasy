package fantasy.rmi;

import java.net.URI;
import java.rmi.Naming;
import java.rmi.Remote;
import java.rmi.registry.LocateRegistry;
import java.util.*;

@SuppressWarnings("rawtypes")
public final class RmiUtils {
    
	private RmiUtils()
	{
		
	}
	
	private static HashSet<Integer> _registeredPorts = new HashSet<Integer>();
	
	public static void bind(Remote service) throws Exception
	{
		
		Class type = service.getClass();
		
		URI url = RmiSettings.getDefault().getServices().GetUrl(type);
		if(!_registeredPorts.contains(url.getPort()))
		{
			LocateRegistry.createRegistry(url.getPort()); 
			_registeredPorts.add(url.getPort());
		}
		
		Naming.rebind(url.toString(), service);
		
	}
	
	public static void unbind(Remote service) throws Exception
	{
        Class type = service.getClass();
		
        URI url = RmiSettings.getDefault().getServices().GetUrl(type);
		Naming.unbind(url.toString());
	}
	
}
