package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("try", NamespaceUri = Consts.XNamespaceURI)]
public class Try extends Sequence implements IConditionalObject
{
	@Override
	public void Execute()
	{
		super.ExecuteSequence();
	}


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	private String _condition = null;
	private String getCondition()
	{
		return this._condition;
	}
	private void setCondition(String value)
	{
		this._condition = value;
	}

}