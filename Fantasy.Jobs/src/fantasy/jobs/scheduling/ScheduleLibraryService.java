package fantasy.jobs.scheduling;

import java.rmi.RemoteException;
import java.text.SimpleDateFormat;

import fantasy.*;

import fantasy.io.*;
import fantasy.jobs.management.*;
import fantasy.xserialization.*;
import fantasy.servicemodel.*;
import org.jdom2.*;


public class ScheduleLibraryService extends AbstractService implements IScheduleLibraryService
{
	public ScheduleLibraryService() throws RemoteException {
		super();
		// TODO Auto-generated constructor stub
	}


	private IScheduleService _scheduleService;

	@Override
	public void initializeService() throws Exception
	{
		_scheduleService = this.getSite().getService(IScheduleService.class);
		loadSchedules();

		super.initializeService();
	}


	private java.util.ArrayList<ScheduleData> _schedules = new java.util.ArrayList<ScheduleData>();

	private void loadSchedules() throws Exception
	{
		ILogger logger = this.getSite().getService(ILogger.class);


		String root = JobManagerSettings.getDefault().getScheduleDirectoryFullPath();
		String searchPattern = "*" + ScheduleExt;
		
		
		

		XSerializer schedSer = new XSerializer(ScheduleItem.class);
		XSerializer dataSer = new XSerializer(ScheduleData.class);
		for (String file : Directory.enumerateFiles(root, searchPattern, true))
		{
			String name = this.ToScheduleRelativePath(file);
			name = name.substring(0, name.length() - ScheduleExt.length());

			try
			{
				Element doc = JDomUtils.loadElement(file);

				ScheduleItem sched = (ScheduleItem)schedSer.deserialize(doc);

				String dataFile = file + ScheduleDataExt;

				ScheduleData data;
				if (File.exists(dataFile))
				{
					Element dataDoc = JDomUtils.loadElement(dataFile);

					data = (ScheduleData)dataSer.deserialize(dataDoc);
				}
				else
				{
					data = new ScheduleData();
				}

				data.setSite(this.getSite());
			
				data.Name = name;
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
			catch(Exception error)
			{
				if (logger != null)
				{
					logger.LogError("Schedule", error, "Failed to load schedule %1$s.", name);
				}
			}
		}
	}

	private void RegisterToScheduleService(final ScheduleData data)
	{
		String message;
		if (data.getScheduleItem().getEnabled())
		{
			if (!data.Expired)
			{
				if(this._scheduleService != null)
				{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
					data.setScheduleCookie(this._scheduleService.register(data.NextRunTime, new Action(){

						@Override
						public void act() throws Exception {
							data.RunAction();
							data.EvalNext();
							if (!data.Expired)
							{
								RegisterToScheduleService(data);
							}
							if (data.Expired && data.getScheduleItem().getDeleteAfterExpired())
							{
								ScheduleLibraryService.this._schedules.remove(data);
								String path = Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), data.getScheduleItem().getName() + ScheduleExt);
								if (File.exists(path))
								{
									File.delete(path);
								}
								path += ScheduleDataExt;
								if (File.exists(path))
								{
									File.delete(path);
								}
							}
							else
							{
								SaveData(data);
							}
							
						}}));
					
				}
				
				SimpleDateFormat fmt = new SimpleDateFormat();
				message = String.format("Schedule %1$s will be execute at %2$s.", data.Name, fmt.format(data.NextRunTime));
			}
			else
			{
				message = String.format("Schedule %1$s is enabled.", data.Name);
			}
		}
		else
		{
			message = String.format("Schedule %1$s is disabled.", data.Name);
		}
		ILogger logger = this.getSite().getService(ILogger.class);
		if (logger != null)
		{
			logger.LogMessage("Schedule", message);
		}
	}

	private void SaveData(ScheduleData data)
	{
		SaveXml(Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), data.Name + ScheduleExt + ScheduleDataExt), data, true);
	}

	


	private String ToScheduleRelativePath(String path)
	{
		String rs = Path.getRelativePath(JobManagerSettings.getDefault().getScheduleDirectoryFullPath() + "\\", path);
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


}