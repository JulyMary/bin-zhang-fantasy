package fantasy.jobs.management;

import Fantasy.Jobs.Resources.*;

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------




public final class JobManagerSettings extends Fantasy.Configuration.SettingsBase
{
	private static JobManagerSettings _default = (JobManagerSettings)Fantasy.Configuration.SettingStorage.Load(new JobManagerSettings());

	public static JobManagerSettings getDefault()
	{
		return _default;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".\\Template")]
	public String getJobTemplateDirectory()
	{
		return (String)this.GetValue("JobTemplateDirectory");
	}
	public void setJobTemplateDirectory(String value)
	{
		this.SetValue("JobTemplateDirectory", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".\\Jobs")]
	public String getJobDirectory()
	{
		return (String)this.GetValue("JobDirectory");
	}
	public void setJobDirectory(String value)
	{
		this.SetValue("JobDirectory", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".Log")]
	public String getLogDirectory()
	{
		return (String)this.GetValue("LogDirectory");
	}
	public void setLogDirectory(String value)
	{
		this.SetValue("LogDirectory", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute("6")]
	public int getJobProcessCount()
	{
		return (int)this.GetValue("JobProcessCount");
	}
	public void setJobProcessCount(int value)
	{
		this.SetValue("JobProcessCount", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".\\Fantasy.Jobs.JobHost.exe")]
	public String getJobHostPath()
	{
		return (String)this.GetValue("JobHostPath");
	}
	public void setJobHostPath(String value)
	{
		this.SetValue("JobHostPath", value);
	}

	public String getStartupFolders()
	{
		return (String)this.GetValue("StartupFolders");
	}
	public void setStartupFolders(String value)
	{
		this.SetValue("StartupFolders", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".\\Schedule")]
	public String getScheduleDirectory()
	{
		return (String)this.GetValue("ScheduleDirectory");
	}
	public void setScheduleDirectory(String value)
	{
		this.SetValue("ScheduleDirectory", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute(".\\ScheduleTemplate")]
	public String getScheduleTemplateDirectory()
	{
		return (String)this.GetValue("ScheduleTemplateDirectory");
	}
	public void setScheduleTemplateDirectory(String value)
	{
		this.SetValue("ScheduleTemplateDirectory", value);
	}

	public String getWebSite()
	{
		return (String)this.GetValue("WebSite");
	}
	public void setWebSite(String value)
	{
		this.SetValue("WebSite", value);
	}

	public String getJobDirectoryUser()
	{
		return (String)this.GetValue("JobDirectoryUser");
	}
	public void setJobDirectoryUser(String value)
	{
		this.SetValue("JobDirectoryUser", value);
	}

	public String getJobDirectoryPassword()
	{
		return (String)this.GetValue("JobDirectoryPassword");
	}
	public void setJobDirectoryPassword(String value)
	{
		this.SetValue("JobDirectoryPassword", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[System.Configuration.DefaultSettingValueAttribute("Standalone")]
	public fantasy.jobs.management.ServiceType getServiceType()
	{
		return (fantasy.jobs.management.ServiceType)this.GetValue("ServiceType");
	}
	public void setServiceType(fantasy.jobs.management.ServiceType value)
	{
		this.SetValue("ServiceType", value);
	}




//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DefaultSettingValueAttribute("<TaskCountSettings />")]
	public TaskCountSettings getConcurrentTaskCount()
	{
		return (TaskCountSettings)this.GetValue("ConcurrentTaskCount");
	}
	public void setConcurrentTaskCount(TaskCountSettings value)
	{
		this.SetValue("ConcurrentTaskCount", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DefaultSettingValueAttribute("<TemplateCountSettings />")]
	public TemplateCountSettings getConcurrentTemplateCount()
	{
		return (TemplateCountSettings)this.GetValue("ConcurrentTemplateCount");
	}
	public void setConcurrentTemplateCount(TemplateCountSettings value)
	{
		this.SetValue("ConcurrentTemplateCount", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DefaultSettingValueAttribute("<RuntimeScheduleSetting />")]
	public RuntimeScheduleSetting getJobRuntimeSchedule()
	{
		return (RuntimeScheduleSetting)this.GetValue("JobRuntimeSchedule");
	}
	public void setJobRuntimeSchedule(RuntimeScheduleSetting value)
	{
		this.SetValue("JobRuntimeSchedule", value);
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DefaultSettingValueAttribute("<TemplateRuntimeScheduleSettings />")]
	public TemplateRuntimeScheduleSettings getTemplateRuntimeSchedule()
	{
		return (TemplateRuntimeScheduleSettings)this.GetValue("TemplateRuntimeSchedule");
		;
	}
	public void setTemplateRuntimeSchedule(TemplateRuntimeScheduleSettings value)
	{
		this.SetValue("TemplateRuntimeSchedule", value);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[DefaultSettingValueAttribute("<TaskRuntimeScheduleSettings />")]
	public TaskRuntimeScheduleSettings getTaskRuntimeSchedule()
	{
		return (TaskRuntimeScheduleSettings)this.GetValue("TaskRuntimeSchedule");
		;
	}
	public void setTaskRuntimeSchedule(TaskRuntimeScheduleSettings value)
	{
		this.SetValue("TaskRuntimeSchedule", value);
	}





	public String getJobTemplateDirectoryFullPath()
	{
	   return this.ExtractToFullPath(this.getJobTemplateDirectory());
	}


	private String ExtractToFullPath(String value)
	{
		String rs = Environment.ExpandEnvironmentVariables(value);

		if (!Path.IsPathRooted(rs))
		{
			Assembly asm = Assembly.GetEntryAssembly();
			String entryPath = Path.GetDirectoryName(asm.getLocation());

			rs = entryPath + Path.DirectorySeparatorChar + rs;
		}

		return new Uri(rs).LocalPath;
	}

	public String getLogDirectoryFullPath()
	{
		return this.ExtractToFullPath(this.getLogDirectory());
	}

	public String getJobDirectoryFullPath()
	{
		return this.ExtractToFullPath(getJobDirectory());
	}

	public String getJobHostFullPath()
	{
		return this.ExtractToFullPath(this.getJobHostPath());
	}

	public String getScheduleDirectoryFullPath()
	{
		return this.ExtractToFullPath(this.getScheduleDirectory());
	}

	public String getScheduleTemplateDirectoryFullPath()
	{
		return this.ExtractToFullPath(this.getScheduleTemplateDirectory());
	}


}