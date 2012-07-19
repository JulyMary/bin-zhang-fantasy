package fantasy.jobs;

import java.io.*;
import java.util.*;
import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
@XSerializable(name ="jobStart", namespaceUri=Consts.XNamespaceURI)
public class JobStartInfo implements Serializable
{
	/**
	 * 
	 */
	private static final long serialVersionUID = 682362345444889072L;
	public JobStartInfo()
	{
		
	}

	@XAttribute(name = "id")
	private UUID privateID = UUID.randomUUID();
	public final UUID getID()
	{
		return privateID;
	}
	public final void setID(UUID value)
	{
		privateID = value;
	}

	@XAttribute(name = "target")
	private String privateTarget;
	public final String getTarget()
	{
		return privateTarget;
	}
	public final void setTarget(String value)
	{
		privateTarget = value;
	}

	@XAttribute(name = "template")
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
	@XAttribute(name = "name")
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