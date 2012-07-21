package fantasy.jobs;


import java.util.*;

import fantasy.*;
import fantasy.jobs.properties.*;
import fantasy.jobs.management.*;
import fantasy.xserialization.*;
import fantasy.jobs.*;
import fantasy.jobs.resources.*;
import fantasy.servicemodel.*;
import org.jdom2.*;


@XSerializable(name = "job", namespaceUri=Consts.XNamespaceURI)
public class Job extends MarshalByRefObject implements IJob, IObjectWithSite
{
	public Job()
	{

//C# TO JAVA CONVERTER TODO TASK: Java has no equivalent to C#-style event wireups:
		this._imports.Inserted += new EventHandler<CollectionEventArgs<ImportAssembly>>(ImportAssemblyAdded);
	}

	private void ImportAssemblyAdded(Object sender, CollectionEventArgs<ImportAssembly> e)
	{
		this.LoadAssembly(e.getValue().LoadAssembly(this.getParser()));

	}
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("id")]
	public UUID _id;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("startupTarget")]
	private String _startupTarget;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XAttribute("template")]
	private String _templateName;

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XElement("runtime")]
	private RuntimeStatus _runtimeStatus = new RuntimeStatus();


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning disable 169
//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XNamespace]
	private XmlNamespaceManager _namespaces;
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
///#pragma warning restore 169





//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Name="properties", Serializer=typeof(JobPropertiesSerializer))]
	private java.util.ArrayList<JobProperty> _properties = new java.util.ArrayList<JobProperty>();



	private void RemoveProperty(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		int index = _properties.BinarySearchBy(name, (p) => p.getName(), StringComparer.OrdinalIgnoreCase);
		if (index >= 0)
		{
			_properties.remove(index);
		}
	}


	private String GetProperty(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		int index = _properties.BinarySearchBy(name, (p) => p.getName(), StringComparer.OrdinalIgnoreCase);

		return index >= 0 ? this._properties.get(index).getValue() : null;

	}
	private void SetProperty(String name, String value)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		int index = _properties.BinarySearchBy(name, (p) => p.getName(), StringComparer.OrdinalIgnoreCase);
		JobProperty prop;
		if (index < 0)
		{
			JobProperty tempVar = new JobProperty();
			tempVar.setName(name);
			prop = tempVar;
			this._properties.add(~index, prop);
		}
		else
		{
			prop = _properties.get(index);
		}

		prop.setValue(value);


	}
	private boolean HasProperty(String name)
	{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
		return _properties.BinarySearchBy(name, (p) => p.getName(), StringComparer.OrdinalIgnoreCase) >= 0;
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Name = "imports"), XArrayItem(Name = "import", java.lang.Class = typeof(ImportAssembly))]
	private Collection<ImportAssembly> _imports = new Collection<ImportAssembly>();






//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Name = "targets"), XArrayItem(Name = "target", java.lang.Class=typeof(Target))]
	private java.util.ArrayList<Target> _targets = new java.util.ArrayList<Target>();


	private java.util.HashMap<String, Integer> _targetPriorities = new java.util.HashMap<String, Integer>(StringComparer.OrdinalIgnoreCase);

	private IStringParser _parser;
	private IStringParser getParser()
	{
		if (_parser == null)
		{
			_parser = (IStringParser)this.Site.GetService(IStringParser.class);
			if (_parser == null)
			{
				throw new ApplicationException("Missing IStringParserService.");
			}
		}
		return _parser;
	}

	private IConditionService _conditionService;
	private IConditionService getConditionService()
	{
		if (_conditionService == null)
		{
			_conditionService = (IConditionService)this.Site.GetService(IConditionService.class);
			if (_conditionService == null)
			{
				throw new ApplicationException("Missing IConditionService.");
			}
		}
		return _conditionService;
	}

	private ILogger _logger;
	private ILogger getLogger()
	{
		if (_logger == null)
		{
			_logger = (ILogger)this.Site.GetService(ILogger.class);
		}
		return _logger;
	}


	private TaskItem AddTaskItem(String name, String category)
	{
		TaskItemGroup group;
		if (this._itemGroups.size() > 0 && DotNetToJavaStringHelper.isNullOrEmpty(this._itemGroups.Last().Condition))
		{
			group = this._itemGroups.Last();
		}
		else
		{
			group = this.AddTaskItemGroup();
		}

		TaskItem rs = group.AddNewItem(name, category);
		return rs;
	}


	private TaskItemGroup AddTaskItemGroup()
	{
		TaskItemGroup rs = new TaskItemGroup();
		this._itemGroups.add(rs);
		return rs;
	}

	private void RemoveTaskItemGroup(TaskItemGroup group)
	{
		this._itemGroups.remove(group);
	}

//C# TO JAVA CONVERTER TODO TASK: Java annotations will not correspond to .NET attributes:
	//[XArray(Name="itemGroups"), XArrayItem(Name="items", java.lang.Class=typeof(TaskItemGroup))]
	private java.util.ArrayList<TaskItemGroup> _itemGroups = new java.util.ArrayList<TaskItemGroup>();

	private TaskItem[] GetItems()
	{

			java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
			for (TaskItemGroup group : this._itemGroups)
			{
				for (TaskItem item : group)
				{
					rs.add(item);
				}
			}
			return rs.toArray(new TaskItem[]{});

	}

	private TaskItem[] GetEvaluatedItemsByCatetory(String category)
	{
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		for (TaskItemGroup g : this._itemGroups)
		{
			if (getConditionService().Evaluate(g))
			{
//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				var query = from i in g where String.equals(i.Category, category, StringComparison.OrdinalIgnoreCase) && getConditionService().Evaluate(i) select i;
				rs.addAll(query);
			}
		}
		return rs.toArray(new TaskItem[]{});
	}

	private TaskItem GetEvaluatedItemByName(String name)
	{

//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		TaskItem rs = (from g in this._itemGroups where getConditionService().Evaluate(g) from item in g where StringComparer.OrdinalIgnoreCase.Compare(name, item.getName()) == 0 && getConditionService().Evaluate(item) select item).FirstOrDefault();

		return rs;
	}

	private void RemoveTaskItem(TaskItem item)
	{
		for (TaskItemGroup group : this._itemGroups)
		{
			int index = group.indexOf(item);
			if (index >= 0)
			{
				group.RemoveAt(index);
				break;
			}
		}
	}

//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IObjectWithSite Members


	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion

	private IJobEngine getEngine()
	{
		return (IJobEngine)this.Site.GetService(IJobEngine.class);
	}

	public final void Initialize()
	{
		this.LoadAssembly(Assembly.GetExecutingAssembly());
	}

	private java.util.ArrayList<Assembly> _loadedAssebmlies = new java.util.ArrayList<Assembly>();
	private java.util.HashMap<XName, java.lang.Class> _loadedTaskOrInstructionTypes = new java.util.HashMap<XName, java.lang.Class>();

	private java.lang.Class ResolveInstructionType(XName name)
	{

		java.lang.Class rs = null;
		if ((rs = this._loadedTaskOrInstructionTypes.get(name)) != null)
		{
			return rs;
		}
		else
		{
			throw new InvalidJobTemplateException(String.format(fantasy.jobs.Properties.Resources.getUnknownTaskText(), name.NamespaceName, name.LocalName));
		}


	}

	private boolean LoadAssembly(Assembly assembly)
	{
		if (_loadedAssebmlies.indexOf(assembly) < 0)
		{
			this._loadedAssebmlies.add(assembly);
			this.LoadInstructionsAndTasks(assembly);
			return true;

		}
		else
		{
			return false;
		}
	}

	private void LoadInstructionsAndTasks(Assembly assembly)
	{

		Iterable<java.lang.Class> tasks;
		try
		{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
			tasks = from type in assembly.GetTypes() where type.GetCustomAttributes(TaskAttribute.class, false).getLength() == 1 select type;
		}
		catch(ReflectionTypeLoadException error)
		{
//C# TO JAVA CONVERTER TODO TASK: Lambda expressions and anonymous methods are not converted by C# to Java Converter:
			String message = String.format("Cannot get types from assembly %1$s. LoaderExceptions:\n %2$s", assembly.FullName, DotNetToJavaStringHelper.join("\n", error.LoaderExceptions.Select(e=>e.Message)));
			throw new JobException(message, error);
		}
		for (java.lang.Class t : tasks)
		{
			TaskAttribute ta = (TaskAttribute)t.GetCustomAttributes(TaskAttribute.class, false)[0];


			XName key = (XNamespace)ta.getNamespaceUri() + ta.getName();


			if (!_loadedTaskOrInstructionTypes.containsKey(key))
			{
				this._loadedTaskOrInstructionTypes.put(key, t);
			}
			else
			{
				throw new InvalidJobTemplateException(String.format(fantasy.jobs.Properties.Resources.getDulplicateTaskText(), t, this._loadedTaskOrInstructionTypes.get(key), ta.getNamespaceUri(), ta.getName()));

			}
		}

//C# TO JAVA CONVERTER TODO TASK: There is no equivalent to implicit typing in Java:
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		var instructions = from type in assembly.GetTypes() where type.IsDefined(InstructionAttribute.class, false) && type.IsDefined(XSerializableAttribute.class, false) select type;
		for (java.lang.Class t : instructions)
		{
			XSerializableAttribute sa = (XSerializableAttribute)t.GetCustomAttributes(XSerializableAttribute.class, false)[0];
			XName key = (XNamespace)sa.NamespaceUri + sa.getName();
			if (!_loadedTaskOrInstructionTypes.containsKey(key))
			{
				this._loadedTaskOrInstructionTypes.put(key, t);
			}
			else
			{
				throw new InvalidJobTemplateException(String.format(fantasy.jobs.Properties.Resources.getDulplicateTaskText(), t, this._loadedTaskOrInstructionTypes.get(key), sa.NamespaceUri, sa.getName()));
			}
		}

	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region Build from StartInfo Methods




	public final void LoadStartInfo(JobStartInfo startInfo)
	{

		this.LoadAssembly(Assembly.GetExecutingAssembly());
		this.AddPropertiesFromStartInfo(startInfo);
		this.AddItemsFromStartInfo(startInfo);
		this._templateName = startInfo.getTemplate();
		IJobTemplatesService ts = (IJobTemplatesService)this.Site.GetService(IJobTemplatesService.class);
		JobTemplate template = ts.GetJobTemplateByName(startInfo.getTemplate());
		if (!DotNetToJavaStringHelper.isNullOrEmpty(startInfo.getTarget()))
		{
			this._startupTarget = startInfo.getTarget();
		}
		else
		{
			XmlDocument doc = new XmlDocument();
			doc.LoadXml(template.getContent());
			this._startupTarget = doc.DocumentElement.GetAttribute("defaultTarget");
		}

		this.AddTemplate(template, 1);
	}

	private void AddTemplate(JobTemplate template, int priority)
	{
		XElement root = XElement.Parse(template.getContent());


		for (XElement element : root.Elements())
		{
//C# TO JAVA CONVERTER NOTE: The following 'switch' operated on a string member and was converted to Java 'if-else' logic:
//			switch (element.Name.LocalName)
//ORIGINAL LINE: case "import":
			if (element.getName().LocalName.equals("import"))
			{
					this.AddImportFromTemplate(element, template);
			}
//ORIGINAL LINE: case "include":
			else if (element.getName().LocalName.equals("include"))
			{
					this.AddInlcude(element, template, priority);
			}
//ORIGINAL LINE: case "properties":
			else if (element.getName().LocalName.equals("properties"))
			{
					this.AddPropertiesFromTemplate(element, priority);
			}
//ORIGINAL LINE: case "target":
			else if (element.getName().LocalName.equals("target"))
			{
					this.AddTargetFrom(element, priority);
			}
//ORIGINAL LINE: case "items" :
			else if (element.getName().LocalName.equals("items"))
			{
					this.AddItemsFromTemplate(element);
			}
		}
	}

	private void AddItemsFromTemplate(XElement element)
	{
		XSerializer ser = new XSerializer(TaskItemGroup.class);
		TaskItemGroup group = (TaskItemGroup)ser.Deserialize(element);
		for (TaskItem item : group)
		{
			item.setName(this.getParser().Parse(item.getName()));
		}
		this._itemGroups.add(group);
	}

	private void AddTargetFrom(XElement element, int priority)
	{

		int oldPri = 0;
		boolean needAdd = false;
		String targetName = (String)element.Attribute(Target.NameAttributeName);
		if ((oldPri = this._targetPriorities.get(targetName)) != null)
		{
			if (priority <= oldPri)
			{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
				Target oldTarget = (from tgt in this._targets where StringComparer.OrdinalIgnoreCase.Compare(tgt.getName(), targetName) == 0 select tgt).Single();
				this._targets.remove(oldTarget);
				needAdd = true;
			}

		}
		else
		{
			needAdd = true;

		}

		if (needAdd)
		{
			XSerializer tempVar = new XSerializer(Target.class);
			tempVar.Context = this.getSite();
			XSerializer ser = tempVar;

			Target target = new Target();

			ser.Deserialize(element, target);
			this._targets.add(target);
			this._targetPriorities.put(target.getName(), priority);
		}

	}

	private void AddPropertiesFromTemplate(XElement element, int priority)
	{
		XSerializer ser = new XSerializer(JobPropertyGroup.class);
		JobPropertyGroup group = (JobPropertyGroup)ser.Deserialize(element);
		if (getConditionService().Evaluate(group))
		{
			for (JobProperty property : group)
			{
				if (getConditionService().Evaluate(property))
				{
					this.TryAddProperty(property.getName(), property.getValue(), priority);
				}
			}
		}
	}



	private void AddInlcude(XElement element, JobTemplate template, int priority)
	{
		JobTemplate newTemplate;
		IJobTemplatesService ts = (IJobTemplatesService)this.Site.GetService(IJobTemplatesService.class);
		String name = (String)element.Attribute("name");
		if (!DotNetToJavaStringHelper.isNullOrEmpty(name))
		{
			newTemplate = ts.GetJobTemplateByName(name);
		}
		else
		{
			String path = (String)element.Attribute("path");
			if (!DotNetToJavaStringHelper.isNullOrEmpty("path"))
			{
				path = getParser().Parse(path);
				if (!System.IO.Path.IsPathRooted(path))
				{
					String dir = System.IO.Path.GetDirectoryName(template.getLocation());
					path = Fantasy.IO.LongPath.Combine(dir, path);
				}
				newTemplate = ts.GetJobTemplateByPath(path);
			}
			else
			{
			   throw new InvalidOperationException("\"include\" element must has name or path attribute.");
			}
		}

		this.AddTemplate(newTemplate, priority + 1);

	}


	private void AddImportFromTemplate(XElement element, JobTemplate template)
	{
		XSerializer ser = new XSerializer(ImportAssembly.class);
		ImportAssembly import = (ImportAssembly)ser.Deserialize(element);
		import.setSrc(Path.GetDirectoryName(template.getLocation()));
		Assembly assembly = import.LoadAssembly(this.getParser());
		if (this.LoadAssembly(assembly))
		{
			this._imports.Add(import);
		}
	}



	private java.util.HashMap<String, Integer> _propPriorities = new java.util.HashMap<String, Integer>(StringComparer.OrdinalIgnoreCase);

	private void AddPropertiesFromStartInfo(JobStartInfo startInfo)
	{

		for (JobPropertyGroup group : startInfo.getPropertyGroups())
		{
			if (getConditionService().Evaluate(group))
			{
				for (JobProperty property : group)
				{
					if (getConditionService().Evaluate(group))
					{
						this.TryAddProperty(property.getName(), property.getValue(), 0);
					}
				}
			}
		}
	}

	private void AddItemsFromStartInfo(JobStartInfo startInfo)
	{


		for (TaskItemGroup group : startInfo.getItemGroups())
		{
			for (TaskItem item : group)
			{
				item.setName(this.getParser().Parse(item.getName()));
			}

			this._itemGroups.add(group);
		}
	}

	private void TryAddProperty(String name, String value, int priority)
	{


		int old = 0;
		if ((old = this._propPriorities.get(name)) != null)
		{
			if (old >= priority)
			{
				this._propPriorities.put(name, priority);
				this.SetProperty(name, value);
			}

		}
		else
		{
			this._propPriorities.put(name, priority);
			this.SetProperty(name, value);
		}


	}
//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#endregion



	private void ExecuteInstruction(IInstruction instruction)
	{
		if (instruction == null)
		{
			throw new ArgumentNullException("instruction");
		}
		instruction.Site = this.getSite();
		this._runtimeStatus.PushStack();
		try
		{
			try
			{
				instruction.Execute();
			}

			catch (RuntimeException ex)
			{

				this.getEngine().SaveStatusForError(ex);
				if (!(ex instanceof ThreadAbortException))
				{
					throw ex;
				}
			}
		}
		finally
		{
			this._runtimeStatus.PopStack();
		}
	}

	private void ExecuteTarget(String targetName)
	{
//C# TO JAVA CONVERTER TODO TASK: There is no Java equivalent to LINQ queries:
		Target target = (from tgt in this._targets where StringComparer.OrdinalIgnoreCase.Compare(tgt.getName(), targetName) == 0 select tgt).SingleOrDefault();

		if (target == null)
		{
			throw new JobException(String.format(fantasy.jobs.Properties.Resources.getUndefinedTargetText(), targetName));
		}
		this.ExecuteInstruction(target);

	}

	private void Execute()
	{
		if (DotNetToJavaStringHelper.isNullOrEmpty(this._startupTarget))
		{
			throw new InvalidJobStartInfoException(String.format(fantasy.jobs.Properties.Resources.getNoStartupTargetText(), this._templateName));
		}
		IResourceService resSvc = this.getEngine().<IResourceService>GetService();
		IResourceHandle resHandle = null;
		if (resSvc != null)
		{

			ResourceParameter[] res = CreateExecuteResourceParameters();
			if (res.length > 0)
			{
				resHandle = resSvc.Request(res);
				this.SetProperty("WaitAll", "");
				this.SetProperty("WaitAny", "");
			}
		}
		try
		{
			this.ExecuteTarget(this._startupTarget);
		}
		finally
		{
			if (resHandle != null)
			{
				resHandle.dispose();
			}
		}
	}


	private boolean _isNested = false;

	public final boolean getIsNested()
	{
		return _isNested;
	}
	public final void setIsNested(boolean value)
	{
		_isNested = value;
	}


	private ResourceParameter[] CreateExecuteResourceParameters()
	{
		java.util.ArrayList<ResourceParameter> rs = new java.util.ArrayList<ResourceParameter>();
		if (! getIsNested())
		{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			rs.add(new ResourceParameter("RunJob", new { template = this._templateName }));
		}
		IStringParser parser = this.getEngine().<IStringParser>GetRequiredService();
		String waitAll = parser.Parse(this.GetProperty("WaitAll"));

		if (!DotNetToJavaStringHelper.isNullOrEmpty(waitAll))
		{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			rs.add(new ResourceParameter("WaitFor", new { jobs = waitAll, mode = WaitForMode.All}));
		}

		String waitAny = parser.Parse(this.GetProperty("WaitAny"));

		if (!DotNetToJavaStringHelper.isNullOrEmpty(waitAny))
		{
//C# TO JAVA CONVERTER TODO TASK: This type of object initializer has no direct Java equivalent:
			rs.add(new ResourceParameter("WaitFor", new { jobs = waitAll, mode = WaitForMode.Any }));
		}

		return rs.toArray(new ResourceParameter[]{});
	}


	public final void LoadStatus(Element element)
	{

		XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
		nsMgr.AddNamespace("cvj", Consts.XNamespaceURI);
		XSerializer impSer = new XSerializer(ImportAssembly.class);

		for (XElement ele : element.XPathSelectElements("/cvj:job/cvj:imports/cvj:import", nsMgr))
		{
			ImportAssembly ia = (ImportAssembly)impSer.Deserialize(ele);
			this.LoadAssembly(ia.LoadAssembly(null));
		}

		XSerializer tempVar = new XSerializer(Job.class);
		tempVar.Context = this.getSite();
		XSerializer jobser = tempVar;

		jobser.Deserialize(element, this);

	}

	public final Element SaveStatus()
	{
		XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
		XSerializer ser = new XSerializer(Job.class);
		Element rs = ser.Serialize(this,ns);
		return rs;
	}


//C# TO JAVA CONVERTER TODO TASK: There is no preprocessor in Java:
		///#region IJob Members

	private TaskItem AddTaskItem(String name, String category)
	{
		return this.AddTaskItem(name, category);
	}

	private void RemoveTaskItem(TaskItem item)
	{
		this.RemoveTaskItem(item);
	}

	private TaskItemGroup AddTaskItemGroup()
	{
		return this.AddTaskItemGroup();
	}

	private void RemoveTaskItemGroup(TaskItemGroup group)
	{
		this.RemoveTaskItemGroup(group);
	}

	private TaskItem[] GetEvaluatedItemsByCatetory(String category)
	{
		return this.GetEvaluatedItemsByCatetory(category);
	}

	private TaskItem GetEvaluatedItemByName(String name)
	{
		return this.GetEvaluatedItemByName(name);
	}

	private TaskItem[] getItems()
	{
		return this.GetItems();
	}

	public UUID getID()
	{
		return this._id;
	}
	
	void setID(UUID value)
	{
		this._id = value;
	}

	private String getTemplateName()
	{
		return this._templateName;
	}

	private JobProperty[] getProperties()
	{
		return this._properties.toArray(new JobProperty[]{});
	}

	private String GetProperty(String name)
	{
		return this.GetProperty(name);
	}

	private void SetProperty(String name, String value)
	{
		this.SetProperty(name, value);
	}

	private boolean HasProperty(String name)
	{
		return this.HasProperty(name);
	}

	private void RemoveProperty(String name)
	{
		this.RemoveProperty(name);
	}

	private String getStartupTarget()
	{
		return this._startupTarget;
	}
	private void setStartupTarget(String value)
	{
		this._startupTarget = value;
	}

	private java.lang.Class ResolveInstructionType(XName name)
	{
		return this.ResolveInstructionType(name);
	}

	private RuntimeStatus getRuntimeStatus()
	{
		return this._runtimeStatus;
	}

	

	private void ExecuteInstruction(IInstruction instruction)
	{
		this.ExecuteInstruction(instruction);
	}

	private void ExecuteTarget(String targetName)
	{
		this.ExecuteTarget(targetName);
	}

}