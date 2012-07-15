package Fantasy.Jobs.Management;

import Fantasy.IO.*;
import Fantasy.ServiceModel.*;

public class StartupFolderWatcher extends AbstractService
{

	private java.util.ArrayList<FileSystemWatcher> _watchers = new java.util.ArrayList<FileSystemWatcher>();
	private IJobQueue _jobQueue;

	private java.util.LinkedList<String> _fileQueue = new java.util.LinkedList<String>();
	private Object _syncRoot = new Object();
	private Thread _addingThread;

	private AutoResetEvent _waitHandler = new AutoResetEvent(false);

	@Override
	public void InitializeService()
	{

		_jobQueue = this.Site.<IJobQueue>GetRequiredService();

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Task.Factory.StartNew(() =>
		{
			String[] folders = JobManagerSettings.getDefault().getStartupFolders().split("[;]", -1);
			String baseDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().getLocation());
			for (String folder : folders)
			{
				String fullPath = Fantasy.IO.LongPath.Combine(baseDir, folder);
				if (LongPathDirectory.Exists(fullPath))
				{
					for (String s : Directory.GetFiles(fullPath, "*.xml", SearchOption.TopDirectoryOnly))
					{
						_fileQueue.offer(s);
					}
					FileSystemWatcher watcher = new FileSystemWatcher(fullPath, "*.xml");
//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
					watcher.Created += new FileSystemEventHandler(FileWatcherCreated);
					watcher.EnableRaisingEvents = true;
					_watchers.add(watcher);
				}
			}
			_addingThread = ThreadFactory.CreateThread(this.AddJobs).WithStart();
		}
	   );
		super.InitializeService();
	}


	private void AddJobs()
	{
		while (true)
		{
			String file = null;

			synchronized (this._fileQueue)
			{
				if (this._fileQueue.size() > 0)
				{

					file = this._fileQueue.poll();
				}
			}

			if (file != null)
			{
				this.StartJob(file);
			}
			else
			{
				this._waitHandler.WaitOne();
			}


		}
	}


	private void FileWatcherCreated(Object sender, FileSystemEventArgs e)
	{
		synchronized (this._fileQueue)
		{
			if (!this._fileQueue.contains(e.FullPath))
			{
				this._fileQueue.offer(e.FullPath);
				this._waitHandler.Set();
			}
		}
	}



	private void StartJob(String path)
	{
		ILogger logger = this.Site.<ILogger>GetService();
		try
		{

			String xml = this.ReadStartInfo(path);
			if (xml != null)
			{

				JobMetaData job = _jobQueue.CreateJobMetaData();
				job.LoadXml(xml);

				_jobQueue.ApplyChange(job);

				File.Delete(path);

				if (logger != null)
				{
					logger.LogMessage(LogCategories.getManager(), "Load job start info at {0}", path);
				}
			}


		}
		catch (ThreadAbortException e)
		{
		}
		catch (RuntimeException error)
		{
			if (logger != null)
			{
				logger.LogError(LogCategories.getManager(), error, "Invalid job start file {0}", path);
			}

			try
			{

				File.Delete(path);
			}
			catch (java.lang.Exception e2)
			{
			}
		}


	}

	private String ReadStartInfo(String path)
	{
		String rs = null;
		while(rs == null && File.Exists(path))
		{
			try
			{

				rs = File.ReadAllText(path);

			}
			catch (IOException e)
			{

			}
			Thread.sleep(50);
		}

		return rs;
	}

	@Override
	public void UninitializeService()
	{

		super.UninitializeService();
		this._addingThread.stop();
		for (FileSystemWatcher w : this._watchers)
		{
			//w.EnableRaisingEvents = false;
			w.dispose();
		}
	}

}