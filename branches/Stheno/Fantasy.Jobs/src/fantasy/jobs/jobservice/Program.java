package fantasy.jobs.jobservice;

import java.io.IOException;

import fantasy.JavaLibraryUtils;
import fantasy.jobs.management.JobManager;

public class Program {
    public static void main(String[] args) throws IOException
    {
    	boolean exit = false;
    	
    	JavaLibraryUtils.setEntryLibrary(JavaLibraryUtils.getLibrary(Program.class));
    	
    	Program prog = new Program();
    	try {
    		
			prog.onStart();
		} catch (Exception e) {
		    exit = true;
		}
    	
    	if(!exit)
    	{
    	//System.in.read();
    	
    	try {
			prog.onStop();
		} catch (Exception e) {
			e.printStackTrace();
			exit = true;
		}
    	}
    	
    	Runtime.getRuntime().exit(0);
    	
    	
    	
    }
    
    
    private void onStart() throws Exception
    {
    	JobManager.getDefault().Start(null);
    }
    
    private void onStop() throws Exception
    {
    	JobManager.getDefault().Stop();
    }
}
