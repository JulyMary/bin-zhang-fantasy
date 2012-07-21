package fantasy.jobs;

import java.util.Arrays;

import fantasy.xserialization.*;
import fantasy.servicemodel.*;
import fantasy.*;

import org.jdom2.*;

@Instruction
@XSerializable(name = "callTemplate", namespaceUri = Consts.XNamespaceURI)
public class CallTemplate extends AbstractInstruction implements IConditionalObject, IXSerializable
{

	@Override
	public void Execute() throws Exception
	{
		IConditionService conditionSvc = (IConditionService)this.getSite().getService(IConditionService.class);
		ILogger logger = (ILogger)this.getSite().getService(ILogger.class);
		IJobEngine engine = (IJobEngine)this.getSite().getRequiredService(IJobEngine.class);

		if (conditionSvc.Evaluate(this))
		{
			IJob parentJob = (IJob)this.getSite().getRequiredService(IJob.class);

			ServiceContainer childSite = new ServiceContainer();

			Job tempVar = new Job();
			tempVar.setIsNested(true);
			_job = tempVar;
			_job.setID(parentJob.getID());

			
			java.util.ArrayList<Object> services = new java.util.ArrayList<Object>();
			services.addAll(Arrays.asList(AddIn.CreateObjects("jobEngine/job.services/service")));
			services.add(_job);

			childSite.initializeServices(this.getSite(), services.toArray(new Object[]{}));

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

			childSite.uninitializeServices();
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
		IItemParser itemParser = this._job.getSite().getRequiredService2(IItemParser.class);
		IStringParser strParser = this._job.getSite().getRequiredService2(IStringParser.class);
		IJob parentJob = (IJob)this.getSite().getRequiredService(IJob.class);
		TaskItemGroup group = null;
		for (CallTemplateParameter output : this._outputs)
		{
			if (!StringUtils2.isNullOrWhiteSpace(output.ItemCategory))
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
			else if (!StringUtils2.isNullOrWhiteSpace(output.PropertyName))
			{
				String value = strParser.Parse(output.Value);
				parentJob.SetProperty(output.PropertyName, value);
			}
			else
			{
				throw new JobException(fantasy.jobs.properties.Resources.getInvalidOutputText());
			}
		}
	}

	private void AddInputs(JobStartInfo si) throws Exception
	{
		IItemParser itemParser = this.getSite().getRequiredService2(IItemParser.class);
		IStringParser strParser = this.getSite().getRequiredService2(IStringParser.class);
		TaskItemGroup itemGroup = new TaskItemGroup();
		IConditionService conditionSvc = this.getSite().getRequiredService2(IConditionService.class);
		si.getItemGroups().add(itemGroup);

		JobPropertyGroup propGroup = new JobPropertyGroup();
		si.getPropertyGroups().add(propGroup);

		for (CallTemplateParameter input : _inputs)
		{
			if (conditionSvc.Evaluate(input))
			{
				if (!StringUtils2.isNullOrWhiteSpace(input.ItemCategory))
				{
					TaskItem[] srcItems = itemParser.ParseItem(input.Include);
					for (TaskItem srcItem : srcItems)
					{
						TaskItem item = itemGroup.AddNewItem(srcItem.getName(), input.ItemCategory);
						srcItem.CopyMetaDataTo(item);
					}
				}
				else if (!StringUtils2.isNullOrWhiteSpace(input.PropertyName))
				{
					String value = strParser.Parse(input.Value);
					JobProperty prop = propGroup.AddNewItem(input.PropertyName);
					prop.setValue(value);
				}
				else
				{
					throw new JobException(fantasy.jobs.properties.Resources.getInvalidCallTemplateInputText());
				}
			}
		}
	}

	@XAttribute( name = "template")
	public String Template = null;


	@XAttribute(name = "target")
	public String Target = null;


	@XArray(items=@XArrayItem(name = "input",type = CallTemplateParameter.class))
	private java.util.List<CallTemplateParameter> _inputs = new java.util.ArrayList<CallTemplateParameter>();




	@XArray(items=@XArrayItem(name = "output", type = CallTemplateParameter.class))
	private java.util.List<CallTemplateParameter> _outputs = new java.util.ArrayList<CallTemplateParameter>();

	public final java.util.List<CallTemplateParameter> getOutputs()
	{
		return _outputs;
	}

	private Job _job = null;
	private Element _jobElement = null;

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

	@XNamespace
	private Namespace[] _namespaces;

	@Override
	public final void Load(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().LoadByXAttributes(context, element, this);
		
		_jobElement = element.getChild("job", Namespace.getNamespace(Consts.XNamespaceURI));
	}

	public final void Save(IServiceProvider context, Element element) throws Exception
	{
		XHelper.getDefault().SaveByXAttributes(context, element, this);
		if (_job != null)
		{
			_jobElement = _job.SaveStatus();
		}
		if (_jobElement != null)
		{
			element.addContent(_jobElement);
		}
	}
}