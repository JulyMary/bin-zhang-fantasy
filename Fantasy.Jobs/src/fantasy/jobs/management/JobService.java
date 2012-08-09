package fantasy.jobs.management;

import fantasy.rmi.RmiUtils;
import fantasy.servicemodel.AbstractService;
import fantasy.io.*;
import fantasy.jobs.*;
import fantasy.configuration.*;

import java.lang.reflect.*;
import java.rmi.RemoteException;
import java.text.SimpleDateFormat;
import java.util.*;
import fantasy.*;
import fantasy.collections.*;

import org.apache.commons.io.FileUtils;
import org.jdom2.*;

public class JobService extends AbstractService implements IJobService, IJobQueueListener
{

	/**
	 * 
	 */
	private static final long serialVersionUID = -2913864011750791636L;



	public JobService() throws RemoteException {
		super();
		
	}



	private IJobQueue getQueue() throws Exception
	{
		return JobManager.getDefault().getRequiredService(IJobQueue.class);
	}



	public final JobMetaData StartJob(String startInfo) throws Exception
	{

		JobMetaData job = getQueue().CreateJobMetaData();
		job.LoadXml(startInfo);
		getQueue().Add(job);
		return job;
	}

	public final void Resume(UUID id) throws Exception
	{
		getQueue().Resume(id);
	}

	public final void Cancel(UUID id) throws Exception
	{
		getQueue().Cancel(id);
	}

	public final void Pause(UUID id) throws Exception
	{
		getQueue().UserPause(id);
	}

	private JobMetaData[] GetJobMetaDataByIds(UUID[] ids) throws Exception
	{
		if (ids != null)
		{
			java.util.ArrayList<JobMetaData> rs = new java.util.ArrayList<JobMetaData>();
			for (UUID id : ids)
			{
				JobMetaData job = getQueue().FindJobMetaDataById(id);
				if (job != null)
				{
					rs.add(job);
				}
			}
			return rs.toArray(new JobMetaData[]{});
		}
		else
		{
			return new JobMetaData[0];
		}
	}

	private void Resume(JobMetaData[] jobs)
	{
		for (JobMetaData job : jobs)
		{
			try
			{
				if (job.getState() == JobState.UserPaused)
				{
					getQueue().Resume(job.getId());
				}
			}
			catch (java.lang.Exception e)
			{
			}
		}
	}



	public final void Resume(UUID[] ids) throws Exception
	{
		JobMetaData[] jobs = GetJobMetaDataByIds(ids);
		Resume(jobs);
	}

	public final void ResumeByFilter(String filter) throws Exception
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, null, 0, Integer.MAX_VALUE).toArray(new JobMetaData[0]);
		total = tempRef_total.argvalue;
		this.Resume(jobs);
	}


	private void Cancel(JobMetaData[] jobs) throws Exception
	{
		
		Enumerable<JobMetaData> query = new Enumerable<JobMetaData>(jobs).where(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				return !job.getIsTerminated(); 
			}})
			.orderBy(new Selector<JobMetaData, Boolean>(){

				@Override
				public Boolean select(JobMetaData item) {
					return item.getState() != JobState.Running;
				}});
		
		for (JobMetaData job : query)
		{
			try
			{
				getQueue().Cancel(job.getId());
			}
			catch (Exception e)
			{
			}
		}
	}

	public final void Cancel(UUID[] ids) throws Exception
	{
		JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
		this.Cancel(jobs);
	}

	public final void CancelByFilter(String filter) throws Exception
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, null, 0, Integer.MAX_VALUE).toArray(new JobMetaData[0]);
		total = tempRef_total.argvalue;
		Cancel(jobs);

	}

	private void Pause(JobMetaData[] jobs) throws Exception
	{

		Enumerable<JobMetaData> query = new Enumerable<JobMetaData>(jobs).where(new Predicate<JobMetaData>(){

			@Override
			public boolean evaluate(JobMetaData job) throws Exception {
				return job.getState() != JobState.UserPaused; 
			}})
			.orderBy(new Selector<JobMetaData, Boolean>(){

				@Override
				public Boolean select(JobMetaData item) {
					return item.getState() != JobState.Running;
				}});
		
	
		for (JobMetaData job : query)
		{
			try
			{
				getQueue().UserPause(job.getId());
			}
			catch (java.lang.Exception e)
			{
			}
		}
	}

	public final void Pause(UUID[] ids) throws Exception
	{
		JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
		this.Pause(jobs);
	}




	public final void PauseByFilter(String filter) throws Exception
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, null, 0, Integer.MAX_VALUE).toArray(new JobMetaData[0]);
		total = tempRef_total.argvalue;
		Pause(jobs);
	}


	public final JobMetaData FindJobById(UUID id) throws Exception
	{
		return getQueue().FindJobMetaDataById(id);
	}

	

	public final JobMetaData[] FindUnterminatedJob(RefObject<Integer> totalCount, String filter,  String order, int skip, int take) throws Exception
	{
		return getQueue().FindUnterminated(totalCount, filter,  order, skip, take).toArray(new JobMetaData[0]);
	}

	public final JobMetaData[] FindTerminatedJob(RefObject<Integer> totalCount, String filter, String order, int skip, int take) throws Exception
	{

		return getQueue().FindTerminated(totalCount, filter, order, skip, take).toArray(new JobMetaData[0]);

	}

	public final String GetJobLog(UUID id) throws Exception
	{
		String path = String.format("%1$s\\%2$s\\%2$s.xlog", JobManagerSettings.getDefault().getJobDirectoryFullPath(), id);
		String rs = null;
		try
		{
			rs = File.readAllText(path, "UTF-8");
		}
		catch (Exception e)
		{

		}

		return rs;
	}

	public final String GetJobScript(UUID id) throws Exception
	{
		String rs = null;
		String path = String.format("%1$s\\%2$s\\%2$s.job", JobManagerSettings.getDefault().getJobDirectoryFullPath(), id);
		if (!File.exists(path))
		{
			path += ".bak";
		}
		if (File.exists(path))
		{
			try
			{
				rs = File.readAllText(path, "UTF-8");
			}
			catch (Exception e)
			{

			}
		}

		return rs;
	}

	public final String GetManagerLog(java.util.Date date) throws Exception
	{
		Element rs = new Element("xlog");
		String path = JobManagerSettings.getDefault().getLogDirectoryFullPath();
		
		SimpleDateFormat fmt = new SimpleDateFormat("yyyy-MM-dd");
		
		path = Path.combine(path,fmt.format(date) + ".xlog");
		if (File.exists(path))
		{
			
			for(String line : FileUtils.readLines(new java.io.File(path), "UTF-8"))
			{
				Element node = JDomUtils.parseElement(line);
				rs.addContent(node);
			}
			

		}

		return JDomUtils.toString(rs);
	}

	public final java.util.Date[] GetManagerLogAvaiableDates() throws Exception
	{
		java.util.ArrayList<java.util.Date> rs = new java.util.ArrayList<java.util.Date>();
		String path = JobManagerSettings.getDefault().getLogDirectoryFullPath();

		SimpleDateFormat fmt = new SimpleDateFormat("yyyy-MM-dd");
		for (String fullPath : Directory.enumerateFiles(path, "*.xlog"))
		{
			String name = Path.getFileNameWithoutExtension(fullPath);
			java.util.Date date = fmt.parse(name);
			rs.add(date);
		}
		
		Collections.sort(rs);
		
		return rs.toArray(new java.util.Date[]{});
	}

	public final JobTemplate[] GetJobTemplates() throws Exception
	{
		IJobTemplatesService svc = JobManager.getDefault().getRequiredService(IJobTemplatesService.class);
		return svc.GetJobTemplates();
	}

	public final int GetTerminatedCount() throws Exception
	{
		return getQueue().getTerminatedCount();
	}

	public final int GetUnterminatedCount() throws Exception
	{
		return getQueue().getUnterminatedCount();
	}


	public final String[] GetAllApplications() throws Exception
	{
		return getQueue().GetAllApplications();
	}

	public final String[] GetAllUsers() throws Exception
	{
		return getQueue().GetAllUsers();
	}





	public final String GetSettings(String typeName) throws Exception
	{
		return GetDefaultSettingsInstance(typeName).toXml();
	}

	public final void SetSettings(String typeName, String xml) throws Exception
	{
		SettingsBase settings = this.GetDefaultSettingsInstance(typeName);
		settings.renew(xml);
		settings.save();
	}


	@SuppressWarnings({ "rawtypes", "unchecked" })
	private SettingsBase GetDefaultSettingsInstance(String typeName) throws Exception
	{
		java.lang.Class t = java.lang.Class.forName(typeName);
		
		Method method = t.getDeclaredMethod("getDefault");
		
		method.setAccessible(true);
		
		return (SettingsBase) method.invoke(null);
		
	}

	public final String Version()
	{
		return "2.0.0.0";
	}


	public final String GetLocation()
	{
		return JavaLibraryUtils.getEntryLibrary().getParent();
	}


	
	private TreeMap<UUID, IJobServiceListener> _listeners = new TreeMap<UUID, IJobServiceListener>();

	@Override
	public void addListener(UUID token, IJobServiceListener listener)
			throws Exception {
		synchronized(this._listeners)
		{
			this._listeners.put(token, listener);
		}
		
	}



	@Override
	public void removeListener(UUID token) throws Exception {
		synchronized(this._listeners)
		{
			this._listeners.remove(token);
		}
		
	}



	@Override
	public void echo() throws Exception {
		
		
	}



	@Override
	public void Changed(final JobMetaData job) throws Exception {
		
		TreeMap<UUID, IJobServiceListener> listeners;
		synchronized(this._listeners)
		{
			
			listeners = new TreeMap<UUID, IJobServiceListener>(this._listeners);
			
		}
		
		for(final Map.Entry<UUID, IJobServiceListener> entry : listeners.entrySet())
		{
			ThreadFactory.queueUserWorkItem(new Runnable(){

				@Override
				public void run() {
					try
					{
						entry.getValue().Changed(job);
					}
					catch(Exception error)
					{
						synchronized(JobService.this._listeners)
						{
							JobService.this._listeners.remove(entry.getKey());
						}
					}
					
				}});
		}
		
	}



	@Override
	public void Added(final JobMetaData job) throws Exception {
		TreeMap<UUID, IJobServiceListener> listeners;
		synchronized(this._listeners)
		{
			
			listeners = new TreeMap<UUID, IJobServiceListener>(this._listeners);
			
		}
		
		for(final Map.Entry<UUID, IJobServiceListener> entry : listeners.entrySet())
		{
			ThreadFactory.queueUserWorkItem(new Runnable(){

				@Override
				public void run() {
					try
					{
						entry.getValue().Added(job);
					}
					catch(Exception error)
					{
						synchronized(JobService.this._listeners)
						{
							JobService.this._listeners.remove(entry.getKey());
						}
					}
					
				}});
		}
	}
	
	
	@Override
	public void initializeService() throws Exception
	{
		
		this.getQueue().addListener(this);
		
		RmiUtils.bind(this);
		
		
		super.initializeService();
	}
	
	@Override
	public void uninitializeService() throws Exception
	{
		super.uninitializeService();
		RmiUtils.unbind(this);
		this.getQueue().removeListener(this);
	}



	@Override
	public void RequestCancel(JobMetaData job) throws Exception {
		
	}



	@Override
	public void RequestSuspend(JobMetaData job) throws Exception {
		
	}



	@Override
	public void RequestUserPause(JobMetaData job) throws Exception {
	
	}

}