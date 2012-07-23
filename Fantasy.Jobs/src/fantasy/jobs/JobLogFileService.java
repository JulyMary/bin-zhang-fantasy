package fantasy.jobs;

import java.io.*;

import java.nio.file.*;
import java.rmi.RemoteException;
import java.util.*;

import fantasy.servicemodel.*;

public class JobLogFileService extends LogFileService
{


	/**
	 * 
	 */
	private static final long serialVersionUID = -2626826712860834896L;

	public JobLogFileService() throws RemoteException {
		super();
		// TODO Auto-generated constructor stub
	}

	@Override
	public void initializeService() throws Exception
	{
		IJobEngine engine = this.getSite().getService(IJobEngine.class);
		
		String filePath = String.format("%1$s\\%2$s.xlog", engine.getJobDirectory(), engine.getJobId());
		
		
		OutputStream os = Files.newOutputStream(Paths.get(filePath), StandardOpenOption.APPEND, StandardOpenOption.CREATE);
		this._writer = new OutputStreamWriter(os, this.getCharset());
    	
		this.writeStart();

		super.initializeService();
	}

	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		this._writer.close();
	}

	public static String GetLogFilePath(String jobsDirectory, UUID jobId)
	{
		return String.format("%1$s\\%2$s\\%2$s.xlog", jobsDirectory, jobId);
	}

	private OutputStreamWriter _writer;

	@Override
	protected OutputStreamWriter getWriter()
	{
		return _writer;
	}

	
}