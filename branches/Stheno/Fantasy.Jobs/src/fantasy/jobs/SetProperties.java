package fantasy.jobs;

import fantasy.xserialization.*;
import fantasy.servicemodel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("properties", NamespaceUri = Consts.XNamespaceURI)]
public class SetProperties extends AbstractInstruction implements IConditionalObject
{

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Serializer = typeof(JobPropertiesSerializer))]
	private java.util.ArrayList<JobProperty> _list = new java.util.ArrayList<JobProperty>();

	@Override
	public void Execute()
	{
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		ILogger logger = this.getSite().<ILogger>GetService();
		IJob job = this.getSite().<IJob>GetRequiredService();
		IConditionService conditionSvc = this.getSite().<IConditionService>GetRequiredService();
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
						logger.LogMessage("property", MessageImportance.Low, "set property {0} as {1}", prop.getName(), value);
					}
				}
				index++;
				job.getRuntimeStatus().getLocal().setItem("setproperties.index", index);

			}
		}

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