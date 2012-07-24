package fantasy.jobs.management;

import Fantasy.IO.*;
import Fantasy.Configuration.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
public class JobService extends WCFSingletonService implements IJobService
{

	private IJobQueue getQueue()
	{
		return JobManager.getDefault().<IJobQueue>GetRequiredService();
	}



	public final JobMetaData StartJob(String startInfo)
	{

		JobMetaData job = getQueue().CreateJobMetaData();
		job.LoadXml(startInfo);
		getQueue().ApplyChange(job);
		return job;
	}

	public final void Resume(Guid id)
	{
		getQueue().Resume(id);
	}

	public final void Cancel(Guid id)
	{
		getQueue().Cancel(id);
	}

	public final void Pause(Guid id)
	{
		getQueue().UserPause(id);
	}

	private JobMetaData[] GetJobMetaDataByIds(Guid[] ids)
	{
		if (ids != null)
		{
			java.util.ArrayList<JobMetaData> rs = new java.util.ArrayList<JobMetaData>();
			for (Guid id : ids)
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



	public final void Resume(Guid[] ids)
	{
		JobMetaData[] jobs = GetJobMetaDataByIds(ids);
		Resume(jobs);
	}

//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public void ResumeByFilter(string filter, string[] args = null)
	public final void ResumeByFilter(String filter, String[] args)
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, args, null, 0, Integer.MAX_VALUE).toArray();
		total = tempRef_total.argvalue;
		this.Resume(jobs);
	}


	private void Cancel(JobMetaData[] jobs)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from job in jobs where !job.IsTerminated orderby job.State != JobState.Running select job;
		for (JobMetaData job : query)
		{
			try
			{
				getQueue().Cancel(job.getId());
			}
			catch (java.lang.Exception e)
			{
			}
		}
	}

	public final void Cancel(Guid[] ids)
	{
		JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
		this.Cancel(jobs);
	}


//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public void CancelByFilter(string filter, string[] args = null)
	public final void CancelByFilter(String filter, String[] args)
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, args, null, 0, Integer.MAX_VALUE).toArray();
		total = tempRef_total.argvalue;
		Cancel(jobs);

	}

	private void Pause(JobMetaData[] jobs)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from job in jobs where job.IsTerminated == false && job.State != JobState.UserPaused orderby job.State != JobState.Running select job;
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

	public final void Pause(Guid[] ids)
	{
		JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
		this.Pause(jobs);
	}





//C# TO JAVA CONVERTER TODO TASK: C# optional parameters are not converted to Java:
//ORIGINAL LINE: public void PauseByFilter(string filter, string[] args = null)
	public final void PauseByFilter(String filter, String[] args)
	{
		int total = 0;
		RefObject<Integer> tempRef_total = new RefObject<Integer>(total);
		JobMetaData[] jobs = getQueue().FindUnterminated(tempRef_total, filter, args, null, 0, Integer.MAX_VALUE).toArray();
		total = tempRef_total.argvalue;
		Pause(jobs);
	}


	public final JobMetaData FindJobById(Guid id)
	{
		return getQueue().FindJobMetaDataById(id);
	}

	public final JobMetaData[] FindAllJobs()
	{
		return getQueue().FindAll().toArray();
	}

	public final JobMetaData[] FindUnterminatedJob(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{
		return getQueue().FindUnterminated(totalCount, filter, args, order, skip, take).toArray();
	}

	public final JobMetaData[] FindTerminatedJob(RefObject<Integer> totalCount, String filter, String[] args, String order, int skip, int take)
	{

		return getQueue().FindTerminated(totalCount, filter, args, order, skip, take).toArray();

	}

	public final String GetJobLog(Guid id)
	{
		String path = String.format("%1$s\\%2$s\\%2$s.xlog", JobManagerSettings.getDefault().getJobDirectoryFullPath(), id);
		String rs = null;
		try
		{
			rs = LongPathFile.ReadAllText(path);
		}
		catch (IOException e)
		{

		}

		return rs;
	}

	public final String GetJobScript(Guid id)
	{
		String rs = null;
		String path = String.format("%1$s\\%2$s\\%2$s.job", JobManagerSettings.getDefault().getJobDirectoryFullPath(), id);
		if (!File.Exists(path))
		{
			path += ".bak";
		}
		if (File.Exists(path))
		{
			try
			{
				rs = File.ReadAllText(path);
			}
			catch (IOException e)
			{

			}
		}

		return rs;
	}

	public final String GetManagerLog(java.util.Date date)
	{
		XElement rs = new XElement("xlog");
		String path = JobManagerSettings.getDefault().getLogDirectoryFullPath();
		path = LongPath.Combine(path, date.ToString("yyyy-MM-dd") + ".xlog");
		if (LongPathFile.Exists(path))
		{
			java.util.ArrayList<XElement> nodes = new java.util.ArrayList<XElement>();
			FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			StreamReader reader = new StreamReader(fs);
			try
			{
				String line;
				while (!reader.EndOfStream)
				{
					line = reader.ReadLine();

					try
					{
						XElement node = XElement.Parse(line);
						rs.Add(node);
					}
					catch (java.lang.Exception e)
					{
					}
				}
			}
			finally
			{
				reader.Close();
			}


		}

		return rs.toString();
	}

	public final java.util.Date[] GetManagerLogAvaiableDates()
	{
		java.util.ArrayList<java.util.Date> rs = new java.util.ArrayList<java.util.Date>();
		String path = JobManagerSettings.getDefault().getLogDirectoryFullPath();

		for (String fullPath : LongPathDirectory.EnumerateFiles(path, "*.xlog"))
		{
			String name = Path.GetFileNameWithoutExtension(fullPath);
			java.util.Date date = new java.util.Date(java.util.Date.parse(name));
			rs.add(date);
		}
		rs.Sort();
		return rs.toArray(new java.util.Date[]{});
	}

	public final JobTemplate[] GetJobTemplates()
	{
		IJobTemplatesService svc = JobManager.getDefault().<IJobTemplatesService>GetRequiredService();
		return svc.GetJobTemplates();
	}

	public final int GetTerminatedCount()
	{
		return getQueue().getTerminates().Count();
	}

	public final int GetUnterminatedCount()
	{
		return getQueue().getUnterminates().Count();
	}


	public final String[] GetAllApplications()
	{
		return getQueue().GetAllApplications();
	}

	public final String[] GetAllUsers()
	{
		return getQueue().GetAllUsers();
	}





	public final String GetSettings(String typeName)
	{
		return GetDefaultSettingsInstance(typeName).ToXml();
	}

	public final void SetSettings(String typeName, String xml)
	{
		SettingsBase settings = this.GetDefaultSettingsInstance(typeName);
		settings.Renew(xml);
		settings.Save();
	}


	private SettingsBase GetDefaultSettingsInstance(String typeName)
	{
		java.lang.Class t = java.lang.Class.forName(typeName, true);
		SettingsBase rs = (SettingsBase)t.InvokeMember("Default", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static, null, null, null);
		return rs;
	}

	public final String Version()
	{
		Assembly assembly = Assembly.GetCallingAssembly();
		return assembly.GetName().Version.toString();
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJobService Members


	public final String GetLocation()
	{
		return LongPath.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}