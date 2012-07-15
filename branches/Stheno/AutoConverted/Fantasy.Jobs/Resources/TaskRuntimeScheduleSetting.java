package Fantasy.Jobs.Resources;

import Fantasy.Configuration.*;

public class TaskRuntimeScheduleSetting extends RuntimeScheduleSetting
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
	//[XmlAttribute("namespace")]
	private String privateNamespace;
	public final String getNamespace()
	{
		return privateNamespace;
	}
	public final void setNamespace(String value)
	{
		privateNamespace = value;
	}


}