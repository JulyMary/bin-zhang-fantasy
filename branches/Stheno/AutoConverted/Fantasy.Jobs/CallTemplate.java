package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.ServiceModel.*;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[Instruction, XSerializable("callTemplate", NamespaceUri = Consts.XNamespaceURI)]
public class CallTemplate extends AbstractInstruction implements IConditionalObject, IXSerializable
{

	@Override
	public void Execute()
	{
		IConditionService conditionSvc = (IConditionService)this.Site.GetService(IConditionService.class);
		ILogger logger = (ILogger)this.Site.GetService(ILogger.class);
		IJobEngine engine = this.getSite().<IJobEngine>GetRequiredService();

		if (conditionSvc.Evaluate(this))
		{
			IJob parentJob = this.getSite().<IJob>GetRequiredService();

			ServiceContainer childSite = new ServiceContainer();

			Job tempVar = new Job();
			tempVar.setIsNested(true);
			_job = tempVar;
			_job._id = parentJob.getID();

			java.util.ArrayList<Object> services = new java.util.ArrayList<Object>();
			services.addAll(AddIn.CreateObjects("jobEngine/job.services/service"));
			services.add(_job);

			childSite.InitializeServices(this.getSite(), services.toArray(new Object[]{}));

			_job.Initialize();
			if (_jobElement == null || !parentJob.getRuntimeStatus().getLocal().GetValue("jobinitialized", false))
			{
				JobStartInfo si = new JobStartInfo();
				si.setID(engine.getJobId());
				si.setName(parentJob.GetProperty("Name"));
				si.setTemplate(this.Template);
				si.setTarget(this.Target);

				AddInputs(si);

				_job.LoadStartInfo(si);
				parentJob.getRuntimeStatus().getLocal().setItem("jobinitialized", true);
				engine.SaveStatus();
			}
			else
			{
				_job.LoadStatus(_jobElement);
			}

			((IJob)_job).Execute();


			SetOutputParameters();

			childSite.UninitializeServices();
			//clean jobs so we do not serialize it anymore.
			_job = null;
			_jobElement = null;
		}
		else if (logger != null)
		{
			logger.LogMessage(LogCategories.getInstruction(), "Skip callTemplate {0}", this.Template);
		}

	}

	private void SetOutputParameters()
	{
		IItemParser itemParser = this._job.getSite().<IItemParser>GetRequiredService();
		IStringParser strParser = this._job.getSite().<IStringParser>GetRequiredService();
		IJob parentJob = this.getSite().<IJob>GetRequiredService();
		TaskItemGroup group = null;
		for (CallTemplateParameter output : this._outputs)
		{
			if (!String.IsNullOrWhiteSpace(output.ItemCategory))
			{
				TaskItem[] srcItems = itemParser.ParseItem(output.Include);
				for (TaskItem srcItem : srcItems)
				{
					if(group == null)
					{
						group = parentJob.AddTaskItemGroup();
					}
					TaskItem item = group.AddNewItem(srcItem.getName(), output.ItemCategory);
					srcItem.CopyMetaDataTo(item);
				}

			}
			else if (!String.IsNullOrWhiteSpace(output.PropertyName))
			{
				String value = strParser.Parse(output.getValue());
				parentJob.SetProperty(output.PropertyName, value);
			}
			else
			{
				throw new JobException(Fantasy.Jobs.Properties.Resources.getInvalidOutputText());
			}
		}
	}

	private void AddInputs(JobStartInfo si)
	{
		IItemParser itemParser = this.getSite().<IItemParser>GetRequiredService();
		IStringParser strParser = this.getSite().<IStringParser>GetRequiredService();
		TaskItemGroup itemGroup = new TaskItemGroup();
		IConditionService conditionSvc = this.getSite().<IConditionService>GetRequiredService();
		si.getItemGroups().add(itemGroup);

		JobPropertyGroup propGroup = new JobPropertyGroup();
		si.getPropertyGroups().add(propGroup);

		for (CallTemplateParameter input : _inputs)
		{
			if (conditionSvc.Evaluate(input))
			{
				if (!String.IsNullOrWhiteSpace(input.ItemCategory))
				{
					TaskItem[] srcItems = itemParser.ParseItem(input.Include);
					for (TaskItem srcItem : srcItems)
					{
						TaskItem item = itemGroup.AddNewItem(srcItem.getName(), input.ItemCategory);
						srcItem.CopyMetaDataTo(item);
					}
				}
				else if (!String.IsNullOrWhiteSpace(input.PropertyName))
				{
					String value = strParser.Parse(input.getValue());
					JobProperty prop = propGroup.AddNewItem(input.PropertyName);
					prop.setValue(value);
				}
				else
				{
					throw new JobException(Fantasy.Jobs.Properties.Resources.getInvalidCallTemplateInputText());
				}
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("template")]
	public String Template = null;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("target")]
	public String Target = null;


//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray, XArrayItem(Name = "input", java.lang.Class = typeof(CallTemplateParameter))]
	private java.util.List<CallTemplateParameter> _inputs = new java.util.ArrayList<CallTemplateParameter>();



//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray, XArrayItem(Name = "output", java.lang.Class = typeof(CallTemplateParameter))]
	private java.util.List<CallTemplateParameter> _outputs = new java.util.ArrayList<CallTemplateParameter>();

	public final java.util.List<CallTemplateParameter> getOutputs()
	{
		return _outputs;
	}

	private Job _job = null;
	private XElement _jobElement = null;


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

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma  warning disable 169
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XNamespace]
	private XmlNamespaceManager _namespaces;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 169

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	public final void Load(IServiceProvider context, XElement element)
	{
		XHelper.Default.LoadByXAttributes(context, element, this);
		XName name = (XNamespace)Consts.XNamespaceURI + "job";
		_jobElement = element.Element(name);
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		XHelper.Default.SaveByXAttributes(context, element, this);
		if (_job != null)
		{
			_jobElement = _job.SaveStatus();
		}
		if (_jobElement != null)
		{
			element.Add(_jobElement);
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion
}