package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("parameters", NamespaceUri = Consts.XNamespaceURI)]
public class CallTemplateParameter implements IConditionalObject
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("itemCategory")]
	public String ItemCategory = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("include")]
	public String Include = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String _condition = null;
	public String getCondition()
	{
		return this._condition;
	}
	public void setCondition(String value)
	{
		this._condition = value;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("propertyName")]
	public String PropertyName = null;
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("value")]
	public String Value = null;
}