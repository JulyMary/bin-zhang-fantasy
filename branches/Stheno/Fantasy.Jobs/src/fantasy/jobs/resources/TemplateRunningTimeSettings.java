package fantasy.jobs.resources;

import fantasy.xserialization.*;


@XSerializable(name="TemplateRunningTimeSettings", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TemplateRunningTimeSettings extends RunningTimeSetting
{
	@XArray(items=@XArrayItem(name="template", type=TemplateRunningTimeSetting.class))
	private java.util.ArrayList<TemplateRunningTimeSetting> _templates = new java.util.ArrayList<TemplateRunningTimeSetting>();

	public final java.util.ArrayList<TemplateRunningTimeSetting> getTemplates()
	{
		return _templates;
	}




}