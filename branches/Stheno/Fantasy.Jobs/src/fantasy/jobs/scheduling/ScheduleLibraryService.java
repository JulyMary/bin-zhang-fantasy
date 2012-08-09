package fantasy.jobs.scheduling;

import Fantasy.IO.*;
import Fantasy.Jobs.Management.*;
import fantasy.xserialization.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults=true, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
public class ScheduleLibraryService extends WCFSingletonService implements IScheduleLibraryService
{
	private IScheduleService _scheduleService;

	@Override
	public void InitializeService()
	{
		_scheduleService = this.Site.<IScheduleService>GetService();
		LoadSchedules();

		super.InitializeService();
	}


	private java.util.ArrayList<ScheduleData> _schedules = new java.util.ArrayList<ScheduleData>();

	private void LoadSchedules()
	{
		ILogger logger = this.Site.<ILogger>GetService();


		String root = JobManagerSettings.getDefault().getScheduleDirectoryFullPath();
		String searchPattern = "*" + ScheduleExt;
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Iterable<String> dirs = root.Flatten(s => LongPathDirectory.EnumerateDirectories(s));
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		Iterable<String> files = from dir in dirs from file in LongPathDirectory.EnumerateFiles(dir, searchPattern) select file;
		XSerializer schedSer = new XSerializer(ScheduleItem.class);
		XSerializer dataSer = new XSerializer(ScheduleData.class);
		for (String file : files)
		{
			String name = this.ToScheduleRelativePath(file);
			name = name.substring(0, name.length() - ScheduleExt.length());

			try
			{
				XElement doc = XElement.Load(file);

				ScheduleItem sched = (ScheduleItem)schedSer.Deserialize(doc);

				String dataFile = file + ScheduleDataExt;

				ScheduleData data;
				if (LongPathFile.Exists(dataFile))
				{
					XElement dataDoc = XElement.Load(dataFile);

					data = (ScheduleData)dataSer.Deserialize(dataDoc);
				}
				else
				{
					data = new ScheduleData();
				}

				data.Site = this.Site;
				data.setName(name);
				sched.setName(name);
				data.setScheduleItem(sched);

				sched.getTrigger().setNextRunTime(data.NextRunTime);

				this._schedules.add(data);
				if (data.OnLoad())
				{
					SaveData(data);
				}

				this.RegisterToScheduleService(data);

			}
			catch(RuntimeException error)
			{
				if (logger != null)
				{
					logger.LogError("Schedule", error, "Failed to load schedule {0}.", name);
				}
			}
		}
	}

	private void RegisterToScheduleService(ScheduleData data)
	{
		String message;
		if (data.getScheduleItem().getEnabled())
		{
			if (!data.Expired)
			{
				if(this._scheduleService != null)
				{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					data.setScheduleCookie(this._scheduleService.Register(data.NextRunTime.getValue(), ()=>)
					{
						data.RunAction();
						data.EvalNext();
						if (!data.Expired)
						{
							RegisterToScheduleService(data);
						}
						if (data.Expired && data.getScheduleItem().getDeleteAfterExpired())
						{
							this._schedules.remove(data);
							String path = LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), data.getScheduleItem().getName() + ScheduleExt);
							if (LongPathFile.Exists(path))
							{
								LongPathFile.Delete(path);
							}
							path += ScheduleDataExt;
							if (LongPathFile.Exists(path))
							{
								LongPathFile.Delete(path);
							}
						}
						else
						{
							SaveData(data);
						}
					}
				   );
				}
				message = String.format("Schedule %1$s will be execute at %2$s.", data.getName(), data.NextRunTime);
			}
			else
			{
				message = String.format("Schedule %1$s is enabled.", data.getName());
			}
		}
		else
		{
			message = String.format("Schedule %1$s is disabled.", data.getName());
		}
		ILogger logger = this.Site.<ILogger>GetService();
		if (logger != null)
		{
			logger.LogMessage("Schedule", message);
		}
	}

	private void SaveData(ScheduleData data)
	{
		SaveXml(LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), data.getName() + ScheduleExt + ScheduleDataExt), data, true);
	}

	@Override
	public void UninitializeService()
	{
		super.UninitializeService();
	}


	private String ToScheduleRelativePath(String path)
	{
		String rs = LongPath.GetRelativePath(JobManagerSettings.getDefault().getScheduleDirectoryFullPath() + "\\", path);
		if (rs.startsWith(".\\"))
		{
			rs = rs.substring(2);
		}
		return rs;
	}






	private static final String ScheduleExt = ".xsched";
	private static final String ScheduleDataExt = ".xdat";

	public final void DeleteGroup(String path)
	{

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		String query = from data in this._schedules where data.getName().startsWith(path + "\\", StringComparison.OrdinalIgnoreCase) select data.getName();
		String[] schedules = query.toArray();
		for (String schedule : schedules)
		{
			this.DeleteSchedule(schedule);
		}

		path = LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		if (LongPathDirectory.Exists(path))
		{
			LongPathDirectory.Delete(path);
		}
	}

	XmlWriterSettings tempVar = new XmlWriterSettings();
	tempVar.Encoding = Encoding.UTF8;
	tempVar.Indent = true;
	tempVar.IndentChars = "    ";
	tempVar.OmitXmlDeclaration = false;
	tempVar.CloseOutput = true;
	private XmlWriterSettings _xmlWriterSettings = tempVar;

	private Object _syncRoot = new Object();

	private ScheduleData GetDataByPath(String path)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from d in this._schedules where StringComparer.OrdinalIgnoreCase.Compare(path, d.getName()) == 0 select d;
		return query.SingleOrDefault();
	}

	private void SaveXml(String path, Object instance, boolean overwrite)
	{


		XSerializer ser = new XSerializer(instance.getClass());
		XElement ele = ser.Serialize(instance);


		FileStream fs = LongPathFile.Open(path, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
		XmlWriter writer = XmlWriter.Create(fs, _xmlWriterSettings);
		try
		{

			ele.Save(writer);
		}
		finally
		{
			writer.Close();
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IScheduleLibraryService Members

	public final void CreateSchedule(String path, ScheduleItem schedule, boolean overwrite)
	{
		synchronized (_syncRoot)
		{
			IScheduleService svc = this.Site.<IScheduleService>GetService();
			ScheduleData old = this.GetDataByPath(path);

			if (old == null || overwrite)
			{
				java.util.ArrayList<ScheduleEvent> events = new java.util.ArrayList<ScheduleEvent>();
				if (old != null)
				{
					if (old.getScheduleItem().getEnabled() || !old.Expired && svc != null)
					{
						svc.Unregister(old.getScheduleCookie());
					}
					events.addAll(old.getHistory());
					this._schedules.remove(old);
				}
				schedule.setName(path);
				ScheduleData data = new ScheduleData();
				data.Site = this.Site;
				data.setName(path);
				data.setScheduleItem(schedule);
				data.getHistory().addAll(events);

				SaveXml(LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(),path + ScheduleExt), schedule, true);
				this._schedules.add(data);

				data.OnLoad();
				this.SaveData(data);
				this.RegisterToScheduleService(data);

			}
			else
			{
				throw new ScheduleException(String.format(Properties.Resources.getScheduleExistsText(), path));
			}


		}

	}

	public final void DeleteSchedule(String path)
	{
		synchronized (_syncRoot)
		{
			IScheduleService svc = this.Site.<IScheduleService>GetService();
			ScheduleData data = this.GetDataByPath(path);
			if (data != null)
			{
				if (data.getScheduleItem().getEnabled() || !data.Expired && svc != null)
				{
					svc.Unregister(data.getScheduleCookie());
				}
				this._schedules.remove(data);
			}

			path = LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path + ScheduleExt);
			if (LongPathFile.Exists(path))
			{
				LongPathFile.Delete(path);
			}
			path += ScheduleDataExt;
			if (LongPathFile.Exists(path))
			{
				LongPathFile.Delete(path);
			}

		}
	}


	public final String[] GetScheduleNames(String path)
	{

		path = !path.equals("") ? path + "\\" : path;

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		String query = from data in this._schedules where data.getName().startsWith(path, StringComparison.OrdinalIgnoreCase) && data.getName().substring(path.length()).indexOf('\\') < 0 select data.getName();
		return query.toArray();
	}

	public final ScheduleItem GetSchedule(String path)
	{
		ScheduleData data = this.GetDataByPath(path);
		return data != null ? data.getScheduleItem() : null;
	}

	public final ScheduleEvent[] GetScheduleHistory(String path)
	{
		ScheduleData data = this.GetDataByPath(path);
		return data != null ? data.getHistory().toArray(new ScheduleEvent[]{}) : null;
	}

	public final String[] GetTemplateNames()
	{
		java.util.ArrayList<String> rs = new java.util.ArrayList<String>();
		for (String file : LongPathDirectory.EnumerateFiles(JobManagerSettings.getDefault().getScheduleTemplateDirectoryFullPath(), "*.xslt"))
		{
			rs.add(Path.GetFileNameWithoutExtension(file));
		}
		return rs.toArray(new String[]{});
	}

	public final String GetTemplate(String name)
	{
		String file = LongPath.Combine(JobManagerSettings.getDefault().getScheduleTemplateDirectoryFullPath(), name + ".xslt");
		return LongPathFile.ReadAllText(file);
	}

	public final void CreateGroup(String path)
	{
		path = LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		if (!LongPathDirectory.Exists(path))
		{
			LongPathDirectory.Create(path);
		}
	}

	public final String[] GetGroups(String path)
	{
		path = LongPath.Combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		String[] dirs = LongPathDirectory.EnumerateDirectories(path).toArray();
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var query = from p in dirs select ToScheduleRelativePath(p);
		return query.toArray();
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}