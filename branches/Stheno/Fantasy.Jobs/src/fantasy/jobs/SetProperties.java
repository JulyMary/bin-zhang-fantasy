package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.jobs.properties.Resources;
import fantasy.servicemodel.*;

@Instruction
@XSerializable(name = "properties", namespaceUri = Consts.XNamespaceURI)
public class SetProperties extends AbstractInstruction implements IConditionalObject
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	@XArray(serializer = JobPropertiesSerializer.class, items={})
	private java.util.ArrayList<JobProperty> _list = new java.util.ArrayList<JobProperty>();

	@Override
	public void Execute() throws Exception
	{
		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);
		ILogger logger = this.getSite().getService(ILogger.class);
		IJob job = this.getSite().getRequiredService(IJob.class);
		IConditionService conditionSvc = this.getSite().getRequiredService(IConditionService.class);
		if (conditionSvc.Evaluate(this))
		{
			
			int index = job.getRuntimeStatus().getLocal().GetValue("setproperties.index", 0);
			while (index < _list.size())
			{
				JobProperty prop = this._list.get(index);
				if (conditionSvc.Evaluate(prop))
				{
					String value = parser.Parse(prop.getValue());
					job.SetProperty(prop.getName(), value);
					if (logger != null)
					{
						logger.LogMessage(LogCategories.getProperty(), MessageImportance.Low, Resources.getSetPropertyMessage(), prop.getName(), value);
					}
				}
				index++;
				job.getRuntimeStatus().getLocal().setItem("setproperties.index", index);
				

			}
		}

	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
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