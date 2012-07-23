package fantasy.jobs;

import fantasy.xserialization.*;

@XSerializable(name = "output", namespaceUri=Consts.XNamespaceURI)
public class TaskOutput implements IConditionalObject
{

	@XAttribute(name = "taskParameter")
	private String privateTaskParameter;
	public final String getTaskParameter()
	{
		return privateTaskParameter;
	}
	public final void setTaskParameter(String value)
	{
		privateTaskParameter = value;
	}

	@XAttribute(name = "propertyName")
	private String privatePropertyName;
	public final String getPropertyName()
	{
		return privatePropertyName;
	}
	public final void setPropertyName(String value)
	{
		privatePropertyName = value;
	}

	@XAttribute(name = "itemCategory")
	private String privateItemCategory;
	public final String getItemCategory()
	{
		return privateItemCategory;
	}
	public final void setItemCategory(String value)
	{
		privateItemCategory = value;
	}

	@XAttribute(name = "condition")
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