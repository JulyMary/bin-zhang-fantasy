package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.servicemodel.*;
import fantasy.jobs.properties.*;

@Instruction
@XSerializable(name = "until-success", namespaceUri = Consts.XNamespaceURI)
public class UntilSuccess extends AbstractInstruction
{
	@XAttribute(name = "failIfAllSkipped")
	private boolean _failIfAllSkipped = true;



	@Override
	public void Execute() throws Exception
	{
		if (this._items.size() > 0)
		{
			IConditionService conditionSvc = this.getSite().getRequiredService(IConditionService.class);
			IJob job = this.getSite().getRequiredService(IJob.class);
			ILogger logger = this.getSite().getService(ILogger.class);
			int index = job.getRuntimeStatus().getLocal().GetValue("until-success.index", 0);
			boolean success = false;
			boolean hasException = false;
			while (index < this._items.size() && !success)
			{
				if(Thread.interrupted())
				{
					throw new InterruptedException();
				}
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
						Log.SafeLogMessage(logger, LogCategories.getInstruction(), "Skip try No.{0}", index);
					}

				}
				catch (InterruptedException e)
				{
                     throw e;
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
						logger.LogError(LogCategories.getInstruction(), Resources.getUnitlSuccessFailedText());
					}
					throw new JobException(Resources.getUnitlSuccessFailedText());
				}
				else
				{
					if (logger != null)
					{
						logger.LogError(LogCategories.getInstruction(), Resources.getUntilSuccessAllSkippedText());

					}
					throw new JobException(Resources.getUntilSuccessAllSkippedText());
				}
			}

		}
	}

	
	
	@XArray(order = 10, items=@XArrayItem(name = "try", type = Try.class))
	private java.util.List<Try> _items = new java.util.ArrayList<Try>();


}