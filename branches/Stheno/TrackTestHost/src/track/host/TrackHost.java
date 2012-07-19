package track.host;

import java.io.*;
import java.rmi.registry.*;
import java.util.*;
import fantasy.*;

import fantasy.tracking.*;

public class TrackHost {
    public static void main(String[] args) throws Exception
    {
    	initTrackService();
    	
    	
    	ITrackManager manager = TrackManagerFactory.createTrackManager(uri);
    	
    	HashMap<String, Object> init = new HashMap<String, Object>();
        
    	init.put("message","start");
    	
    	ITrackProvider provider = manager.getProvider(id, "test", "testCategory", init);
    	
    	String message;
    	
    	InputStreamReader input   =   new   InputStreamReader(System.in); 
    	BufferedReader  reader   =  new BufferedReader(input); 
    	
    	do
    	{
    		message = reader.readLine();
    		if(!StringUtils2.isNullOrEmpty(message))
    		{
    			provider.setItem("message", message);
    		}
    	}while(!StringUtils2.isNullOrEmpty(message));
    	
    	TrackingConfiguration.CloseTrackingService();
    }
    
    private static void initTrackService() throws Exception
    {
//    	if (System.getSecurityManager() == null) {
//		    System.setSecurityManager(new RMISecurityManager());
//		}
    	
    	LocateRegistry.createRegistry(9000); 
    	
    	TrackingConfiguration.StartTrackingService(uri);

    }
    
    private static final String uri =  "rmi://localhost:9000/ITrackManagerService";
    
    
    private static final UUID id = UUID.fromString("4F6E3DED-2337-45E7-A32C-5646C6F54B84");
}
