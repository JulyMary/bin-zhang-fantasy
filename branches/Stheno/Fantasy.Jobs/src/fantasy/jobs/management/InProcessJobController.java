package fantasy.jobs.management;

import Fantasy.ServiceModel.*;
import Fantasy.XSerialization.*;

public class InProcessJobController extends AbstractService implements IJobController, IJobEngineEventHandler
{
	public InProcessJobController()
	{
	   _appDomainSetup = new AppDomainSetup();
			_appDomainSetup.ApplicationBase = System.Environment.CurrentDirectory;
			_appDomainSetup.DisallowBindingRedirects = false;
			_appDomainSetup.DisallowCodeDownload = true;
			_appDomainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
	}

	private Thread _checkProcessThread;

	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}

	@Override
	public void InitializeService()
	{
		_checkProcessThread = ThreadFactory.CreateThread(this.CheckThreadExist).WithStart();
		super.InitializeService();
	}

	private void CheckThreadExist()
	{

		while (true)
		{
			Thread.sleep(30 * 1000);
			synchronized (_syncRoot)
			{
				for (JobThread jt : new java.util.ArrayList<JobThread>(this._threads))
				{
					boolean exited = true;
					exited = (jt.getThread().ThreadState & (ThreadState.Aborted | ThreadState.Stopped)) > 0;
					if (exited)
					{
						this.SetJobExited(jt.getJob().getId(), JobState.Failed);
					}
				}
			}
		}
	}

	private boolean _uninitializing = false;
	@Override
	public void UninitializeService()
	{

		_uninitializing = true;
		this._checkProcessThread.stop();
		super.UninitializeService();
		this.SuspendAll(true);

	}

	private java.util.ArrayList<JobThread> _threads = new java.util.ArrayList<JobThread>();





	public final boolean IsJobProccessExisted(UUID id)
	{
		boolean rs = false;
		JobThread jp = GetJobThreadById(id);
		if (jp != null)
		{
			 rs = (jp.getThread().ThreadState & (ThreadState.Stopped | ThreadState.Aborted)) == 0;
		}

		return rs;
	}

	private Object _syncRoot = new Object();

	private void CommitChange(JobMetaData job)
	{
		IJobQueue queue = this.getSite().<IJobQueue>GetRequiredService();
		queue.ApplyChange(job);
	}

	private void SetJobExited(UUID id, int exitState)
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null)
		{
			synchronized (_syncRoot)
			{
				try
				{
					jp.getJob().setState(exitState);
					if (jp.getJob().getIsTerminated())
					{
						jp.getJob().setEndTime(new java.util.Date());
					}
					this._threads.remove(jp);
					this.CommitChange(jp.getJob());
				}
				finally
				{
					jp.getExitEvent().Set();
				}
			}
		}
	}

	public void Resume(JobMetaData job)
	{
		synchronized (_syncRoot)
		{

			Thread thread = CreateHostThread(job);

			JobThread jp = new JobThread(job, thread, true);
			this._threads.add(jp);
			thread.start();
			job.setState(JobState.Running);
			this.CommitChange(job);
		}
	}


	private AppDomainSetup _appDomainSetup;

	private Thread CreateHostThread(JobMetaData job)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Thread rs = ThreadFactory.CreateThread(() =>
		{
			AppDomain dm = AppDomain.CreateDomain("AppDomain_" + job.getId().toString(), null, _appDomainSetup);
			try
			{
				InProcessJobEngineHost jobHost = (InProcessJobEngineHost)dm.CreateInstanceAndUnwrap(InProcessJobEngineHost.class.getPackage().FullName, InProcessJobEngineHost.class.FullName);
				jobHost.Run(JobManager.getDefault(), job.getId());
			}
			finally
			{
				AppDomain.Unload(dm);
			}
		}
	   );

		return rs;
	}


	public void StartJob(JobMetaData job)
	{
		synchronized (_syncRoot)
		{
			Thread thread = CreateHostThread(job);

			JobThread jp = new JobThread(job, thread, false);
			this._threads.add(jp);
			thread.start();
			job.setStartTime(new java.util.Date());
			job.setState(JobState.Running);
			CommitChange(job);
		}

	}

	public void Cancel(UUID id)
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Terminate();
		}
	}

	public void Suspend(UUID id)
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Suspend();
		}

	}

	public void UserPause(UUID id)
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().UserPause();
		}
	}

	private JobThread GetJobThreadById(UUID id)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			return (from p in this._threads where p.Job.Id == id select p).SingleOrDefault();
		}
	}

	public void RegisterJobEngine(IJobEngine engine)
	{
		JobThread jp = this.GetJobThreadById(engine.getJobId());
		if (jp != null)
		{
			jp.setEngine(engine);
			engine.AddHandler(this);
			XElement doc = XElement.Parse(jp.getJob().getStartInfo());

			XSerializer ser = new XSerializer(JobStartInfo.class);
			JobStartInfo si = (JobStartInfo)ser.Deserialize(doc);
			if (!jp.getIsResume())
			{
				engine.Start(si);
			}
			else
			{
				engine.Resume(si);
			}

		}
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobEngineEventHandler Members

	private void HandleStart(IJobEngine sender)
	{

	}

	private void HandleResume(IJobEngine sender)
	{

	}

	private void HandleExit(IJobEngine sender, JobExitEventArgs e)
	{
		this.SetJobExited(sender.getJobId(), e.getExitState());
	}

	private void HandleLoad(IJobEngine sender)
	{

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	public int GetAvailableProcessCount()
	{
		if (!_uninitializing)
		{
			synchronized (_syncRoot)
			{
				return JobManagerSettings.getDefault().getJobProcessCount() - this._threads.size();
			}
		}
		else
		{
			return 0;
		}
	}






	public final JobMetaData[] GetRunningJobs()
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			return this._threads.Select(p => p.Job).toArray();
		}
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobController Members


	public final void SuspendAll(boolean waitForExit)
	{
		JobThread[] process;
		synchronized (this._threads)
		{
			process = this._threads.toArray(new JobThread[]{});
		}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Parallel.ForEach(this._threads.toArray(new JobThread[]{}), p =>
		{
			try
			{
				if (p.Thread.IsAlive)
				{
					p.Engine.Suspend();
					p.ExitEvent.WaitOne(10*1000);
				}
			}
			catch (java.lang.Exception e)
			{
			}
		}
	   );
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}