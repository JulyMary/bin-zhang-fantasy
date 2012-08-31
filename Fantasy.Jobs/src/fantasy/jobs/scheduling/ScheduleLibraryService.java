package fantasy.jobs.scheduling;

import java.rmi.RemoteException;
import java.text.SimpleDateFormat;
import java.util.*;

import fantasy.*;

import fantasy.collections.*;
import fantasy.io.*;
import fantasy.jobs.management.*;
import fantasy.jobs.properties.Resources;
import fantasy.xserialization.*;
import fantasy.servicemodel.*;

import org.apache.commons.lang3.StringUtils;
import org.jdom2.*;


public class ScheduleLibraryService extends AbstractService implements IScheduleLibraryService
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 7891440656826934324L;

	public ScheduleLibraryService() throws RemoteException {
		super();
	
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
			String name = this.toScheduleRelativePath(file);
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
					saveData(data);
				}

				this.registerToScheduleService(data);

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

	private void registerToScheduleService(final ScheduleData data) throws Exception
	{
		String message;
		if (data.getScheduleItem().getEnabled())
		{
			if (!data.Expired)
			{
				if(this._scheduleService != null)
				{
    					data.setScheduleCookie(this._scheduleService.register(data.NextRunTime, new Action(){

						@Override
						public void call() throws Exception {
							data.RunAction();
							data.EvalNext();
							if (!data.Expired)
							{
								registerToScheduleService(data);
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
								saveData(data);
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

	private void saveData(ScheduleData data) throws Exception
	{
		saveXml(Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), data.Name + ScheduleExt + ScheduleDataExt), data, true);
	}

	


	private String toScheduleRelativePath(String path) throws Exception
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

	public final void deleteGroup(String path) throws Exception
	{


		
		
		for (ScheduleData data : this._schedules.toArray(new ScheduleData[0]))
		{
			if(StringUtils.startsWithIgnoreCase(data.Name, path))
			{
			    this.deleteSchedule(data.Name);
			}
		}

		path = Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		if (Directory.exists(path))
		{
			Directory.delete(path, true);
		}
	}

	
	private Object _syncRoot = new Object();

	private ScheduleData getDataByPath(final String path) throws Exception
	{
		
		ScheduleData rs = new Enumerable<ScheduleData>(this._schedules).singleOrDefault(new Predicate<ScheduleData>(){

			@Override
			public boolean evaluate(ScheduleData obj) throws Exception {
				return StringUtils.equalsIgnoreCase(obj.Name, path);
			}});
		
		return rs;
		
		
	}

	private void saveXml(String path, Object instance, boolean overwrite) throws Exception
	{


		XSerializer ser = new XSerializer(instance.getClass());
		Element ele = ser.serialize(instance);
		
		JDomUtils.saveElement(ele, path);

	}


	public final void createSchedule(String path, ScheduleItem schedule, boolean overwrite) throws Exception
	{
		synchronized (_syncRoot)
		{
			IScheduleService svc = this.getSite().getService(IScheduleService.class);
			ScheduleData old = this.getDataByPath(path);

			if (old == null || overwrite)
			{
				java.util.ArrayList<ScheduleEvent> events = new java.util.ArrayList<ScheduleEvent>();
				if (old != null)
				{
					if (old.getScheduleItem().getEnabled() || !old.Expired && svc != null)
					{
						svc.unregister(old.getScheduleCookie());
					}
					events.addAll(old.getHistory());
					this._schedules.remove(old);
				}
				schedule.setName(path);
				ScheduleData data = new ScheduleData();
				data.setSite(this.getSite());
				data.Name = path;
				data.setScheduleItem(schedule);
				data.getHistory().addAll(events);

				saveXml(Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(),path + ScheduleExt), schedule, true);
				this._schedules.add(data);

				data.OnLoad();
				this.saveData(data);
				this.registerToScheduleService(data);

			}
			else
			{
				throw new ScheduleException(String.format(Resources.getScheduleExistsText(), path));
			}


		}

	}

	public final void deleteSchedule(String path) throws Exception
	{
		synchronized (_syncRoot)
		{
			IScheduleService svc = this.getSite().getService(IScheduleService.class);
			ScheduleData data = this.getDataByPath(path);
			if (data != null)
			{
				if (data.getScheduleItem().getEnabled() || !data.Expired && svc != null)
				{
					svc.unregister(data.getScheduleCookie());
				}
				this._schedules.remove(data);
			}

			path = Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path + ScheduleExt);
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
	}


	public final String[] getScheduleNames(String path)
	{

		path = !path.equals("") ? path + "\\" : path;
		
		ArrayList<String> rs = new ArrayList<String>();
		
		for (ScheduleData data : this._schedules.toArray(new ScheduleData[0]))
		{
			if(StringUtils.startsWithIgnoreCase(data.Name, path))
			{
			   rs.add(data.Name);
			}
		}
		
		return rs.toArray(new String[0]);
		
	}

	public final ScheduleItem getSchedule(String path) throws Exception
	{
		ScheduleData data = this.getDataByPath(path);
		return data != null ? data.getScheduleItem() : null;
	}

	public final ScheduleEvent[] getScheduleHistory(String path) throws Exception
	{
		ScheduleData data = this.getDataByPath(path);
		return data != null ? data.getHistory().toArray(new ScheduleEvent[]{}) : null;
	}

	public final String[] getTemplateNames() throws Exception
	{
		java.util.ArrayList<String> rs = new java.util.ArrayList<String>();
		for (String file : Directory.enumerateFiles(JobManagerSettings.getDefault().getScheduleTemplateDirectoryFullPath(), "*.xslt"))
		{
			rs.add(Path.getFileNameWithoutExtension(file));
		}
		return rs.toArray(new String[]{});
	}

	public final String getTemplate(String name) throws Exception
	{
		String file = Path.combine(JobManagerSettings.getDefault().getScheduleTemplateDirectoryFullPath(), name + ".xslt");
		return File.readAllText(file, "UTF-8");
	}

	public final void createGroup(String path) throws Exception
	{
		path = Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		if (!Directory.exists(path))
		{
			Directory.create(path);
		}
	}

	public final String[] getGroups(String path) throws Exception
	{
		
		ArrayList<String> rs = new ArrayList<String>();
		
		path = Path.combine(JobManagerSettings.getDefault().getScheduleDirectoryFullPath(), path);
		Iterable<String> dirs = Directory.enumerateDirectories(path, false);
		
		for(String dir : dirs)
		{
			rs.add(toScheduleRelativePath(dir));
		}
		
		return rs.toArray(new String[0]);
	}


}