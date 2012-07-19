package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.jobs.Properties.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("choose", NamespaceUri = Consts.XNamespaceURI)]
public class Choose extends AbstractInstruction
{
	@Override
	public void Execute()
	{
		IJob job = (IJob)this.getSite().getService(Job.class);
		IConditionService conditionService = (IConditionService)this.getSite().getService(IConditionService.class);
		When chose = null;
		int index = (int)job.getRuntimeStatus().getLocal().GetValue("chose", -1);
		if (index == -1)
		{
			for (int i = 0; i < this._cases.size(); i++)
			{
				if (!String.IsNullOrWhiteSpace(this._cases.get(i).Condition))
				{
					if (conditionService.Evaluate(this._cases.get(i).Condition))
					{
						job.getRuntimeStatus().getLocal().setItem("chose", i);
						chose = this._cases.get(i);
						break;
					}
				}
				else
				{
					throw new JobException(Properties.Resources.getWhenRequireConditionText());
				}
			}

			if (chose == null && this.Otherwise != null)
			{
				job.getRuntimeStatus().getLocal().setItem("chose", Integer.MAX_VALUE);
				chose = this.Otherwise;
			}

		}
		else if (index == Integer.MAX_VALUE)
		{
			chose = this.Otherwise;
		}
		else
		{
			chose = this._cases.get(index);
		}

		if (chose != null)
		{
			job.ExecuteInstruction(chose);
		}


	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Order = 10), XArrayItem(Name = "when", java.lang.Class = typeof(When))]
	private java.util.List<When> _cases = new java.util.ArrayList<When>();




//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XElement("otherwise", Order = 20)]
	public When Otherwise = null;

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning disable 169
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XNamespace]
	private XmlNamespaceManager _namespaces;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 169

}