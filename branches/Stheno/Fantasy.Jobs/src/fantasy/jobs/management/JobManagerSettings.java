package fantasy.jobs.management;

import fantasy.*;
import fantasy.jobs.Consts;
import fantasy.jobs.resources.*;
import fantasy.xserialization.*;


@XSerializable(name = "JobManagerSettings", namespaceUri=Consts.JobServiceNamespaceURI)
public final class JobManagerSettings extends fantasy.configuration.SettingsBase
{
	private static JobManagerSettings _default;

	
	
	public static JobManagerSettings getDefault() throws Exception
	{
		if(_default == null)
		{
			_default = (JobManagerSettings)fantasy.configuration.SettingStorage.load(new JobManagerSettings());
		}
		
		return _default;
	}


	
	@XElement(name="JobTemplateDirectory")
	private String _jobTemplateDirectory = ".\\Templates";
	
	public String getJobTemplateDirectory()
	{
		return _jobTemplateDirectory;
	}
	

    @XElement(name="JobDirectory")
	private String _jobDirectory = ".\\Jobs";
	public String getJobDirectory()
	{
		return _jobDirectory;
	}
	

	@XElement(name="LogDirectory")
	private String _logDirectory = ".\\Log";
	
	public String getLogDirectory()
	{
		return _logDirectory;
	}
	

	@XElement(name="MaxDegreeOfParallelism")
	private int _maxDegreeOfParallelism = 6;
	
	public int getMaxDegreeOfParallelism()
	{
		return _maxDegreeOfParallelism;
	}
	
	public void setMaxDegreeOfParallelism(int value) throws Exception
	{
		if(this._maxDegreeOfParallelism != value)
		{
			this._maxDegreeOfParallelism = value;
			this.onPropertyChanged("MaxDegreeOfParallelism");
		}
	}
	
	
	@XElement(name="TemplateCapacity")
	private TemplateCapacitySettings _templateCapacity = new TemplateCapacitySettings();
	public TemplateCapacitySettings getTemplateCapacity()
	{
		return _templateCapacity;
	}
	
	public void setTemplateCapacity(TemplateCapacitySettings value) throws Exception
	{
		if(_templateCapacity != null)
		{
			this._templateCapacity = value;
			this.onPropertyChanged("TemplateCapacity");
		}
	}
	
	@XElement(name="TaskCapacity")
	private TaskCapacitySettings _taskTaskCapacity = new TaskCapacitySettings();
	public TaskCapacitySettings getTaskCapacity()
	{
		return _taskTaskCapacity;
	}
	
	public void setTaskCapacity(TaskCapacitySettings value) throws Exception
	{
		if(_taskTaskCapacity != null)
		{
			this._taskTaskCapacity = value;
			this.onPropertyChanged("TaskCapacity");
		}
	}
	
	
	
	@XElement(name="StartupFolders")
	private String _startupFolders = "";
	
	public String getStartupFolders()
	{
		return _startupFolders;
	}
	

	@XElement(name="ScheduleDirectory")
    private String _scheduleDirectory = ".\\Schedule";
	public String getScheduleDirectory()
	{
		return _scheduleDirectory;
	}
	

	@XElement(name="ScheduleTemplateDirectory")
	private String _scheduleTemplateDirectory = ".\\ScheduleTemplate";
	public String getScheduleTemplateDirectory()
	{
		return _scheduleTemplateDirectory;
	}
	

	@XElement(name="WebSite")
	private String _webSite = "";
	public String getWebSite()
	{
		return _webSite;
	}
	public void setWebSite(String value) throws Exception
	{
		if(this._webSite != value)
		{
			this._webSite = value;
			this.onPropertyChanged("WebSite");
		}
	}

	@XElement(name="JobDirectoryUser")
	private String _jobDirectoryUser = "";
	public String getJobDirectoryUser()
	{
		return _jobDirectoryUser;
	}
	
	@XElement(name="JobDirectoryPassword")
    private String _jobDirectoryPassword;
	public String getJobDirectoryPassword()
	{
		return this._jobDirectoryPassword;
	}
	


	@XElement(name="JobRunningTime")
    private  RunningTimeSetting _jobRunningTime = new RunningTimeSetting();
	public RunningTimeSetting getJobRunningTime()
	{
		return this._jobRunningTime;
	}
	public void setJobRunningTime(RunningTimeSetting value) throws Exception
	{
		 this._jobRunningTime = value;
		 this.onPropertyChanged("JobRunningTime");
	}

	
    @XElement(name="TemplateRunningTime")
	private TemplateRunningTimeSettings _templateRunningTime = new TemplateRunningTimeSettings();
	public TemplateRunningTimeSettings getTemplateRunningTime()
	{
		return this._templateRunningTime;
	}
	public void setTemplateRunningTime(TemplateRunningTimeSettings value) throws Exception
	{
		this._templateRunningTime = value;
		this.onPropertyChanged("TemplateRunningTime");
	}


	@XElement(name="TaskRunningTime")
	private TaskRunningTimeSettings _taskRunningTime = new TaskRunningTimeSettings();
	public TaskRunningTimeSettings getTaskRunningTime()
	{
		return this._taskRunningTime;
		
	}
	public void setTaskRunningTime(TaskRunningTimeSettings value) throws Exception
	{
		this._taskRunningTime = value;
		this.onPropertyChanged("TaskRunningTime");
		
	}





	public String getJobTemplateDirectoryFullPath() throws Exception
	{
	   return JavaLibraryUtils.extractToFullPath(this.getJobTemplateDirectory());
	}


	
	public String getLogDirectoryFullPath() throws Exception
	{
		return JavaLibraryUtils.extractToFullPath(this.getLogDirectory());
	}

	public String getJobDirectoryFullPath() throws Exception
	{
		return JavaLibraryUtils.extractToFullPath(getJobDirectory());
	}

	

	public String getScheduleDirectoryFullPath() throws Exception
	{
		return JavaLibraryUtils.extractToFullPath(this.getScheduleDirectory());
	}

	public String getScheduleTemplateDirectoryFullPath() throws Exception
	{
		return JavaLibraryUtils.extractToFullPath(this.getScheduleTemplateDirectory());
	}
	
	@XElement(name = "DatabasePath")
	private String _databasePath = ".\\jobs.db;";
	public String getJobsDbConnectionString() throws Exception
	{
		String fullPath = JavaLibraryUtils.extractToFullPath(_databasePath);
		return String.format("jdbc:derby:%1$s", fullPath);
		
	}


}