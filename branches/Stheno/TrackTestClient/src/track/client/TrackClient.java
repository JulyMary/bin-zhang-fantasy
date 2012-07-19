package track.client;

import java.io.*;
import java.util.EventObject;
import java.util.UUID;
import java.util.concurrent.Semaphore;


import fantasy.tracking.*;


public class TrackClient {
	
	
	
	public static void main(String[] args) throws Exception
	{
		ITrackManager manager = TrackManagerFactory.createTrackManager(uri);
		
		ITrackListener listener = manager.getListener(id);
		
		
		OutputStreamWriter output =  new OutputStreamWriter(System.out);
		final BufferedWriter writer = new BufferedWriter(output);
		listener.addHandler(new ITrackListenerHandler(){

			@Override
			public void HandleChanged(TrackChangedEventObject e) {
				try {
					writer.newLine();
					writer.write((String)e.getNewValue());
				} catch (IOException e1) {
					
					e1.printStackTrace();
				}
				
				
			}

			@Override
			public void HandleActiveChanged(EventObject e) {
				try {
					writer.newLine();
					writer.write("active");
				} catch (IOException e1) {
				
					e1.printStackTrace();
				}
				
				
			}});
		
		Thread.sleep(Long.MAX_VALUE);
		
	}
	
private static final String uri =  "rmi://localhost:9000/ITrackManagerService";
    
    
    private static final UUID id = UUID.fromString("4F6E3DED-2337-45E7-A32C-5646C6F54B84");
}
