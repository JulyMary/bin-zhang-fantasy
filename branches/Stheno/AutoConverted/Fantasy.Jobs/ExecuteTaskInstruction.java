package Fantasy.Jobs;

import Fantasy.XSerialization.*;
import Fantasy.Jobs.Properties.*;
import Fantasy.ServiceModel.*;
import Fantasy.Jobs.Resources.*;
import Fantasy.IO.*;

//public enum TaskExecutionLocation {InProcess, OutProcess}
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
//[XSerializable("executeTask", NamespaceUri= Consts.XNamespaceURI)]
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

	private java.util.ArrayList<XElement> _inlineElements = new java.util.ArrayList<XElement>();

	private String privateCondition;
	public final String getCondition()
	{
		return privateCondition;
	}
	public final void setCondition(String value)
	{
		privateCondition = value;
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IXSerializable Members

	public final void Load(IServiceProvider context, XElement element)
	{
		this.setTaskName(element.getName().LocalName);
		this.setTaskNamespaceUri(element.getName().NamespaceName);

		this._xmlNamespaceManager = XHelper.CreateXmlNamespaceManager(element, true);

		for (XAttribute node : element.Attributes())
		{
			if (!DotNetToJavaStringHelper.isNullOrEmpty(node.getValue()))
			{

//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//				switch (node.Name.LocalName)
//ORIGINAL LINE: case "condition":
				if (node.getName().LocalName.equals("condition"))
				{
						this.setCondition(node.getValue());
				}
				else
				{
						if (!node.IsNamespaceDeclaration)
						{
							this._properties.put(node.getName().LocalName, node.getValue());
						}

				}
			}
		}



		XSerializer ser = new XSerializer(TaskOutput.class);

		XName outputName = (XNamespace)Consts.XNamespaceURI + "output";
		for (XElement child : element.Elements())
		{

			if (child.getName() == outputName)
			{
				TaskOutput output = (TaskOutput)ser.Deserialize(child);
				this.getTaskOutputs().add(output);
			}
			else
			{
				this._inlineElements.add(child);
			}
		}

	}


	private XmlNamespaceManager _xmlNamespaceManager;



	private java.util.List<TaskOutput> _taskOutputs = new java.util.ArrayList<TaskOutput>();

	public final java.util.List<TaskOutput> getTaskOutputs()
	{
		return _taskOutputs;
	}

	public final void Save(IServiceProvider context, XElement element)
	{
		XHelper.AddNamespace(this._xmlNamespaceManager,element);

		if (!DotNetToJavaStringHelper.isNullOrEmpty(this.getCondition()))
		{
			element.SetAttributeValue("condition", this.getCondition());

		}

		for (java.util.Map.Entry<String, String> pair : this._properties.entrySet())
		{
			element.SetAttributeValue(pair.getKey(), pair.getValue());
		}

		XSerializer ser = new XSerializer(TaskOutput.class);

		for (XElement inline : this._inlineElements)
		{
			XElement newNode = new XElement(inline);
			element.Add(newNode);
		}

		for (TaskOutput output : this.getTaskOutputs())
		{
			XElement child = ser.Serialize(output);
			element.Add(child);
		}

	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	@Override
	public void Execute()
	{
		IResourceService resSvc = this.getSite().<IResourceService>GetService();
		if (resSvc != null)
		{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			ResourceParameter res = new ResourceParameter("RunTask", new { taskname = this.getTaskName(), namespace = this.getTaskNamespaceUri() });
//C# TO JAVA CONVERTER NOTE: The following 'using' block is replaced by its Java equivalent:
//			using (IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res }))
			IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res });
			try
			{
				InnerExecute();
			}
			finally
			{
				handle.dispose();
			}
		}
		else
		{
			InnerExecute();
		}
	}

	private void InnerExecute()
	{
		IConditionService conditionSvc = (IConditionService)this.Site.GetService(IConditionService.class);
		ILogger logger = (ILogger)this.Site.GetService(ILogger.class);

		if (conditionSvc.Evaluate(this))
		{
			logger.LogMessage(LogCategories.getInstruction(), "Execute task {0}", this.getTaskName());
			IJob job = (IJob)this.Site.GetService(IJob.class);
			java.lang.Class t = job.ResolveInstructionType((XNamespace)this.TaskNamespaceUri + this.getTaskName());
			ITask task = (ITask)Activator.CreateInstance(t);
			System.ComponentModel.Design.ServiceContainer site = new System.ComponentModel.Design.ServiceContainer(this.getSite());
			site.AddService(ExecuteTaskInstruction.class, this);
			site.AddService(XmlNamespaceManager.class, this._xmlNamespaceManager);

			task.Site = site;

			this.SetInputParams(task);
			try
			{
				boolean success = task.Execute();

				if (!success)
				{
					throw new TaskFailedException(String.format(Properties.Resources.getTaskFailedText(), this.getTaskName()));
				}
			}
			finally
			{
				IStatusBarService statusBar = this.getSite().<IStatusBarService>GetService();
				if (statusBar != null)
				{
					statusBar.SetStatus("");
				}
			}
			this.SetOutputParams(task);
		}
		else if(logger != null)
		{
			logger.LogMessage(LogCategories.getInstruction(), "Skip task {0}", this.getTaskName());
		}
	}


	private void SetOutputParams(ITask task)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Func<MemberInfo, TaskMemberAttribute> getAttribute = mi => ((TaskMemberAttribute)mi.GetCustomAttributes(TaskMemberAttribute.class, true)[0]);

		java.lang.Class taskType = task.getClass();
		IConditionService condition = (IConditionService)this.Site.GetService(IConditionService.class);
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		for (TaskOutput output : this.getTaskOutputs().Where(t => condition.Evaluate(t)))
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			MemberInfo mi = (from m in taskType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty) where m.IsDefined(TaskMemberAttribute.class, true) && (getAttribute(m).Flags & TaskMemberFlags.Output) > 0 && output.getTaskParameter().equals(getAttribute(m).getName()) select m).SingleOrDefault();
			if(mi != null)
			{
				java.lang.Class memberType = this.GetMemberInfoType(mi);
				if (!DotNetToJavaStringHelper.isNullOrEmpty(output.getItemCategory()))
				{
					CreateTaskItems(task, output, mi);
				}
				else
				{
					CreateJobProperty(task, output, mi);
				}
			}
			else
			{
				throw new JobException(String.format(Properties.Resources.getUndefinedTaskOutputParameterText(), this.getTaskName(), output.getTaskParameter()));
			}

		}
	}

	private void CreateJobProperty(ITask task, TaskOutput output, MemberInfo mi)
	{
		java.lang.Class memberType = this.GetMemberInfoType(mi);
		StringBuilder text = new StringBuilder();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		MethodInvoker<Object> AppendText = delegate (Object o)
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
					TypeConverter cvt = XHelper.Default.CreateXConverter(o.getClass());
					s = (String)cvt.ConvertFrom(o);
				}
				if (!DotNetToJavaStringHelper.isNullOrEmpty(s))
				{
					if (text.length() > 0)
					{
						text.append(";");
					}
					text.append(s);
				}
			}
		}

		Object value = this.GetMemberInfoValue(mi, task);
		if (value != null)
		{
			if (value instanceof Array)
			{
				for (Object ele : (Array)value)
				{
					AppendText(ele);
				}
			}
			else
			{
				AppendText(value);
			}
		}

		IJob job = (IJob)this.Site.GetService(IJob.class);
		job.SetProperty(output.getPropertyName(), text.toString());

	}

	private void CreateTaskItems(ITask task, TaskOutput output, MemberInfo mi)
	{
		java.lang.Class memberType = this.GetMemberInfoType(mi);

		TaskItem[] items = null;
		if (memberType == TaskItem.class)
		{
			TaskItem item = (TaskItem)this.GetMemberInfoValue(mi, task);
			if (item != null)
			{
				items = new TaskItem[] { item };
			}
		}
		else if (memberType == TaskItem[].class)
		{
			items = (TaskItem[])this.GetMemberInfoValue(mi, task);
		}
		else if (memberType == String.class || memberType == String[].class || Iterable<String>.class.IsAssignableFrom(memberType))
		{
			Object v = this.GetMemberInfoValue(mi, task);

			if (v != null)
			{
				Iterable paths;
				if (memberType == String.class)
				{
					paths = ((String)v).split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				}
				else
				{
					paths = (Iterable)v;
				}

				IJobEngine engine = this.getSite().<IJobEngine>GetRequiredService();
				java.util.ArrayList<TaskItem> list = new java.util.ArrayList<TaskItem>();
				for (String path : paths)
				{
					String name;
					if (LongPath.IsPathRooted(path))
					{
						name = LongPath.GetRelativePath(engine.getJobDirectory() + "\\", path);
					}
					else
					{
						name = path;
					}
					TaskItem tempVar = new TaskItem();
					tempVar.setName(name);
					list.add(tempVar);
				}
				items = list.toArray(new TaskItem[]{});
			}
		}
		else
		{

			throw new JobException(String.format(Properties.Resources.getCannotConverTaskOutputParameterToTaskItemText(), output.getTaskParameter(), this.getTaskName()));
		}

		if (items != null && items.length > 0)
		{

			IJob job = (IJob)this.Site.GetService(IJob.class);
			TaskItemGroup group = job.AddTaskItemGroup();
			for (TaskItem item : items)
			{
				TaskItem newItem = group.AddNewItem(item.getName(), output.getItemCategory());
				newItem.setCondition(item.getCondition());
				item.CopyMetaDataTo(newItem);
			}
		}


	}



	private void SetInputParams(ITask task)
	{
		java.lang.Class taskType = task.getClass();

//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		Func<MemberInfo, TaskMemberAttribute> getAttribute = mi =>((TaskMemberAttribute)mi.GetCustomAttributes(TaskMemberAttribute.class, true)[0]);

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
		var query = from mi in taskType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty) where mi.IsDefined(TaskMemberAttribute.class, true) && (getAttribute(mi).Flags & TaskMemberFlags.Input) > 0 select new {Member = mi, Attribute = getAttribute(mi) };
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
		for (var m : query)
		{
			MemberInfo mi = m.Member;
			if (this._properties.containsKey(m.Attribute.getName()))
			{
				try
				{
					java.lang.Class memberType = this.GetMemberInfoType(mi);

					if (memberType == TaskItem.class)
					{
						SetTaskItemValue(task, mi, m.Attribute.getName());
					}
					else if (memberType == TaskItem[].class)
					{
						SetTaskItemsValue(task, mi, m.Attribute.getName());
					}
					else if (memberType.IsArray)
					{
						SetArrayValue(task, mi, m.Attribute.getName());
					}
					else
					{
						SetScalarValue(task, mi, m.Attribute.getName());
					}
				}
				catch (ThreadAbortException e)
				{
				}
				catch (RuntimeException error)
				{
					throw new JobException(String.format(Properties.Resources.getSetTaskParamErrorText(), this.getTaskName(), m.Attribute.getName()), error);
				}
			}
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			else if ((m.Attribute.Flags & TaskMemberFlags.Inline) > 0 &&　this._inlineElements.Any(e=>e.getName().LocalName.compareToIgnoreCase(m.Attribute.getName()) == 0))
			{
				java.lang.Class memberType = this.GetMemberInfoType(mi);
				if (memberType == XElement.class)
				{
					this.SetInlineValue(task, mi, m.Attribute.getName(), m.Attribute.ParseInline);
				}
				else
				{
					throw new JobException(String.format(Properties.Resources.getInlineTaskMemberMustBeXElementText(), this.getTaskName(), m.Attribute.getName()));
				}
			}
			else if ((m.Attribute.Flags & TaskMemberFlags.Required) > 0)
			{
				throw new JobException(String.format(Properties.Resources.getRequireAttrubiteOfTaskText(), m.Attribute.getName(), this.getTaskName()));
			}
		}
	}

	private void SetInlineValue(ITask task, MemberInfo mi, String elementName, boolean parseInline)
	{
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		XElement element = this._inlineElements.Find(e => e.getName().LocalName.compareToIgnoreCase(elementName) == 0);

		XElement val = parseInline ? parser.Parse(element) : new XElement(element);

		this.SetMemberInfoValue(mi, task, val);
	}


	private void SetScalarValue(ITask task, MemberInfo mi, String param)
	{
		IStringParser parser = this.getSite().<IStringParser>GetRequiredService();
		String str = parser.Parse(this._properties.get(param));
		if (!DotNetToJavaStringHelper.isNullOrEmpty(str))
		{
			TypeConverter cvt = XHelper.Default.CreateXConverter(this.GetMemberInfoType(mi));
			Object value = cvt.ConvertTo(str, this.GetMemberInfoType(mi));
			this.SetMemberInfoValue(mi, task, value);
		}
	}

	private void SetArrayValue(ITask task, MemberInfo mi, String param)
	{

		java.lang.Class memberType = this.GetMemberInfoType(mi);
		java.lang.Class elementType = memberType.GetElementType();
		java.util.ArrayList temp = new java.util.ArrayList();
		IStringParser parser = (IStringParser)this.Site.GetService(IStringParser.class);
		String str = parser.Parse(this._properties.get(param));
		if (!DotNetToJavaStringHelper.isNullOrEmpty(str))
		{

			String[] strArr = str.split("[;]", -1);
			Array arr = Array.CreateInstance(elementType, strArr.length);
			TypeConverter cvt = XHelper.Default.CreateXConverter(elementType);
			for (int i = 0; i < arr.getLength(); i++)
			{
				Object value = cvt.ConvertTo(strArr[i], elementType);
				arr.SetValue(value, i);
			}

			this.SetMemberInfoValue(mi, task, arr);
		}
	}



	private void SetTaskItemsValue(ITask task, MemberInfo mi, String param)
	{
		IItemParser parser = this.getSite().<IItemParser>GetRequiredService();
		TaskItem[] items = parser.ParseItem(this._properties.get(param));
		this.SetMemberInfoValue(mi, task, items);
	}

	private void SetTaskItemValue(ITask task, MemberInfo mi, String param)
	{
		IItemParser parser = this.getSite().<IItemParser>GetRequiredService();
		TaskItem[] items = parser.ParseItem(this._properties.get(param));
		if (items.length == 1)
		{
			this.SetMemberInfoValue(mi, task, items[0]);
		}
		else if (items.length > 1)
		{
			throw new JobException(String.format(Properties.Resources.getInputParamContainsMultipleItemsText(), param, this.getTaskName(), items.length));
		}
	}

	private java.lang.Class GetMemberInfoType(MemberInfo mi)
	{
		if (mi.MemberType == MemberTypes.Field)
		{
			return ((java.lang.reflect.Field)mi).FieldType;
		}
		else
		{
			return ((PropertyInfo)mi).PropertyType;
		}
	}

	private void SetMemberInfoValue(MemberInfo mi, Object instance, Object value)
	{
		if (mi.MemberType == MemberTypes.Field)
		{
			((java.lang.reflect.Field)mi).SetValue(instance, value);
		}
		else
		{
			((PropertyInfo)mi).SetValue(instance, value, null);
		}
	}

	private Object GetMemberInfoValue(MemberInfo mi, Object instance)
	{
		if (mi.MemberType == MemberTypes.Field)
		{
			return ((java.lang.reflect.Field)mi).GetValue(instance);
		}
		else
		{
			return ((PropertyInfo)mi).GetValue(instance, null);
		}
	}
}