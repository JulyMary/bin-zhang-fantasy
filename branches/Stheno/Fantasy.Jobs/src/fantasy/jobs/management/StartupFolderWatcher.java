package fantasy.jobs.management;

import fantasy.*;
import fantasy.io.*;
import fantasy.jobs.*;
import fantasy.jobs.properties.Resources;
import fantasy.servicemodel.*;

import java.nio.file.*;

import java.rmi.RemoteException;
import java.util.*;
import java.io.*;

import org.apache.commons.lang3.StringUtils;

public class StartupFolderWatcher extends AbstractService
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -4723449494856707652L;


	public StartupFolderWatcher() throws RemoteException {
		super();
		
	}

	private HashMap<WatchKey, java.nio.file.Path> _watchers = new HashMap<WatchKey, java.nio.file.Path>();
	private IJobQueue _jobQueue;

	private Thread _watchThread;
	
	private WatchService _watcher; 


	@Override
	public void initializeService() throws Exception
	{

		_jobQueue = this.getSite().getRequiredService(IJobQueue.class);

		this._watchThread = ThreadFactory.createAndStart(new Runnable(){

			@Override
			public void run() {
				
				try {
					StartupFolderWatcher.this.startWatch();
				} catch (Exception e) {
					
					e.printStackTrace();
				}
			}});
		
		super.initializeService();
	}
	
	
	private void startWatch() throws Exception
	{
		this._watcher = FileSystems.getDefault().newWatchService();
		String[] folders = StringUtils2.split(JobManagerSettings.getDefault().getStartupFolders(), ";", true);
		for(String folder : folders)
		{
			String fullPath = JavaLibraryUtils.extractToFullPath(folder);
			if(Directory.exists(fullPath))
			{
				boolean found = false;
				do
				{
					found = false;
					for(String s : Directory.enumerateFiles(fullPath, "*.xml", false))
					{
						found = true;
						this.StartJob(s);
					}
				}
				while(found);
				
				
				this.register(fullPath);
			}
			
		}
		
		while(true)
		{
			WatchKey key;
			try
			{
				key = this._watcher.take();
			}
			catch(InterruptedException error)
			{
				return;
			}

			try
			{
				for(WatchEvent<?> event : key.pollEvents())
				{
					WatchEvent.Kind<?> kind = event.kind();
					if(kind == StandardWatchEventKinds.ENTRY_CREATE || kind == StandardWatchEventKinds.ENTRY_MODIFY)
					{
						@SuppressWarnings("unchecked")
						WatchEvent<java.nio.file.Path> ev = (WatchEvent<java.nio.file.Path>)event;
						java.nio.file.Path filename = ev.context();

						String fullPath = this._watchers.get(key).resolve(filename).toAbsolutePath().toString();
						if(StringUtils.equalsIgnoreCase(fantasy.io.Path.getExtension(fullPath), ".xml"))
						{
						    this.StartJob(fullPath);
						}
					}

				}
			}
			finally
			{
				key.reset();
			}


		}
	}

	
	private void register(String path) throws IOException 
	{         
		java.nio.file.Path dir = java.nio.file.Paths.get(path);
		WatchKey key = dir.register(this._watcher, StandardWatchEventKinds.ENTRY_CREATE, StandardWatchEventKinds.ENTRY_MODIFY);  
		this._watchers.put(key, dir);
	}
	
	


	



	private void StartJob(String path) throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);
		try
		{

			String xml = this.ReadStartInfo(path);
			if (xml != null)
			{

				JobMetaData job = _jobQueue.CreateJobMetaData();
				job.LoadXml(xml);

				_jobQueue.Add(job);

				fantasy.io.File.delete(path);

				if (logger != null)
				{
					logger.LogMessage(LogCategories.getManager(), Resources.getLoadJobStartInfoFileMessage(), path);
				}
			}


		}
		catch (InterruptedException e)
		{
		}
		catch (Exception error)
		{
			if (logger != null)
			{
				logger.LogError(LogCategories.getManager(), error, "Invalid job start file {0}", path);
			}

			try
			{

				fantasy.io.File.delete(path);
			}
			catch (java.lang.Exception e2)
			{
			}
		}


	}

	private String ReadStartInfo(String path) throws Exception
	{
		String rs = null;
		while(rs == null && fantasy.io.File.exists(path))
		{
			try
			{

				rs = fantasy.io.File.readAllText(path);

			}
			catch (IOException e)
			{

			}
			Thread.sleep(50);
		}

		return rs;
	}

	@Override
	public void uninitializeService() throws Exception
	{

		super.uninitializeService();
		this._watchThread.interrupt();
	}

}