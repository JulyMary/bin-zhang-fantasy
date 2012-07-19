package fantasy.jobs;

import fantasy.xserialization.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("when", NamespaceUri=Consts.XNamespaceURI)]
public class When extends Sequence
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("condition")]
	public String Condition = null;

	@Override
	public void Execute()
	{
		this.ExecuteSequence();
	}
}