using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using Fantasy.Jobs.Properties;
using Fantasy.Jobs.Management;
using Fantasy.XSerialization;
using Fantasy.Jobs;
using System.IO;
using System.Threading;
using Fantasy.Jobs.Resources;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Xml.Serialization;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    [XSerializable("job", NamespaceUri=Consts.XNamespaceURI)]     
    internal class Job : MarshalByRefObject, IJob, IObjectWithSite
    {
        public Job()
        {

            this._imports.Inserted += new EventHandler<CollectionEventArgs<ImportAssembly>>(ImportAssemblyAdded);
        }

        void ImportAssemblyAdded(object sender, CollectionEventArgs<ImportAssembly> e)
        {
            this.LoadAssembly(e.Value.LoadAssembly(this.Parser));
            
        }
        [XAttribute("id")]
        internal Guid _id;

        [XAttribute("startupTarget")]
        private string _startupTarget;

        [XAttribute("template")]
        private string _templateName;
        
        [XElement("runtime")]
        private RuntimeStatus _runtimeStatus = new RuntimeStatus();


#pragma warning disable 169
        [XNamespace]
        private XmlNamespaceManager _namespaces;
#pragma warning restore 169


       

        
        [XArray( Name="properties", Serializer=typeof(JobPropertiesSerializer)) ]
        private List<JobProperty> _properties = new List<JobProperty>(); 

        

        private void RemoveProperty(string name)
        {
            int index = _properties.BinarySearchBy(name, (p) => p.Name, StringComparer.OrdinalIgnoreCase);
            if (index >= 0)
            {
                _properties.RemoveAt(index); 
            }
        }


        private string GetProperty(string name)
        {
            int index = _properties.BinarySearchBy(name, (p) => p.Name, StringComparer.OrdinalIgnoreCase);
            
            return index >= 0 ? this._properties[index].Value : null;
            
        }
        private void SetProperty(string name, string value)
        {
            int index = _properties.BinarySearchBy(name, (p) => p.Name, StringComparer.OrdinalIgnoreCase);
            JobProperty prop;
            if (index < 0)
            {
                prop = new JobProperty() { Name = name };
                this._properties.Insert(~index, prop);
            }
            else
            {
                prop = _properties[index];
            }

            prop.Value = value;


        }
        private bool HasProperty(string name)
        {
            return _properties.BinarySearchBy(name, (p) => p.Name, StringComparer.OrdinalIgnoreCase) >= 0;
        }

        [XArray(Name = "imports")]
        [XArrayItem(Name = "import", Type = typeof(ImportAssembly))] 
        private Collection<ImportAssembly> _imports = new Collection<ImportAssembly>();

       

        
      

        [XArray(Name = "targets")]     
        [XArrayItem(Name = "target", Type=typeof(Target))] 
        private List<Target> _targets = new List<Target>();
       

        private Dictionary<string, int> _targetPriorities = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private IStringParser _parser;
        private IStringParser Parser
        {
            get
            {
                if (_parser == null)
                {
                    _parser = (IStringParser)this.Site.GetService(typeof(IStringParser));
                    if (_parser == null)
                    {
                        throw new ApplicationException("Missing IStringParserService.");
                    }
                }
                return _parser;
            }
        }

        private IConditionService _conditionService;
        private IConditionService ConditionService
        {
            get
            {
                if (_conditionService == null)
                {
                    _conditionService = (IConditionService)this.Site.GetService(typeof(IConditionService));
                    if (_conditionService == null)
                    {
                        throw new ApplicationException("Missing IConditionService.");
                    }
                }
                return _conditionService;
            }
        }

        private ILogger  _logger;
        private ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = (ILogger)this.Site.GetService(typeof(ILogger));
                }
                return _logger;
            }
        }
 

        private TaskItem AddTaskItem(string name, string category)
        {
            TaskItemGroup group;
            if (this._itemGroups.Count > 0 && String.IsNullOrEmpty(this._itemGroups.Last().Condition))
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
            this._itemGroups.Add(rs);
            return rs;
        }

        private void RemoveTaskItemGroup(TaskItemGroup group)
        {
            this._itemGroups.Remove(group);
        }

        [XArray(Name="itemGroups")]
        [XArrayItem(Name="items", Type=typeof(TaskItemGroup))]
        private List<TaskItemGroup> _itemGroups = new List<TaskItemGroup>();

        private TaskItem[] GetItems() 
        {
            
                List<TaskItem> rs = new List<TaskItem>();
                foreach (TaskItemGroup group in this._itemGroups)
                {
                    foreach (TaskItem item in group)
                    {
                        rs.Add(item);
                    }
                }
                return rs.ToArray();
            
        }

        private TaskItem[] GetEvaluatedItemsByCatetory(string category)
        {
            List<TaskItem> rs = new List<TaskItem>();
            foreach (TaskItemGroup g in this._itemGroups)
            {
                if (ConditionService.Evaluate(g))
                {
                    var query = from i in g where String.Equals(i.Category, category, StringComparison.OrdinalIgnoreCase)  && ConditionService.Evaluate(i) select i;
                    rs.AddRange(query); 
                }
            }
            return rs.ToArray();
        }

        private TaskItem GetEvaluatedItemByName(string name)
        {

            TaskItem rs = (from g in this._itemGroups where ConditionService.Evaluate(g)
                          from item in g where  StringComparer.OrdinalIgnoreCase.Compare(name, item.Name ) == 0 && ConditionService.Evaluate(item) select item ).FirstOrDefault();

            return rs;
        }

        private void RemoveTaskItem(TaskItem item)
        {
            foreach (TaskItemGroup group in this._itemGroups)
            {
                int index = group.IndexOf(item);
                if (index >= 0)
                {
                    group.RemoveAt(index);
                    break;
                }
            }
        }

        #region IObjectWithSite Members
      

        public IServiceProvider Site {get;set;}
       

        #endregion

        private IJobEngine Engine
        {
            get
            {
                return (IJobEngine)this.Site.GetService(typeof(IJobEngine));  
            }
        }

        internal void Initialize()
        {
            this.LoadAssembly(Assembly.GetExecutingAssembly());
        }

        private List<Assembly> _loadedAssebmlies = new List<Assembly>();
        private Dictionary<XName, Type> _loadedTaskOrInstructionTypes = new Dictionary<XName, Type>();

        private Type ResolveInstructionType(XName name)
        {
            
            Type rs;
            if (this._loadedTaskOrInstructionTypes.TryGetValue(name, out rs))
            {
                return rs;
            }
            else
            {
                throw new InvalidJobTemplateException(String.Format(Fantasy.Jobs.Properties.Resources.UnknownTaskText, name.NamespaceName, name.LocalName));  
            }

            
        }

        private bool LoadAssembly(Assembly assembly)
        {
            if (_loadedAssebmlies.IndexOf(assembly) < 0)
            {
                this._loadedAssebmlies.Add(assembly);
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
            
            IEnumerable<Type> tasks;
            try
            {
                tasks = from type in assembly.GetTypes() where type.GetCustomAttributes(typeof(TaskAttribute), false).Length == 1 select type;
            }
            catch(ReflectionTypeLoadException error)
            {
                string message = string.Format("Cannot get types from assembly {0}. LoaderExceptions:\n {1}", assembly.FullName, string.Join("\n", error.LoaderExceptions.Select(e=>e.Message)));
                throw new JobException(message, error);
            }
            foreach (Type t in tasks)
            {
                TaskAttribute ta = (TaskAttribute)t.GetCustomAttributes(typeof(TaskAttribute), false)[0];
              

                XName key = (XNamespace)ta.NamespaceUri + ta.Name;
               

                if (!_loadedTaskOrInstructionTypes.ContainsKey(key))
                {
                    this._loadedTaskOrInstructionTypes.Add(key, t);
                }
                else
                {
                    throw new InvalidJobTemplateException(String.Format(Fantasy.Jobs.Properties.Resources.DulplicateTaskText, t, this._loadedTaskOrInstructionTypes[key], ta.NamespaceUri, ta.Name));   
                     
                }
            }

            var instructions = from type in assembly.GetTypes() where type.IsDefined(typeof(InstructionAttribute), false) && type.IsDefined(typeof(XSerializableAttribute), false)  select type;
            foreach (Type t in instructions)
            {
                XSerializableAttribute sa = (XSerializableAttribute)t.GetCustomAttributes(typeof(XSerializableAttribute), false)[0];
                XName key = (XNamespace)sa.NamespaceUri + sa.Name;
                if (!_loadedTaskOrInstructionTypes.ContainsKey(key))
                {
                    this._loadedTaskOrInstructionTypes.Add(key, t);
                }
                else
                {
                    throw new InvalidJobTemplateException(String.Format(Fantasy.Jobs.Properties.Resources.DulplicateTaskText, t, this._loadedTaskOrInstructionTypes[key], sa.NamespaceUri, sa.Name));   
                }
            }

        }


        #region Build from StartInfo Methods




        internal void LoadStartInfo(JobStartInfo startInfo)
        {
            
            this.LoadAssembly(Assembly.GetExecutingAssembly());
            this.AddPropertiesFromStartInfo(startInfo);
            this.AddItemsFromStartInfo(startInfo);
            this._templateName = startInfo.Template; 
            IJobTemplatesService ts = (IJobTemplatesService)this.Site.GetService(typeof(IJobTemplatesService));
            JobTemplate template = ts.GetJobTemplateByName(startInfo.Template);
            if (!string.IsNullOrEmpty(startInfo.Target))
            {
                this._startupTarget = startInfo.Target;
            }
            else
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(template.Content);
                this._startupTarget = doc.DocumentElement.GetAttribute("defaultTarget"); 
            }

            this.AddTemplate(template, 1);
        }

        private void AddTemplate(JobTemplate template, int priority)
        {
            XElement root = XElement.Parse(template.Content);
          

            foreach (XElement element in root.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "import":
                        this.AddImportFromTemplate(element, template);
                        break;
                    case "include":
                        this.AddInlcude(element, template, priority);
                        break;
                    case "properties":
                        this.AddPropertiesFromTemplate(element, priority);
                        break;
                    case "target":
                        this.AddTargetFrom(element, priority);
                        break;
                    case "items" :
                        this.AddItemsFromTemplate(element);
                        break;
                }
            }
        }

        private void AddItemsFromTemplate(XElement element)
        {
            XSerializer ser = new XSerializer(typeof(TaskItemGroup));
            TaskItemGroup group = (TaskItemGroup)ser.Deserialize(element);
            foreach (TaskItem item in group)
            {
                item.Name = this.Parser.Parse(item.Name);
            }
            this._itemGroups.Add(group);
        }

        private void AddTargetFrom(XElement element, int priority)
        {
           
            int oldPri;
            bool needAdd = false;
            string targetName = (string)element.Attribute(Target.NameAttributeName);
            if (this._targetPriorities.TryGetValue(targetName , out oldPri))
            {
                if (priority <= oldPri)
                {
                    Target oldTarget = (from tgt in this._targets where StringComparer.OrdinalIgnoreCase.Compare(tgt.Name, targetName) == 0 select tgt).Single();
                    this._targets.Remove(oldTarget);
                    needAdd = true;
                }

            }
            else
            {
                needAdd = true;
                
            }

            if (needAdd)
            {
                XSerializer ser = new XSerializer(typeof(Target)) { Context = this.Site };
                
                Target target = new Target();
               
                ser.Deserialize(element, target);
                this._targets.Add(target);
                this._targetPriorities[target.Name] = priority;
            }

        }

        private void AddPropertiesFromTemplate(XElement element, int priority)
        {
            XSerializer ser = new XSerializer(typeof(JobPropertyGroup));
            JobPropertyGroup group = (JobPropertyGroup)ser.Deserialize(element);
            if (ConditionService.Evaluate(group))
            {
                foreach (JobProperty property in group)
                {
                    if (ConditionService.Evaluate(property))
                    {
                        this.TryAddProperty(property.Name, property.Value, priority); 
                    }
                }
            }
        }



        private void AddInlcude(XElement element, JobTemplate template, int priority)
        {
            JobTemplate newTemplate;
            IJobTemplatesService ts = (IJobTemplatesService)this.Site.GetService(typeof(IJobTemplatesService));
            string name = (string)element.Attribute("name");
            if (!string.IsNullOrEmpty(name))
            {
                newTemplate = ts.GetJobTemplateByName(name);
            }
            else
            {
                string path = (string)element.Attribute("path");
                if (!string.IsNullOrEmpty("path"))
                {
                    path = Parser.Parse(path);
                    if (!System.IO.Path.IsPathRooted(path))
                    {
                        string dir = System.IO.Path.GetDirectoryName(template.Location);
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
            XSerializer ser = new XSerializer(typeof(ImportAssembly));
            ImportAssembly import = (ImportAssembly)ser.Deserialize(element);
            import.Src = Path.GetDirectoryName(template.Location);
            Assembly assembly = import.LoadAssembly(this.Parser);
            if (this.LoadAssembly(assembly))
            {
                this._imports.Add(import);
            }
        }

       

        private Dictionary<string, int> _propPriorities = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private void AddPropertiesFromStartInfo(JobStartInfo startInfo)
        {
            
            foreach (JobPropertyGroup group in startInfo.PropertyGroups)
            {
                if (ConditionService.Evaluate(group))
                {
                    foreach (JobProperty property in group)
                    {
                        if (ConditionService.Evaluate(group))
                        {
                            this.TryAddProperty(property.Name, property.Value, 0);
                        }
                    }
                }
            }
        }

        private void AddItemsFromStartInfo(JobStartInfo startInfo)
        {
            

            foreach (TaskItemGroup group in startInfo.ItemGroups)
            {
                foreach (TaskItem item in group)
                {
                    item.Name = this.Parser.Parse(item.Name);
                }

                this._itemGroups.Add(group);
            }
        }

        private void TryAddProperty(string name, string value, int priority)
        {
           
            
            int old;
            if (this._propPriorities.TryGetValue(name, out old))
            {
                if (old >= priority)
                {
                    this._propPriorities[name] = priority;
                    this.SetProperty(name, value);
                }

            }
            else
            {
                this._propPriorities.Add(name, priority);
                this.SetProperty(name, value); 
            }

          
        }
        #endregion

       

        private void ExecuteInstruction(IInstruction instruction)
        {
            if (instruction == null)
            {
                throw new ArgumentNullException("instruction"); 
            }
            instruction.Site = this.Site;
            this._runtimeStatus.PushStack();
            try
            {
                try
                {
                    instruction.Execute();
                }
               
                catch (Exception ex)
                {

                    this.Engine.SaveStatusForError(ex);
                    if (!(ex is ThreadAbortException))
                    {
                        throw;
                    }
                }
            }
            finally
            {
                this._runtimeStatus.PopStack();
            }
        }

        private void ExecuteTarget(string targetName)
        {
            Logger.SafeLogMessage("Engine", "Execute");
            Target target = (from tgt in this._targets where StringComparer.OrdinalIgnoreCase.Compare(tgt.Name, targetName) == 0 select tgt).SingleOrDefault();

            if (target == null)
            {
                throw new JobException(String.Format(Fantasy.Jobs.Properties.Resources.UndefinedTargetText, targetName));
            }
            this.ExecuteInstruction(target);

        }

        private void Execute()
        {
            if (string.IsNullOrEmpty(this._startupTarget))
            {
                throw new InvalidJobStartInfoException(string.Format(Fantasy.Jobs.Properties.Resources.NoStartupTargetText, this._templateName));
            }
            IResourceService resSvc = this.Engine.GetService<IResourceService>();
            IResourceHandle resHandle = null;
            if (resSvc != null)
            {

                ResourceParameter[] res = CreateExecuteResourceParameters();
                if (res.Length > 0)
                {
                    resHandle = resSvc.Request(res);
                    this.SetProperty("WaitAll", String.Empty);
                    this.SetProperty("WaitAny", String.Empty);
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
                    resHandle.Dispose(); 
                }
            }
        }


        private bool _isNested = false;

        internal bool IsNested
        {
            get { return _isNested; }
            set { _isNested = value; }
        }


        private ResourceParameter[] CreateExecuteResourceParameters()
        {
            List<ResourceParameter> rs = new List<ResourceParameter>();
            if (! IsNested)
            {
                rs.Add(new ResourceParameter("RunJob", new Dictionary<string, string>() { {"template", this._templateName }}));
            }
            IStringParser parser = this.Engine.GetRequiredService<IStringParser>();
            string waitAll = parser.Parse(this.GetProperty("WaitAll"));

            if (!String.IsNullOrEmpty(waitAll))
            {
                rs.Add(new ResourceParameter("WaitFor", new Dictionary<string, string>() { { "jobs", waitAll }, {"mode" , WaitForMode.All.ToString()} }));
            }

            string waitAny = parser.Parse(this.GetProperty("WaitAny"));

            if (!String.IsNullOrEmpty(waitAny))
            {
                rs.Add(new ResourceParameter("WaitFor", new Dictionary<string, string>() { { "jobs", waitAll }, { "mode", WaitForMode.Any.ToString() } }));
            }

            return rs.ToArray();
        }


        internal void LoadStatus(XElement element)
        {
           
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(new NameTable());
            nsMgr.AddNamespace("cvj", Consts.XNamespaceURI);
            XSerializer impSer = new XSerializer(typeof(ImportAssembly));

            foreach (XElement ele in element.XPathSelectElements("/cvj:job/cvj:imports/cvj:import", nsMgr))
            {
                ImportAssembly ia = (ImportAssembly)impSer.Deserialize(ele);
                this.LoadAssembly(ia.LoadAssembly(null));
            }

            XSerializer jobser = new XSerializer(typeof(Job)) { Context = this.Site };

            jobser.Deserialize(element, this);

        }

        internal XElement SaveStatus()
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); 
            XSerializer ser = new XSerializer(typeof(Job));
            XElement rs = ser.Serialize(this,ns);
            return rs;
        }


        #region IJob Members

        TaskItem IJob.AddTaskItem(string name, string category)
        {
            return this.AddTaskItem(name, category);
        }

        void IJob.RemoveTaskItem(TaskItem item)
        {
            this.RemoveTaskItem(item);
        }

        TaskItemGroup IJob.AddTaskItemGroup()
        {
            return this.AddTaskItemGroup();
        }

        void IJob.RemoveTaskItemGroup(TaskItemGroup group)
        {
            this.RemoveTaskItemGroup(group);
        }

        TaskItem[] IJob.GetEvaluatedItemsByCatetory(string category)
        {
            return this.GetEvaluatedItemsByCatetory(category);
        }

        TaskItem IJob.GetEvaluatedItemByName(string name)
        {
            return this.GetEvaluatedItemByName(name);
        }

        TaskItem[] IJob.Items
        {
            get { return this.GetItems() ; }
        }

        Guid IJob.ID
        {
            get { return this._id; }
        }

        string IJob.TemplateName
        {
            get { return this._templateName; }
        }

        JobProperty[] IJob.Properties
        {
            get { return this._properties.ToArray(); }
        }

        string IJob.GetProperty(string name)
        {
            return this.GetProperty(name);
        }

        void IJob.SetProperty(string name, string value)
        {
            this.SetProperty(name, value);
        }

        bool IJob.HasProperty(string name)
        {
            return this.HasProperty(name);
        }

        void IJob.RemoveProperty(string name)
        {
            this.RemoveProperty(name);
        }

        string IJob.StartupTarget
        {
            get
            {
                return this._startupTarget;
            }
            set
            {
                this._startupTarget = value;
            }
        }

        Type IJob.ResolveInstructionType(XName name)
        {
            return this.ResolveInstructionType(name);
        }

        RuntimeStatus IJob.RuntimeStatus
        {
            get { return this._runtimeStatus; }
        }

        void IJob.Execute()
        {
            this.Execute();
        }

        void IJob.ExecuteInstruction(IInstruction instruction)
        {
            this.ExecuteInstruction(instruction);
        }

        void IJob.ExecuteTarget(string targetName)
        {
            this.ExecuteTarget(targetName);
        }

        #endregion
    }

}
