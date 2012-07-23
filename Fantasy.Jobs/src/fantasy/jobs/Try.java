package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
@Instruction
@XSerializable(name = "try", namespaceUri = Consts.XNamespaceURI)
public class Try extends Sequence implements IConditionalObject
{
	@Override
	public void Execute() throws Exception
	{
		this.ExecuteSequence();
	}

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

}