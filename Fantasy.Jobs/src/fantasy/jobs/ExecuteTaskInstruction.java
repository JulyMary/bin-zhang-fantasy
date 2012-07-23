package fantasy.jobs;

import java.lang.reflect.*;
import java.util.*;

import fantasy.xserialization.*;
import fantasy.jobs.properties.*;
import fantasy.servicemodel.*;
//import fantasy.jobs.resources.*;
import fantasy.collections.*;
import fantasy.io.*;
import fantasy.*;
import org.jdom2.*;


@SuppressWarnings("rawtypes")
@XSerializable(name = "executeTask", namespaceUri= Consts.XNamespaceURI)
public class ExecuteTaskInstruction extends AbstractInstruction implements IConditionalObject, IXSerializable
{
	public ExecuteTaskInstruction()
	{

	}

	private String privateTaskName;
	public final String getTaskName()
	{
		return privateTaskName;
	}
	public final void setTaskName(String value)
	{
		privateTaskName = value;
	}
	private String privateTaskNamespaceUri;
	public final String getTaskNamespaceUri()
	{
		return privateTaskNamespaceUri;
	}
	public final void setTaskNamespaceUri(String value)
	{
		privateTaskNamespaceUri = value;
	}

	private java.util.HashMap<String, String> _properties = new java.util.HashMap<String, String>();

	private java.util.ArrayList<Element> _inlineElements = new java.util.ArrayList<Element>();

	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}


	public final void Load(IServiceProvider context, Element element) throws Exception
	{
		this.setTaskName(element.getName());
		this.setTaskNamespaceUri(element.getNamespaceURI());

		this._xmlNamespaceManager = new NamespaceManager();
		this._xmlNamespaceManager.setNamespaces(element.getNamespacesInScope().toArray(new Namespace[0]));

		for (Attribute node : element.getAttributes())
		{
			if (!StringUtils2.isNullOrEmpty(node.getValue()))
			{

				if (node.getName() == "condition" )
				{
						this.setCondition(node.getValue());
				}
				else
				{
				    this._properties.put(node.getName(), node.getValue());
				}
			}
		}

		XSerializer ser = new XSerializer(TaskOutput.class);

		for (Element child : element.getChildren())
		{

			if (child.getName() == "output" && child.getNamespaceURI() == Consts.XNamespaceURI)
			{
				TaskOutput output = (TaskOutput)ser.deserialize(child);
				this.getTaskOutputs().add(output);
			}
			else
			{
				this._inlineElements.add(child);
			}
		}

	}


	private NamespaceManager _xmlNamespaceManager;



	private java.util.List<TaskOutput> _taskOutputs = new java.util.ArrayList<TaskOutput>();

	public final java.util.List<TaskOutput> getTaskOutputs()
	{
		return _taskOutputs;
	}

	public final void Save(IServiceProvider context, Element element) throws Exception
	{
		XHelper.AddNamespace(this._xmlNamespaceManager.getNamespaces(),element);

		if (!StringUtils2.isNullOrEmpty(this.getCondition()))
		{
			element.setAttribute("condition", this.getCondition());

		}

		for (java.util.Map.Entry<String, String> pair : this._properties.entrySet())
		{
			element.setAttribute(pair.getKey(), pair.getValue());
		}

		XSerializer ser = new XSerializer(TaskOutput.class);

		for (Element inline : this._inlineElements)
		{
			Element newNode = inline.clone();
			element.addContent(newNode);
		}

		for (TaskOutput output : this.getTaskOutputs())
		{
			Element child = ser.serialize(output);
			element.addContent(child);
		}

	}

	@Override
	public void Execute() throws Exception
	{
		//TODO: implement resource service
//		IResourceService resSvc = this.getSite().<IResourceService>GetService();
//		if (resSvc != null)
//		{

//			ResourceParameter res = new ResourceParameter("RunTask", new { taskname = this.getTaskName(), namespace = this.getTaskNamespaceUri() });
////			using (IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res }))
//			IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res });
//			try
//			{
//				InnerExecute();
//			}
//			finally
//			{
//				handle.dispose();
//			}
//		}
//		else
		{
			InnerExecute();
		}
	}

	private void InnerExecute() throws  Exception
	{
		IConditionService conditionSvc = this.getSite().getService(IConditionService.class);
		ILogger logger = this.getSite().getService(ILogger.class);

		if (conditionSvc.Evaluate(this))
		{
			logger.LogMessage(LogCategories.getInstruction(), "Execute task {0}", this.getTaskName());
			IJob job = this.getSite().getService(IJob.class);
			java.lang.Class t = job.ResolveInstructionType(this.getTaskNamespaceUri() , this.getTaskName());
			ITask task = (ITask)t.newInstance();
			ServiceContainer taskSite = new ServiceContainer();
			taskSite.initializeServices(this.getSite(), new Object[]{this, this._xmlNamespaceManager} );

			task.setSite(taskSite);

			this.SetInputParams(task);
			try
			{
				boolean success = task.Execute();

				if (!success)
				{
					throw new TaskFailedException(String.format(Resources.getTaskFailedText(), this.getTaskName()));
				}
			}
			finally
			{
				IStatusBarService statusBar = this.getSite().getService(IStatusBarService.class);
				if (statusBar != null)
				{
					statusBar.setStatus("");
				}
			}
			this.SetOutputParams(task);
		}
		else if(logger != null)
		{
			logger.LogMessage(LogCategories.getInstruction(), "Skip task {0}", this.getTaskName());
		}
	}


	private void SetOutputParams(ITask task) throws Exception
	{

		java.lang.Class taskType = task.getClass();
		IConditionService condition = (IConditionService)this.getSite().getService2(IConditionService.class);

		Enumerable<Field> fields = new Enumerable<Field>(taskType.getFields());
		
		for (final TaskOutput output : this.getTaskOutputs())
		{
			
			if(condition.Evaluate(output))
			{

				
				Field field = fields.singleOrDefault(new Predicate<Field>(){

					@Override
					public boolean evaluate(Field field) {
						
						
						TaskMember anno = field.getAnnotation(TaskMember.class);
						return (anno.name() == output.getTaskParameter()) 
								&& Arrays.asList(anno.flags()).indexOf(TaskMemberFlags.Output) >= 0;
					}});
				
			
			if(field != null)
			{
				
				if (!StringUtils2.isNullOrEmpty(output.getItemCategory()))
				{
					CreateTaskItems(task, output, field);
				}
				else
				{
					CreateJobProperty(task, output, field);
				}
			}
			else
			{
				throw new JobException(String.format(Resources.getUndefinedTaskOutputParameterText(), this.getTaskName(), output.getTaskParameter()));
			}
			}

		}
	}

	
	private void appendOutputText(StringBuilder text, Object o) throws Exception
	{
		if (o != null)
		{
			String s;
			if (o instanceof TaskItem)
			{
				s = ((TaskItem)o).getName();
			}
			else
			{
				ITypeConverter cvt = XHelper.getDefault().CreateXConverter(o.getClass());
				s = (String)cvt.convertFrom(o);
			}
			if (!StringUtils2.isNullOrEmpty(s))
			{
				if (text.length() > 0)
				{
					text.append(";");
				}
				text.append(s);
			}
		}
	}
	
	
	private void CreateJobProperty(ITask task, TaskOutput output, Field field) throws Exception
	{
		java.lang.Class memberType = this.GetMemberInfoType(field);
		StringBuilder text = new StringBuilder();


		Object value = this.GetMemberInfoValue(field, task);
		if (value != null)
		{
			if (memberType.isArray())
			{
				
				for (Object ele : Arrays.asList(value))
				{
					appendOutputText(text, ele);
				}
			}
			else
			{
				appendOutputText(text, value);
			}
		}

		IJob job = (IJob)this.getSite().getService2(IJob.class);
		job.SetProperty(output.getPropertyName(), text.toString());

	}

	@SuppressWarnings("unchecked")
	private void CreateTaskItems(ITask task, TaskOutput output, Field field) throws Exception
	{
		
		TaskItem[] items = null;
		
		Object value = this.GetMemberInfoValue(field, task);
		if(value == null)
		{
			return;
		}
		
		if (value instanceof TaskItem)
		{
				items = new TaskItem[] { (TaskItem)value };
		}
		else if (value instanceof TaskItem[])
		{
			items = (TaskItem[])value;
			
		}
		else if (value instanceof String || value instanceof String[] || value instanceof Iterable)
		{
			
				Iterable paths;
				if (value instanceof String)
				{
					paths = new Enumerable<String>(StringUtils2.split((String)value, ";", true));
				}
				else if(value instanceof String[])
				{
					paths = new Enumerable<String>((String[])value);
				}
				else
				{
					paths = (Iterable<String>)value;
				}

				IJobEngine engine = this.getSite().getRequiredService(IJobEngine.class);
				java.util.ArrayList<TaskItem> list = new java.util.ArrayList<TaskItem>();
				for (Object o : paths)
				{
					
					if(o != null)
					{
						String path = o instanceof String ? (String)o : o.toString();
						String name;
						if (Path.isPathRooted(path))
						{
							name = Path.getRelativePath(engine.getJobDirectory(), path);
						}
						else
						{
							name = path;
						}
						TaskItem tempVar = new TaskItem();
						tempVar.setName(name);
						list.add(tempVar);
					}
				}
				items = list.toArray(new TaskItem[]{});
			
		}
		else
		{

			throw new JobException(String.format(Resources.getCannotConverTaskOutputParameterToTaskItemText(), output.getTaskParameter(), this.getTaskName()));
		}

		if (items != null && items.length > 0)
		{

			IJob job = this.getSite().getService(IJob.class);
			TaskItemGroup group = job.AddTaskItemGroup();
			for (TaskItem item : items)
			{
				TaskItem newItem = group.AddNewItem(item.getName(), output.getItemCategory());
				newItem.setCondition(item.getCondition());
				item.CopyMetaDataTo(newItem);
			}
		}


	}



	private void SetInputParams(ITask task) throws Exception
	{
		java.lang.Class taskType = task.getClass();

		
		Enumerable<Field> fields = new Enumerable<Field>(taskType.getFields()).where(new Predicate<Field>(){
			
			@Override
			public boolean evaluate(Field f)
			{
				return f.isAnnotationPresent(TaskMember.class) && Arrays.asList(f.getAnnotation(TaskMember.class).flags()).indexOf(TaskMemberFlags.Input) >= 0;
			}
		});
		

		for (Field field : fields)
		{
			
			TaskMember anno = field.getAnnotation(TaskMember.class);
			if (this._properties.containsKey(anno.name()))
			{
				try
				{
					java.lang.Class memberType = this.GetMemberInfoType(field);

					if (memberType == TaskItem.class)
					{
						SetTaskItemValue(task, field, anno.name());
					}
					else if (memberType == TaskItem[].class)
					{
						SetTaskItemsValue(task, field, anno.name());
					}
					else if (memberType.isArray())
					{
						SetArrayValue(task, field, anno.name());
					}
					else
					{
						SetScalarValue(task, field, anno.name());
					}
				}
				catch (InterruptedException  e)
				{
					throw e;
				}
				catch (Exception error)
				{
					throw new JobException(String.format(Resources.getSetTaskParamErrorText(), this.getTaskName(), anno.name()), error);
				}
			}
			else if (Arrays.asList(anno.flags()).indexOf(TaskMemberFlags.Inline) >= 0)
			{
				java.lang.Class memberType = this.GetMemberInfoType(field);
				if (memberType == Element.class)
				{
					this.SetInlineValue(task, field, anno.name(), anno.parseInline());
				}
				else
				{
					throw new JobException(String.format(Resources.getInlineTaskMemberMustBeElementText(), this.getTaskName(), anno.name()));
				}
			}
			else if (Arrays.asList(anno.flags()).indexOf(TaskMemberFlags.Required) >= 0)
			{
				throw new JobException(String.format(Resources.getRequireAttrubiteOfTaskText(), anno.name(), this.getTaskName()));
			}
		}
	}

	private void SetInlineValue(ITask task, Field mi, final String elementName, boolean parseInline) throws Exception
	{
		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);

		Element element = new Enumerable<Element>(this._inlineElements).firstOrDefault(new Predicate<Element>(){

			@Override
			public boolean evaluate(Element ele) {
				return ele.getName().toLowerCase() == elementName.toLowerCase();
			}});
		
		if(element != null)
		{
			Element val = parseInline ? StringParserUtils.Parse(parser, element, null) : element.clone();
			this.SetMemberInfoValue(mi, task, val);
		}
		
	}


	private void SetScalarValue(ITask task, Field mi, String param) throws Exception
	{
		IStringParser parser = this.getSite().getRequiredService(IStringParser.class);
		String str = parser.Parse(this._properties.get(param));
		if (!StringUtils2.isNullOrEmpty(str))
		{
			ITypeConverter cvt = XHelper.getDefault().CreateXConverter(this.GetMemberInfoType(mi));
			Object value = cvt.convertTo(str, this.GetMemberInfoType(mi));
			this.SetMemberInfoValue(mi, task, value);
		}
	}

	private void SetArrayValue(ITask task, Field mi, String param) throws Exception
	{

		java.lang.Class memberType = this.GetMemberInfoType(mi);
		java.lang.Class elementType = memberType.getComponentType();
		
		IStringParser parser = (IStringParser)this.getSite().getRequiredService(IStringParser.class);
		String str = parser.Parse(this._properties.get(param));
		if (!StringUtils2.isNullOrEmpty(str))
		{

			String[] strArr = StringUtils2.split(str, ";", true);
			Object arr = Array.newInstance(elementType, strArr.length);
			ITypeConverter cvt = XHelper.getDefault().CreateXConverter(elementType);
			for (int i = 0; i < Array.getLength(arr); i++)
			{
				Object value = cvt.convertTo(strArr[i], elementType);
				Array.set(arr, i, value);
				
			}

			this.SetMemberInfoValue(mi, task, arr);
		}
	}



	private void SetTaskItemsValue(ITask task, Field mi, String param) throws Exception
	{
		IItemParser parser = this.getSite().getRequiredService(IItemParser.class);
		TaskItem[] items = parser.ParseItem(this._properties.get(param));
		this.SetMemberInfoValue(mi, task, items);
	}

	private void SetTaskItemValue(ITask task, Field mi, String param) throws Exception
	{
		IItemParser parser = this.getSite().getRequiredService(IItemParser.class);
		TaskItem[] items = parser.ParseItem(this._properties.get(param));
		if (items.length == 1)
		{
			this.SetMemberInfoValue(mi, task, items[0]);
		}
		else if (items.length > 1)
		{
			throw new JobException(String.format(Resources.getInputParamContainsMultipleItemsText(), param, this.getTaskName(), items.length));
		}
	}

	private java.lang.Class GetMemberInfoType(Field mi)
	{
		return mi.getType();
	}

	private void SetMemberInfoValue(Field mi, Object instance, Object value) throws Exception
	{
		mi.set(instance, value);
	}

	private Object GetMemberInfoValue(Field mi, Object instance) throws Exception
	{
		
		return mi.get(instance);
		
	}
}