package fantasy.jobs.resources;

import fantasy.xserialization.XArray;
import fantasy.xserialization.XArrayItem;
import fantasy.xserialization.XAttribute;
import fantasy.xserialization.XSerializable;

@XSerializable(name="templates", namespaceUri=fantasy.jobs.Consts.XNamespaceURI)
public class TemplateCapacitySettings
{
	
	@XAttribute(name = "capacity")
	private int privateCapacity = Integer.MAX_VALUE;
	public final int getCapacity()
	{
		return privateCapacity;
	}
	public final void setCapacity(int value)
	{
		privateCapacity = value;
	}


	@XArray(items=@XArrayItem(name="template", type=TaskCapacitySetting.class))
	private java.util.ArrayList<TemplateCapacitySetting> _templates = new java.util.ArrayList<TemplateCapacitySetting>();

	public final java.util.ArrayList<TemplateCapacitySetting> getTemplates()
	{
		return _templates;
	}

	
}