package Fantasy.Jobs;

import Fantasy.XSerialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[, XSerializable("jobStart", NamespaceUri=Consts.XNamespaceURI)]
public class JobStartInfo implements Serializable
{
	public JobStartInfo()
	{
		this.setID(Guid.NewGuid());
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("id")]
	private Guid privateID = new Guid();
	public final Guid getID()
	{
		return privateID;
	}
	public final void setID(Guid value)
	{
		privateID = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("target")]
	private String privateTarget;
	public final String getTarget()
	{
		return privateTarget;
	}
	public final void setTarget(String value)
	{
		privateTarget = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("template")]
	private String privateTemplate;
	public final String getTemplate()
	{
		return privateTemplate;
	}
	public final void setTemplate(String value)
	{
		privateTemplate = value;
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("name")]
	private String privateName;
	public final String getName()
	{
		return privateName;
	}
	public final void setName(String value)
	{
		privateName = value;
	}

	private java.util.ArrayList<JobPropertyGroup> _propertyGroups = new java.util.ArrayList<JobPropertyGroup>();

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray, XArrayItem(Name="properties", java.lang.Class=typeof(JobPropertyGroup))]
	public final java.util.List<JobPropertyGroup> getPropertyGroups()
	{
		return _propertyGroups;
	}

	private java.util.ArrayList<TaskItemGroup> _itemGroups = new java.util.ArrayList<TaskItemGroup>();
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray, XArrayItem(Name="items", java.lang.Class = typeof(TaskItemGroup))]
	public final java.util.List<TaskItemGroup> getItemGroups()
	{
		return _itemGroups;
	}


}