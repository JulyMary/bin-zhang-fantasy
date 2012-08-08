package fantasy.jobs.resources;

import Fantasy.Configuration.*;

public class TemplateRuntimeScheduleSettings extends RunningTimeSetting
{
	private java.util.ArrayList<TemplateRunningTimeSetting> _templates = new java.util.ArrayList<TemplateRunningTimeSetting>();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlArray("templates"), XmlArrayItem(ElementName="template", java.lang.Class = typeof(TemplateRuntimeScheduleSetting))]
	public final java.util.ArrayList<TemplateRunningTimeSetting> getTemplates()
	{
		return _templates;
	}




}