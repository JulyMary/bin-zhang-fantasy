package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("until-success", NamespaceUri = Consts.XNamespaceURI)]
public class UntilSuccess extends AbstractInstruction
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("failIfAllSkipped")]
	private boolean _failIfAllSkipped = true;



	@Override
	public void Execute()
	{
		if (this._items.size() > 0)
		{
			IConditionService conditionSvc = this.getSite().<IConditionService>GetRequiredService();
			IJob job = this.getSite().<IJob>GetRequiredService();
			ILogger logger = this.getSite().<ILogger>GetService();
			int index = job.getRuntimeStatus().getLocal().GetValue("until-success.index", 0);
			boolean success = false;
			boolean hasException = false;
			while (index < this._items.size() && !success)
			{
				try
				{
					Try chance = this._items.get(index);
					if (conditionSvc.Evaluate(chance))
					{
						if (logger != null)
						{
							logger.LogMessage(LogCategories.getInstruction(), "Execute try No.{0}", index);
						}
						job.ExecuteInstruction(chance);
						success = true;
					}
					else
					{
						logger.LogMessage(LogCategories.getInstruction(), "Skip try No.{0}", index);
					}

				}
				catch (ThreadAbortException e)
				{

				}
				catch (RuntimeException error)
				{
					hasException = true;
					job.getRuntimeStatus().getLocal().setItem("until-success.index", index);
					if (logger != null)
					{
						logger.LogError(LogCategories.getInstruction(), error, "try instruction faild, try next one.");

					}
				}
				index++;
			}


			if (!success)
			{


				if (hasException)
				{
					if (logger != null)
					{
						logger.LogError(LogCategories.getInstruction(), Properties.Resources.getUnitlSuccessFailedText());
					}
					throw new JobException(Properties.Resources.getUnitlSuccessFailedText());
				}
				else
				{
					if (logger != null)
					{
						logger.LogError(LogCategories.getInstruction(), Properties.Resources.getUntilSuccessAllSkippedText());

					}
					throw new JobException(Properties.Resources.getUntilSuccessAllSkippedText());
				}
			}

		}
	}
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Order = 10), XArrayItem(Name = "try", java.lang.Class = typeof(Try))]
	private java.util.List<Try> _items = new java.util.ArrayList<Try>();


}