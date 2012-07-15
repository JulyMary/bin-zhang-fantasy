package Fantasy.Jobs.Management;

import Fantasy.XSerialization.*;
import Fantasy.ServiceModel.*;

public class JobController extends AbstractService implements IJobController, IJobEngineEventHandler
{

	@Override
	public Object InitializeLifetimeService()
	{
		return null;
	}


	private Thread _checkProcessThread;
	@Override
	public void InitializeService()
	{

		super.InitializeService();
		_checkProcessThread = ThreadFactory.CreateThread(this.CheckProcessExist).WithStart();

	}



	private boolean _uninitializing = false;
	@Override
	public void UninitializeService()
	{
		_uninitializing = true;
		super.UninitializeService();
		_checkProcessThread.stop();
		this.SuspendAll(true);

	}

	private java.util.ArrayList<JobProcess> _process = new java.util.ArrayList<JobProcess>();

	private void CheckProcessExist()
	{

		while (true)
		{
			Thread.sleep(30 * 1000);
			synchronized (_syncRoot)
			{
				for (JobProcess process : new java.util.ArrayList<JobProcess>(_process))
				{
					boolean exited = true;
					try
					{
						exited = process.getProcess().HasExited;
					}
					catch (Win32Exception e)
					{

					}
					catch (InvalidOperationException e2)
					{

					}
					if (exited)
					{
						this.SetJobExited(process.getJob().getId(), JobState.Failed);
					}
				}
			}
		}
	}

	public final boolean IsJobProccessExisted(Guid id)
	{
		boolean rs = false;
		JobProcess jp = GetJobProcessById(id);
		if (jp != null)
		{
			try
			{
				rs = ! jp.getProcess().HasExited;
			}
			catch (Win32Exception e)
			{

			}
			catch (InvalidOperationException e2)
			{

			}
		}

		return rs;
	}

	private Object _syncRoot = new Object();

	private void CommitChange(JobMetaData job)
	{
		IJobQueue queue = this.Site.<IJobQueue>GetRequiredService();
		queue.ApplyChange(job);
	}

	private void SetJobExited(Guid id, int exitState)
	{
		JobProcess jp = this.GetJobProcessById(id);
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
					this._process.remove(jp);
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

			Process process = CreateHostProcess(job);

			JobProcess jp = new JobProcess(job, process, true);
			this._process.add(jp);
			process.Start();
			job.setState(JobState.Running);
			this.CommitChange(job);
		}
	}


//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: private static extern bool GetProcessAffinityMask(IntPtr hProcess, out uint lpProcessAffinityMask, out uint lpSystemAffinityMask);
	private static native boolean GetProcessAffinityMask(IntPtr hProcess, RefObject<Integer> lpProcessAffinityMask, RefObject<Integer> lpSystemAffinityMask);
	static
	{
		System.loadLibrary("kernel32.dll");
	}



	private Process CreateHostProcess(JobMetaData job)
	{
		Process process = new Process();

		process.StartInfo.FileName = String.format("%1$s", JobManagerSettings.getDefault().getJobHostFullPath());
		String args = String.format("/id:%1$s", job.getId());
		process.StartInfo.Arguments = args;

//C# TO JAVA CONVERTER WARNING: Unsigned integer types have no direct equivalent in Java:
//ORIGINAL LINE: uint pm, sm;
		int pm, sm = 0;

		RefObject<Integer> tempRef_pm = new RefObject<Integer>(pm);
		RefObject<Integer> tempRef_sm = new RefObject<Integer>(sm);
		GetProcessAffinityMask(Process.GetCurrentProcess().Handle, tempRef_pm, tempRef_sm);
		pm = tempRef_pm.argvalue;
		sm = tempRef_sm.argvalue;

		if (pm != 0)
		{
			args += "/pid:" + (new Integer(pm)).toString();
		}


		process.StartInfo.ErrorDialog = false;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
//#if !DEBUG
			process.StartInfo.CreateNoWindow = true;
//#endif
		process.StartInfo.UseShellExecute = false;
		return process;
	}


	public void StartJob(JobMetaData job)
	{
		synchronized (_syncRoot)
		{
			Process process = CreateHostProcess(job);

			JobProcess jp = new JobProcess(job, process, false);
			this._process.add(jp);
			process.Start();
			job.setStartTime(new java.util.Date());
			job.setState(JobState.Running);
			CommitChange(job);
		}

	}

	public void Cancel(Guid id)
	{
		JobProcess jp = this.GetJobProcessById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Terminate();
		}
	}

	public void Suspend(Guid id)
	{
		JobProcess jp = this.GetJobProcessById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Suspend();
		}

	}

	public void UserPause(Guid id)
	{
		JobProcess jp = this.GetJobProcessById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().UserPause();
		}
	}

	private JobProcess GetJobProcessById(Guid id)
	{
		synchronized (_syncRoot)
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			return (from p in this._process where p.Job.Id == id select p).SingleOrDefault();
		}
	}

	public void RegisterJobEngine(IJobEngine engine)
	{
		JobProcess jp = this.GetJobProcessById(engine.getJobId());
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
				return JobManagerSettings.getDefault().getJobProcessCount() - this._process.size();
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
			return this._process.Select(p => p.Job).toArray();
		}
	}



//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobController Members


	public final void SuspendAll(boolean waitForExit)
	{
		JobProcess[] process;
		synchronized (this._process)
		{
			process = this._process.toArray(new JobProcess[]{});
		}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Parallel.ForEach(this._process.toArray(new JobProcess[]{}), p =>
		{
			try
			{
				if (!p.Process.HasExited)
				{
					p.Engine.Suspend();
					p.ExitEvent.WaitOne();
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