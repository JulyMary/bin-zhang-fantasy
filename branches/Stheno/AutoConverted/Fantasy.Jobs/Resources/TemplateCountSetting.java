package Fantasy.Jobs.Resources;

import Fantasy.Configuration.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XmlRoot("template")]
public class TemplateCountSetting
{


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XmlAttribute("name")]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
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