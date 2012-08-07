package fantasy.jobs.resources;

import Fantasy.Configuration.*;

public class TemplateCountSettings
{
	public TemplateCountSettings()
	{
		setCount(Integer.MAX_VALUE);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlAttribute("count")]
	private int privateCount;
	public final int getCount()
	{
		return privateCount;
	}
	public final void setCount(int value)
	{
		privateCount = value;
	}

	private java.util.ArrayList<TemplateCountSetting> _templates = new java.util.ArrayList<TemplateCountSetting>();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlArray("templates"), XmlArrayItem(java.lang.Class = typeof(TemplateCountSetting), ElementName = "template")]
	public final java.util.ArrayList<TemplateCountSetting> getTemplates()
	{
		return _templates;
	}


	@Override
	public boolean equals(Object obj)
	{
		return ComparsionHelper.DeepEquals(this, obj);
	}

	@Override
	public int hashCode()
	{
		return super.hashCode();
	}
}