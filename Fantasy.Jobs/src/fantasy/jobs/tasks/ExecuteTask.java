package fantasy.jobs.tasks;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

import java.util.concurrent.TimeoutException;


import fantasy.servicemodel.*;
import fantasy.*;
import fantasy.jobs.*;
import fantasy.jobs.Consts;

@Task(name = "execute", namespaceUri = Consts.XNamespaceURI, description="Execute an external process with command line")
public class ExecuteTask extends ObjectWithSite implements ITask
{



	public final void Execute() throws Exception
	{
		
		
		ProcessBuilder builder = new ProcessBuilder();
		builder.command(this.Command);
		if(!StringUtils2.isNullOrEmpty(this.WorkingDirectory))
		{
			builder.directory(new java.io.File(this.WorkingDirectory));
		}
		
		if(this.WaitForExit)
		{
			builder.redirectErrorStream(true);
		}
		
		Process process = builder.start();
		if(this.WaitForExit)
		{
			WaitProcess(process);
		}
		
	}


	private void WriteOutput(InputStream stream)
	{
		try
		{
			ILogger logger = this.getSite().getService(ILogger.class);
			InputStreamReader input   =   new   InputStreamReader(stream); 
	    	BufferedReader  reader   =  new BufferedReader(input); 
			
			String text;

			do
			{
				text = reader.readLine();
				if (text != null)
				{
					
					Log.SafeLogMessage(logger, "Execute", text);
				}
			} while (text != null);
		}
		catch (java.lang.Exception e)
		{
		}

	}
	
	private Object _waitHandler = new Object();
	private boolean _exited = false;
	private Thread _waitForThread;
	private Thread _outputThread;
	private Thread _errorThread;

	private void WaitProcess(final Process process) throws Exception
	{


		this._outputThread = ThreadFactory.createAndStart(new Runnable(){

			@Override
			public void run() {
				WriteOutput(process.getInputStream());
			}});
		this._errorThread = ThreadFactory.createAndStart(new Runnable(){

			@Override
			public void run() {
				WriteOutput(process.getErrorStream());
				
			}});
		
		this._waitForThread = ThreadFactory.createAndStart(new Runnable(){

			@Override
			public void run() {
				try {
					ExecuteTask.this.ExitCode =  process.waitFor();
					ExecuteTask.this._exited = true;
					ExecuteTask.this._waitHandler.notifyAll();
					
				} catch (InterruptedException e) {
					
				}
				
			}});
		
		
		
		this._waitHandler.wait((long)ExecuteTask.this.Timeout.getTotalMilliseconds());
		
		if(!_exited)
		{
			if(this._waitForThread.isAlive())
			{
				this._waitForThread.interrupt();
			}
			if(this._outputThread.isAlive())
			{
				this._outputThread.interrupt();
			}
			if(this._errorThread.isAlive())
			{
				this._errorThread.interrupt();
			}
			
			throw new TimeoutException("Executing time out");
		}

	}


	@TaskMember(name = "command", description="The command-line to use when starting the application.")
	public String Command = "";
	

	@TaskMember(name = "exitCode", flags=TaskMemberFlags.Output, description="The code that the associated process specified when it terminated.")
	public int ExitCode;
	


	@TaskMember(name = "waitForExit", description="true if the task to wait for application to exit; otherwise false.")
	public boolean WaitForExit = false;
	 

	@TaskMember(name = "workingDirectory", description="The initial directory for the process to be started.")
	public String WorkingDirectory = "";
	
	@TaskMember(name = "timeout", description="The amount of time to wai for application to exit.")
	public TimeSpan Timeout = TimeSpan.MaxValue;
	



}