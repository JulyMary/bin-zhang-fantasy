package fantasy.jobs;

import fantasy.xserialization.*;

@XSerializable(name = "parameters", namespaceUri = Consts.XNamespaceURI)
public class CallTemplateParameter implements IConditionalObject
{
	@XAttribute(name = "itemCategory")
	public String ItemCategory = null;

	@XAttribute(name = "include")
	public String Include = null;

    @XAttribute(name = "condition")
	private String _condition = null;
	public String getCondition()
	{
		return this._condition;
	}
	public void setCondition(String value)
	{
		this._condition = value;
	}

	@XAttribute(name = "propertyName")
	public String PropertyName = null;
	
	@XAttribute(name = "value")
	public String Value = null;
}