package Fantasy.Jobs.Resources;

import Fantasy.Configuration.*;

public class TemplateRuntimeScheduleSettings extends RuntimeScheduleSetting
{
	private java.util.ArrayList<TemplateRuntimeScheduleSetting> _templates = new java.util.ArrayList<TemplateRuntimeScheduleSetting>();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlArray("templates"), XmlArrayItem(ElementName="template", java.lang.Class = typeof(TemplateRuntimeScheduleSetting))]
	public final java.util.ArrayList<TemplateRuntimeScheduleSetting> getTemplates()
	{
		return _templates;
	}




}