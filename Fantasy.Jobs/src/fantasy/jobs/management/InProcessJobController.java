package fantasy.jobs.management;

import java.rmi.RemoteException;
import java.util.*;
import java.util.concurrent.*;

import fantasy.JDomUtils;
import fantasy.jobs.*;
import fantasy.servicemodel.*;
import fantasy.xserialization.*;
import org.jdom2.*;

public class InProcessJobController extends AbstractService implements IJobController, IJobEngineEventHandler
{
	/**
	 * 
	 */
	private static final long serialVersionUID = -16713493158777951L;



	public InProcessJobController() throws RemoteException
	{
	      super();
	}

	ScheduledExecutorService _scheduler;

	

	@Override
	public void initializeService() throws Exception
	{
		
		_scheduler = Executors.newScheduledThreadPool(1);
		_scheduler.scheduleAtFixedRate(new Runnable(){

			@Override
			public void run() {
				try {
					InProcessJobController.this.CheckThreadExist();
				}
				
				catch (Exception e) {
					
					
				}
			}}, 30, 30, TimeUnit.SECONDS);
		_scheduler.shutdown();
		
		super.initializeService();
	}

	private void CheckThreadExist() throws Exception
	{

		synchronized (_syncRoot)
		{
			for (JobThread jt : new java.util.ArrayList<JobThread>(this._threads))
			{
				boolean exited = true;
				exited = !jt.getThread().isAlive();
				if (exited)
				{
					this.SetJobExited(jt.getJob().getId(), JobState.Failed);
				}
			}
		}
		
	}

	private boolean _uninitializing = false;
	@Override
	public void uninitializeService() throws Exception
	{

		_uninitializing = true;
		_scheduler.shutdownNow();
		super.uninitializeService();
		this.SuspendAll(true);

	}

	private java.util.ArrayList<JobThread> _threads = new java.util.ArrayList<JobThread>();





	public final boolean IsJobProccessExisted(UUID id)
	{
		boolean rs = false;
		JobThread jp = GetJobThreadById(id);
		if (jp != null)
		{
			 rs = jp.getThread().isAlive();
		}

		return rs;
	}

	private Object _syncRoot = new Object();

	

	private void SetJobExited(UUID id, int exitState) throws Exception
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
					
					this.getSite().getRequiredService(IJobQueue.class).archive(jp.getJob());
					
				}
				finally
				{
					synchronized(jp.getExitEvent())
					{
					    jp.getExitEvent().notifyAll();
					}
				}
			}
		}
	}

	public void Resume(JobMetaData job) throws Exception
	{
		synchronized (_syncRoot)
		{

			Thread thread = CreateHostThread(job);

			JobThread jp = new JobThread(job, thread, true);
			this._threads.add(jp);
			thread.start();
			job.setState(JobState.Running);
			this.getSite().getRequiredService(IJobQueue.class).updateState(job, false);
		}
	}


	

	private Thread CreateHostThread(final JobMetaData job)
	{
		Thread rs = fantasy.ThreadFactory.create(new Runnable(){

			@Override
			public void run() {
				InProcessJobEngineHost jobHost = new InProcessJobEngineHost();
				try {
					jobHost.Run(JobManager.getDefault(), job.getId());
				} catch (Exception e) {
					
					e.printStackTrace();
				}
				
			}});

		return rs;
	}


	public void StartJob(JobMetaData job) throws Exception
	{
		synchronized (_syncRoot)
		{
			Thread thread = CreateHostThread(job);

			JobThread jp = new JobThread(job, thread, false);
			this._threads.add(jp);
			thread.start();
			job.setStartTime(new java.util.Date());
			job.setState(JobState.Running);
			this.getSite().getRequiredService(IJobQueue.class).updateState(job, true);
		}

	}

	public void Cancel(UUID id) throws Exception
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Terminate();
		}
	}

	public void Suspend(UUID id) throws Exception
	{
		JobThread jp = this.GetJobThreadById(id);
		if (jp != null && jp.getEngine() != null)
		{
			jp.getEngine().Suspend();
		}

	}

	public void UserPause(UUID id) throws Exception
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
			for(JobThread jt : this._threads)
			{
				if(jt.getJob().getId().equals(id))
				{
					return jt;
				}
			}
		}
		
		return null;
	}

	public void RegisterJobEngine(IJobEngine engine) throws Exception
	{
		JobThread jp = this.GetJobThreadById(engine.getJobId());
		if (jp != null)
		{
			jp.setEngine(engine);
			engine.AddHandler(this);
			Element doc = JDomUtils.parseElement(jp.getJob().getStartInfo());

			XSerializer ser = new XSerializer(JobStartInfo.class);
			JobStartInfo si = (JobStartInfo)ser.deserialize(doc);
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

	public void HandleStart(IJobEngine sender)
	{

	}

	public void HandleResume(IJobEngine sender)
	{

	}

	public void HandleExit(IJobEngine sender, JobExitEventArgs e) throws Exception
	{
		this.SetJobExited(sender.getJobId(), e.getExitState());
	}

	public void HandleLoad(IJobEngine sender)
	{

	}

	public int GetAvailableProcessCount() throws Exception
	{
		if (!_uninitializing)
		{
			synchronized (_syncRoot)
			{
				return JobManagerSettings.getDefault().getMaxDegreeOfParallelism() - this._threads.size();
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
			JobMetaData[] rs = new JobMetaData[this._threads.size()];
			
			for(int i = 0; i < rs.length; i ++)
			{
				rs[i] = this._threads.get(i).getJob();
						
			}
			
			return rs;
		}
	}


	public final void SuspendAll(boolean waitForExit) throws Exception
	{
		JobThread[] process;
		synchronized (this._threads)
		{
			process = this._threads.toArray(new JobThread[]{});
		}
		
		ExecutorService exec =  Executors.newCachedThreadPool();
		for(final JobThread jt : process)
		{
			exec.execute(new Runnable(){

				@Override
				public void run() {
					try
					{
						if (jt.getThread().isAlive())
						{
							jt.getEngine().Suspend();
							jt.getEngine().wait(10*1000);
						}
					}
					catch (Exception e)
					{
					}
					
				}});
		}
		
		exec.shutdown();
		exec.awaitTermination(Long.MAX_VALUE, TimeUnit.NANOSECONDS);
		
		

	}

}