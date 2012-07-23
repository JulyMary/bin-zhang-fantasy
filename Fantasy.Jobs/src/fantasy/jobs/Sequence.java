package fantasy.jobs;

import org.jdom2.Element;
import org.jdom2.Namespace;
import fantasy.*;

import fantasy.xserialization.*;

public abstract class Sequence extends AbstractInstruction
{
	@XArray(serializer = InstructionsSerializer.class, items={})
	private java.util.ArrayList<IInstruction> _instructions = new java.util.ArrayList<IInstruction>();

	public final java.util.List<IInstruction> getInstructions()
	{
		return _instructions;
	}

	@XNamespace
	protected Namespace[] _namespaces;

	protected final void ResetSequenceIndex() throws Exception
	{
		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);
		job.getRuntimeStatus().getLocal().setItem("sequence.current", 0);
	}

	protected void ExecuteSequence() throws Exception
	{
		IJob job = (IJob)this.getSite().getRequiredService(IJob.class);
		int index = (int)job.getRuntimeStatus().getLocal().GetValue("sequence.current", 0);
		while (index < this.getInstructions().size())
		{
			if(Thread.interrupted())
			{
				throw new InterruptedException();
			}
			job.ExecuteInstruction(this.getInstructions().get(index));

			index++;
			job.getRuntimeStatus().getLocal().setItem("sequence.current", index);

		}
	}

	@SuppressWarnings("rawtypes") 
	private static class InstructionsSerializer implements IXCollectionSerializer
	{


		public final void Save(IServiceProvider context, Element element,Iterable collection) throws Exception
		{
			for (Object inst : collection)
			{
				java.lang.Class t = inst.getClass();
				XSerializer tempVar = new XSerializer(t);
				tempVar.setContext(context);
				XSerializer ser = tempVar;
				Element childElement;
				if (t != ExecuteTaskInstruction.class)
				{
					childElement = ser.serialize(inst);
				}
				else
				{
					ExecuteTaskInstruction taskInst = (ExecuteTaskInstruction)inst;
					childElement = new Element(taskInst.getTaskName(), Namespace.getNamespace(taskInst.getTaskNamespaceUri()));
					ser.serialize(childElement, taskInst);
				}

				element.addContent(childElement);
			}
		}

		public final Iterable Load(IServiceProvider context, Element element) throws Exception
		{
			IJob job = context.getService(IJob.class);
			java.util.ArrayList<Object> rs = new java.util.ArrayList<Object>();
			for (Element childElement : element.getChildren())
			{
				java.lang.Class t = job.ResolveInstructionType(childElement.getNamespaceURI(), childElement.getName());
				if (ITask.class.isAssignableFrom(t))
				{
					t = ExecuteTaskInstruction.class;
				}
				XSerializer ser = new XSerializer(t);
				ser.setContext(context);

				Object inst = ser.deserialize(childElement);
				rs.add(inst);
			}

			return rs;

		}

	}
}