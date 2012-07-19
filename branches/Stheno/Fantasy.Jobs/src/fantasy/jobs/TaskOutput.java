package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("output", NamespaceUri=Consts.XNamespaceURI)]
public class TaskOutput implements IConditionalObject
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("taskParameter")]
	private String privateTaskParameter;
	public final String getTaskParameter()
	{
		return privateTaskParameter;
	}
	public final void setTaskParameter(String value)
	{
		privateTaskParameter = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("propertyName")]
	private String privatePropertyName;
	public final String getPropertyName()
	{
		return privatePropertyName;
	}
	public final void setPropertyName(String value)
	{
		privatePropertyName = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("itemCategory")]
	private String privateItemCategory;
	public final String getItemCategory()
	{
		return privateItemCategory;
	}
	public final void setItemCategory(String value)
	{
		privateItemCategory = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}
}