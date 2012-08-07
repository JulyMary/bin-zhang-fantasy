package fantasy.jobs;


import java.io.*;
import java.nio.file.Paths;
import java.util.*;

import fantasy.*;
import fantasy.collections.*;

import fantasy.io.Path;
import fantasy.jobs.management.*;
import fantasy.xserialization.*;
import fantasy.jobs.resources.*;
import org.apache.commons.lang3.StringUtils;
import org.jdom2.*;
import org.jdom2.filter.*;
import org.jdom2.xpath.*;



@SuppressWarnings({"rawtypes", "unchecked"})
@XSerializable(name = "job", namespaceUri=Consts.XNamespaceURI)
public class Job  implements IJob, IObjectWithSite
{
	public Job()
	{


		
	}


	@XAttribute(name = "id")
	public UUID _id;


	@XAttribute(name ="startupTarget")
	private String _startupTarget;

	@XAttribute(name = "template")
	private String _templateName;

	@XElement(name = "runtime")
	private RuntimeStatus _runtimeStatus = new RuntimeStatus();


	@XNamespace
	private Namespace[] _namespaces;

	@XArray(name="properties", serializer=JobPropertiesSerializer.class, items={})
	private java.util.ArrayList<JobProperty> _properties = new java.util.ArrayList<JobProperty>();

	public void RemoveProperty(String name)
	{
		
		int index = CollectionUtils.binarySearchBy(this._properties, name, new Selector<JobProperty, String>(){

			@Override
			public String select(JobProperty item) {
				return item.getName();
			}}, String.CASE_INSENSITIVE_ORDER);
		if (index >= 0)
		{
			_properties.remove(index);
		}
	}


	public String GetProperty(String name)
	{

		int index = CollectionUtils.binarySearchBy(this._properties, name, new Selector<JobProperty, String>(){

			@Override
			public String select(JobProperty item) {
				return item.getName();
			}}, String.CASE_INSENSITIVE_ORDER);

		return index >= 0 ? this._properties.get(index).getValue() : null;

	}


	public void SetProperty(String name, String value)
	{

		int index = CollectionUtils.binarySearchBy(this._properties, name, new Selector<JobProperty, String>(){

			@Override
			public String select(JobProperty item) {
				return item.getName();
			}}, String.CASE_INSENSITIVE_ORDER);
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
	public  boolean HasProperty(String name)
	{
		
		return CollectionUtils.binarySearchBy(this._properties, name, new Selector<JobProperty, String>(){

			@Override
			public String select(JobProperty item) {
				return item.getName();
			}}, String.CASE_INSENSITIVE_ORDER) >= 0;
	}

	@XArray(name = "imports", items = @XArrayItem(name = "import", type = ImportAssembly.class))
	private ImportList _imports = new ImportList();







	@XArray(name = "targets", items= @XArrayItem(name = "target", type= Target.class))
	private java.util.ArrayList<Target> _targets = new java.util.ArrayList<Target>();


	private java.util.TreeMap<String, Integer> _targetPriorities = new java.util.TreeMap<String, Integer>(String.CASE_INSENSITIVE_ORDER);

	private IStringParser _parser;
	private IStringParser getParser() throws Exception
	{
		if (_parser == null)
		{
			_parser = (IStringParser)this.getSite().getRequiredService(IStringParser.class);

		}
		return _parser;
	}

	private IConditionService _conditionService;
	private IConditionService getConditionService() throws Exception
	{
		if (_conditionService == null)
		{
			_conditionService = (IConditionService)this.getSite().getRequiredService(IConditionService.class);

		}
		return _conditionService;
	}

	public TaskItem AddTaskItem(String name, String category)
	{
		TaskItemGroup group;
		if (this._itemGroups.size() > 0 && StringUtils2.isNullOrEmpty(this._itemGroups.get(this._itemGroups.size() - 1).getCondition()))
		{
			group = this._itemGroups.get(this._itemGroups.size() - 1);
		}
		else
		{
			group = this.AddTaskItemGroup();
		}

		TaskItem rs = group.AddNewItem(name, category);
		return rs;
	}


	public TaskItemGroup AddTaskItemGroup()
	{
		TaskItemGroup rs = new TaskItemGroup();
		this._itemGroups.add(rs);
		return rs;
	}

	public void RemoveTaskItemGroup(TaskItemGroup group)
	{
		this._itemGroups.remove(group);
	}

	@XArray(name="itemGroups", items= @XArrayItem(name="items", type = TaskItemGroup.class))
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

	public TaskItem[] GetEvaluatedItemsByCatetory(final String category) throws Exception
	{
		java.util.ArrayList<TaskItem> rs = new java.util.ArrayList<TaskItem>();
		for (TaskItemGroup g : this._itemGroups)
		{
			if (getConditionService().Evaluate(g))
			{

				Enumerable<TaskItem> query = new Enumerable<TaskItem>(g).where(new Predicate<TaskItem>(){

					@Override
					public boolean evaluate(TaskItem item) throws Exception {
						return StringUtils.equalsIgnoreCase(item.getCategory(), category) && Job.this.getConditionService().Evaluate(item);
					}});
				rs.addAll(query.toArrayList());
			}
		}
		return rs.toArray(new TaskItem[]{});
	}

	public TaskItem GetEvaluatedItemByName(final String name) throws Exception
	{

		Enumerable<TaskItem> query = new Enumerable<TaskItemGroup>(this._itemGroups).where(new Predicate<TaskItemGroup>(){

			@Override
			public boolean evaluate(TaskItemGroup g) throws Exception {

				return Job.this.getConditionService().Evaluate(g);
			}}).from(new Selector<TaskItemGroup, Iterable<TaskItem>>(){

				@Override
				public Iterable<TaskItem> select(TaskItemGroup g) {
					return g;
				}}).where(new Predicate<TaskItem>(){

					@Override
					public boolean evaluate(TaskItem item) throws Exception {
						return StringUtils.equalsIgnoreCase(item.getName(), name) && Job.this.getConditionService().Evaluate(item);
					}});

		TaskItem rs = query.firstOrDefault();

		return rs;
	}

	public void RemoveTaskItem(TaskItem item)
	{
		for (TaskItemGroup group : this._itemGroups)
		{
			int index = group.IndexOf(item);
			if (index >= 0)
			{
				group.RemoveAt(index);
				break;
			}
		}
	}




	private IServiceProvider privateSite;
	public final IServiceProvider getSite()
	{
		return privateSite;
	}
	public final void setSite(IServiceProvider value)
	{
		privateSite = value;
	}



	private IJobEngine getEngine() throws Exception
	{
		return (IJobEngine)this.getSite().getService(IJobEngine.class);
	}

	public final void Initialize() throws Exception
	{
		this._imports.addListener(new IImportListListener(){

			@Override
			public void Added(ImportListAddedEvent e) throws Exception {
				Job.this.LoadAssembly(e.getImportAssembly().LoadAssembly(Job.this.getParser()));

			}});
		this.LoadAssembly(JavaLibraryUtils.getLibrary(this.getClass()));
	}

	private java.util.TreeSet<String> _loadedAssebmlies = new java.util.TreeSet<String>(String.CASE_INSENSITIVE_ORDER);
	
	private java.util.HashMap<String, java.lang.Class> _loadedTaskOrInstructionTypes = new java.util.HashMap<String, java.lang.Class>();

	public java.lang.Class ResolveInstructionType(String name, String namespaceUri)
	{

		String key = namespaceUri + "|" + name;
		java.lang.Class rs =  this._loadedTaskOrInstructionTypes.get(key);
		if (rs == null)
		{
			throw new InvalidJobTemplateException(String.format(fantasy.jobs.properties.Resources.getUnknownTaskText(), namespaceUri, name));

		}
		return rs;


	}

	private boolean LoadAssembly(File assembly) throws Exception
	{
		String path = Paths.get(assembly.getAbsolutePath()).normalize().toString();
		if (!_loadedAssebmlies.contains(path))
		{
			this._loadedAssebmlies.add(path);
			this.LoadInstructionsAndTasks(assembly);
			return true;

		}
		else
		{
			return false;
		}
	}

	private void LoadInstructionsAndTasks(File assembly) throws Exception
	{
		
		Enumerable<Class> classes = new Enumerable<Class>(JavaLibraryUtils.GetAllClasses(assembly));

		Iterable<java.lang.Class> tasks;
		try
		{
			
			tasks = classes.where(new Predicate<Class>(){

				
				@Override
				public boolean evaluate(Class type) throws Exception {
					return type.isAnnotationPresent(Task.class);
				}});
		}
		catch(ClassNotFoundException error)
		{
			String message = String.format("Cannot get types from library %1$s. LoaderExceptions:\n %2$s", assembly.getAbsolutePath(), error.getMessage());
			throw new JobException(message, error);
		}
		for (java.lang.Class t : tasks)
		{
			Task anno = (Task) t.getAnnotation(Task.class);

            String key = anno.namespaceUri() + "|" + anno.name(); 
			


			if (!_loadedTaskOrInstructionTypes.containsKey(key))
			{
				this._loadedTaskOrInstructionTypes.put(key, t);
			}
			else
			{
				throw new InvalidJobTemplateException(String.format(fantasy.jobs.properties.Resources.getDulplicateTaskText(), t, this._loadedTaskOrInstructionTypes.get(key).getName(), anno.namespaceUri(), anno.name()));

			}
		}

		
		Iterable<java.lang.Class> instructions = classes.where(new Predicate<Class>(){

			@Override
			public boolean evaluate(Class type) throws Exception {
				
				return type.isAnnotationPresent(Instruction.class) && type.isAnnotationPresent(XSerializable.class);
			}}); 
		for (java.lang.Class t : instructions)
		{
			XSerializable anno = (XSerializable)t.getAnnotation(XSerializable.class);
			String key = anno.namespaceUri() + "|" + anno.name();
			if (!_loadedTaskOrInstructionTypes.containsKey(key))
			{
				this._loadedTaskOrInstructionTypes.put(key, t);
			}
			else
			{
				throw new InvalidJobTemplateException(String.format(fantasy.jobs.properties.Resources.getDulplicateTaskText(), t, this._loadedTaskOrInstructionTypes.get(key), anno.namespaceUri(), anno.name()));
			}
		}

	}


	public final void LoadStartInfo(JobStartInfo startInfo) throws Exception
	{

		this.LoadAssembly(JavaLibraryUtils.getLibrary(this.getClass()));
		this.AddPropertiesFromStartInfo(startInfo);
		this.AddItemsFromStartInfo(startInfo);
		this._templateName = startInfo.getTemplate();
		IJobTemplatesService ts = this.getSite().getRequiredService(IJobTemplatesService.class);
		JobTemplate template = ts.GetJobTemplateByName(startInfo.getTemplate());
		if (!StringUtils2.isNullOrEmpty(startInfo.getTarget()))
		{
			this._startupTarget = startInfo.getTarget();
		}
		else
		{
			Element ele = JDomUtils.parseElement(template.getContent());
			
			this._startupTarget = ele.getAttributeValue("defaultTarget");
		}

		this.AddTemplate(template, 1);
	}

	private void AddTemplate(JobTemplate template, int priority) throws Exception
	{
		Element root = JDomUtils.parseElement(template.getContent());


		for (Element element : root.getChildren())
		{
			
			if (element.getName().equals("import"))
			{
				this.AddImportFromTemplate(element, template);
			}
			
			else if (element.getName().equals("include"))
			{
				this.AddInlcude(element, template, priority);
			}
			//ORIGINAL LINE: case "properties":
			else if (element.getName().equals("properties"))
			{
				this.AddPropertiesFromTemplate(element, priority);
			}
			//ORIGINAL LINE: case "target":
			else if (element.getName().equals("target"))
			{
				this.AddTargetFrom(element, priority);
			}
			//ORIGINAL LINE: case "items" :
			else if (element.getName().equals("items"))
			{
				this.AddItemsFromTemplate(element);
			}
		}
	}

	private void AddItemsFromTemplate(Element element) throws Exception
	{
		XSerializer ser = new XSerializer(TaskItemGroup.class);
		TaskItemGroup group = (TaskItemGroup)ser.deserialize(element);
		for (TaskItem item : group)
		{
			item.setName(this.getParser().Parse(item.getName()));
		}
		this._itemGroups.add(group);
	}

	private void AddTargetFrom(Element element, int priority) throws Exception
	{

		int oldPri = 0;
		boolean needAdd = false;
		final String targetName = element.getAttributeValue(Target.NameAttributeName);
		if(this._targetPriorities.containsKey(targetName))
		{
		    oldPri = (int)this._targetPriorities.get(targetName);
			if (priority <= oldPri)
			{
				
				Target oldTarget = new Enumerable<Target>(this._targets).single(new Predicate<Target>(){

					@Override
					public boolean evaluate(Target obj) throws Exception {
						return StringUtils.equalsIgnoreCase(obj.Name, targetName);
					}});
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
			XSerializer ser = new XSerializer(Target.class);
			ser.setContext(this.getSite());
			

			Target target = new Target();

			ser.deserialize(element, target);
			this._targets.add(target);
			this._targetPriorities.put(target.Name, priority);
		}

	}

	private void AddPropertiesFromTemplate(Element element, int priority) throws Exception
	{
		XSerializer ser = new XSerializer(JobPropertyGroup.class);
		JobPropertyGroup group = (JobPropertyGroup)ser.deserialize(element);
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



	private void AddInlcude(Element element, JobTemplate template, int priority) throws Exception
	{
		JobTemplate newTemplate;
		IJobTemplatesService ts = (IJobTemplatesService)this.getSite().getRequiredService(IJobTemplatesService.class);
		String name = element.getAttributeValue("name");
		if (!StringUtils2.isNullOrEmpty(name))
		{
			newTemplate = ts.GetJobTemplateByName(name);
		}
		else
		{
			String path = (String)element.getAttributeValue("path");
			if (!StringUtils2.isNullOrEmpty("path"))
			{
				path = getParser().Parse(path);
				if (!Path.isPathRooted(path))
				{
					String dir = Path.getDirectoryName(template.getLocation());
					path = Path.combine(dir, path);
				}
				newTemplate = ts.GetJobTemplateByPath(path);
			}
			else
			{
				throw new IllegalStateException("\"include\" element must has name or path attribute.");
			}
		}

		this.AddTemplate(newTemplate, priority + 1);

	}


	private void AddImportFromTemplate(Element element, JobTemplate template) throws Exception
	{
		XSerializer ser = new XSerializer(ImportAssembly.class);
		ImportAssembly $import = (ImportAssembly)ser.deserialize(element);
		
		File assembly = $import.LoadAssembly(this.getParser());
		if (this.LoadAssembly(assembly))
		{
			this._imports.add($import);
		}
	}



	private java.util.TreeMap<String, Integer> _propPriorities = new java.util.TreeMap<String, Integer>(String.CASE_INSENSITIVE_ORDER);

	private void AddPropertiesFromStartInfo(JobStartInfo startInfo) throws Exception
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

	private void AddItemsFromStartInfo(JobStartInfo startInfo) throws Exception
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
		if (this._propPriorities.containsKey(name))
		{
			old = this._propPriorities.get(name);
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


	public void ExecuteInstruction(IInstruction instruction) throws Exception
	{
		if (instruction == null)
		{
			throw new IllegalArgumentException("instruction");
		}
		instruction.setSite(this.getSite());
		this._runtimeStatus.PushStack();
		try
		{
			try
			{
				instruction.Execute();
			}

			catch (Exception ex)
			{

				this.getEngine().SaveStatusForError(ex);
				
				throw ex;
				
			}
		}
		finally
		{
			this._runtimeStatus.PopStack();
		}
	}

	public void ExecuteTarget(final String targetName) throws Exception
	{
		
		Target target = new Enumerable<Target>(this._targets).singleOrDefault(new Predicate<Target>(){

			@Override
			public boolean evaluate(Target obj) throws Exception {
				return StringUtils.equalsIgnoreCase(obj.Name, targetName);
			}});
		if (target == null)
		{
			throw new JobException(String.format(fantasy.jobs.properties.Resources.getUndefinedTargetText(), targetName));
		}
		this.ExecuteInstruction(target);

	}

	public void Execute() throws Exception
	{
		if (StringUtils2.isNullOrEmpty(this._startupTarget))
		{
			throw new InvalidJobStartInfoException(String.format(fantasy.jobs.properties.Resources.getNoStartupTargetText(), this._templateName));
		}
		IResourceService resSvc = this.getEngine().getService(IResourceService.class);
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

	
	private ResourceParameter[] CreateExecuteResourceParameters() throws Exception
	{
		java.util.ArrayList<ResourceParameter> rs = new java.util.ArrayList<ResourceParameter>();
		if (! getIsNested())
		{
			HashMap<String, String> args = new HashMap<String, String>();
			args.put("template", this._templateName);
			
			rs.add(new ResourceParameter("RunJob", args));
		}
		IStringParser parser = this.getEngine().getRequiredService(IStringParser.class);
		final String waitAll = parser.Parse(this.GetProperty("WaitAll"));

		if (!StringUtils2.isNullOrEmpty(waitAll))
		{
			HashMap<String, String> args = new HashMap<String, String>();
			args.put("jobs", waitAll);
			args.put("mode", WaitForMode.All.name());
			rs.add(new ResourceParameter("WaitFor", args));
		}

		String waitAny = parser.Parse(this.GetProperty("WaitAny"));

		if (!StringUtils2.isNullOrEmpty(waitAny))
		{
			
			HashMap<String, String> args = new HashMap<String, String>();
			args.put("jobs", waitAny);
			args.put("mode", WaitForMode.Any.name());
			rs.add(new ResourceParameter("WaitFor", args));
			
		}

		return rs.toArray(new ResourceParameter[]{});
	}


	public final void LoadStatus(Element element) throws Exception
	{

		
		Namespace ns = Namespace.getNamespace(Consts.XNamespaceURI, "j");
		
		XPathExpression<Element> xpath = XPathFactory.instance().compile("/j:job/j:imports/:import", new ElementFilter(), null, ns);
		
		XSerializer impSer = new XSerializer(ImportAssembly.class);

		for (Element ele : xpath.evaluate(element))
		{
			ImportAssembly ia = (ImportAssembly)impSer.deserialize(ele);
			this.LoadAssembly(ia.LoadAssembly(null));
		}

		XSerializer jobser = new XSerializer(Job.class);
		jobser.setContext(this.getSite());
		

		jobser.deserialize(element, this);

	}

	public final Element SaveStatus() throws Exception
	{
		
		Namespace ns = Namespace.getNamespace("j", Consts.XNamespaceURI);
		
		
		XSerializer ser = new XSerializer(Job.class);
		Element rs = ser.serialize(this,new Namespace[]{ns});
		return rs;
	}



	public TaskItem[] getItems()
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

	public String getTemplateName()
	{
		return this._templateName;
	}

	public JobProperty[] getProperties()
	{
		return this._properties.toArray(new JobProperty[]{});
	}


	public String getStartupTarget()
	{
		return this._startupTarget;
	}
	public void setStartupTarget(String value)
	{
		this._startupTarget = value;
	}



	public RuntimeStatus getRuntimeStatus()
	{
		return this._runtimeStatus;
	}







}