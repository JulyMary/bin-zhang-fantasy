package fantasy.jobs;

import fantasy.xserialization.*;

@Instruction
@XSerializable(name = "when", namespaceUri=Consts.XNamespaceURI)
public class When extends Sequence
{
	@XAttribute(name ="condition")
	public String Condition = null;

	@Override
	public void Execute() throws Exception
	{
		this.ExecuteSequence();
	}
}