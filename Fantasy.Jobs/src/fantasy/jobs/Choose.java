package fantasy.jobs;

import org.jdom2.*;

import fantasy.xserialization.*;
import fantasy.jobs.properties.*;
import fantasy.*;

@Instruction
@XSerializable(name = "choose", namespaceUri = Consts.XNamespaceURI)
public class Choose extends AbstractInstruction
{
	@Override
	public void Execute() throws Exception
	{
		IJob job = (IJob)this.getSite().getService2(Job.class);
		IConditionService conditionService = (IConditionService)this.getSite().getService2(IConditionService.class);
		When chose = null;
		int index = (int)job.getRuntimeStatus().getLocal().GetValue("chose", -1);
		if (index == -1)
		{
			for (int i = 0; i < this._cases.size(); i++)
			{
				if (!StringUtils2.isNullOrWhiteSpace(this._cases.get(i).Condition))
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
					throw new JobException(Resources.getWhenRequireConditionText());
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

	@XArray(order = 10, items=@XArrayItem(name = "when", type = When.class))
	private java.util.List<When> _cases = new java.util.ArrayList<When>();




	@XElement(name = "otherwise", order = 20)
	public When Otherwise = null;


	@XNamespace
	private Namespace[] _namespaces;


}