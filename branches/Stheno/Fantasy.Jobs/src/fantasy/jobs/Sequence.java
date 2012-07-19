package fantasy.jobs;

import fantasy.xserialization.*;

public abstract class Sequence extends AbstractInstruction
{
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Serializer = typeof(InstructionsSerializer))]
	private java.util.ArrayList<IInstruction> _instructions = new java.util.ArrayList<IInstruction>();

	public final java.util.List<IInstruction> getInstructions()
	{
		return _instructions;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning disable 169
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XNamespace]
	protected XmlNamespaceManager _namespaces;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 169

	protected final void ResetSequenceIndex()
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		job.getRuntimeStatus().getLocal().setItem("sequence.current", 0);
	}

	protected void ExecuteSequence()
	{
		IJob job = (IJob)this.Site.GetService(IJob.class);
		int index = (int)job.getRuntimeStatus().getLocal().GetValue("sequence.current", 0);
		while (index < this.getInstructions().size())
		{
			job.ExecuteInstruction(this.getInstructions().get(index));

			index++;
			job.getRuntimeStatus().getLocal().setItem("sequence.current", index);

		}
	}

	private static class InstructionsSerializer extends IXCollectionSerializer
	{
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXCollectionSerializer Members

	public final void Save(IServiceProvider context, XElement element, Iterable collection)
	{
		for (Object inst : collection)
		{
			java.lang.Class t = inst.getClass();
			XSerializer tempVar = new XSerializer(t);
			tempVar.Context = context;
			XSerializer ser = tempVar;
			XElement childElement;
			if (t != ExecuteTaskInstruction.class)
			{
				childElement = ser.Serialize(inst);
			}
			else
			{
				ExecuteTaskInstruction taskInst = (ExecuteTaskInstruction)inst;
				//string prefix = element.GetPrefixOfNamespace(taskInst.TaskNamespaceUri);
				childElement = new XElement((XNamespace)taskInst.getTaskNamespaceUri() + taskInst.getTaskName());
				ser.Serialize(childElement, taskInst);
			}

			element.Add(childElement);
		}
	}

	public final Iterable Load(IServiceProvider context, XElement element)
	{
		IJob job = (IJob)context.GetService(IJob.class);
		java.util.ArrayList rs = new java.util.ArrayList();
		for (XElement childElement : element.Elements())
		{
			java.lang.Class t = job.ResolveInstructionType(childElement.getName());
			if (Array.indexOf(t.GetInterfaces(), IInstruction.class) < 0)
			{
				t = ExecuteTaskInstruction.class;
			}
			XSerializer tempVar = new XSerializer(t);
			tempVar.Context = context;
			XSerializer ser = tempVar;
			Object inst = ser.Deserialize(childElement);
			rs.add(inst);
		}

		return rs;

	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}
}