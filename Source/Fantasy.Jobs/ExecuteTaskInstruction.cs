using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Fantasy.XSerialization;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Fantasy.Jobs.Properties;
using System.Threading;
using Fantasy.ServiceModel;
using System.Xml.Linq;
using Fantasy.Jobs.Resources;
using Fantasy.IO;

namespace Fantasy.Jobs
{
    
    //public enum TaskExecutionLocation {InProcess, OutProcess}
    [XSerializable("executeTask", NamespaceUri= Consts.XNamespaceURI)]
    internal class ExecuteTaskInstruction : AbstractInstruction, IConditionalObject, IXSerializable
    {
        public ExecuteTaskInstruction()
        {
            
        }

        public string TaskName { get; set; }
        public string TaskNamespaceUri { get; set; }

        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        private List<XElement> _inlineElements = new List<XElement>();

        public string Condition { get; set; }

        #region IXSerializable Members

        public void Load(IServiceProvider context, XElement element)
        {
            this.TaskName = element.Name.LocalName;
            this.TaskNamespaceUri = element.Name.NamespaceName;

            this._xmlNamespaceManager = XHelper.CreateXmlNamespaceManager(element, true);
           
            foreach (XAttribute node in element.Attributes())
            {
                if (!string.IsNullOrEmpty(node.Value))
                {

                    switch (node.Name.LocalName)
                    {
                        case "condition":
                            this.Condition = node.Value;
                            break;
                        default:
                            if (!node.IsNamespaceDeclaration)
                            {
                                this._properties.Add(node.Name.LocalName, node.Value);
                            }
                           
                            break;
                    }
                }
            }

            

            XSerializer ser = new XSerializer(typeof(TaskOutput));

            XName outputName = (XNamespace)Consts.XNamespaceURI + "output";
            foreach (XElement child in element.Elements())
            {

                if (child.Name == outputName)
                {
                    TaskOutput output = (TaskOutput)ser.Deserialize(child);
                    this.TaskOutputs.Add(output);
                }
                else
                {
                    this._inlineElements.Add(child);
                }
            }

        }


        private XmlNamespaceManager _xmlNamespaceManager;

      

        private IList<TaskOutput> _taskOutputs = new List<TaskOutput>();

        public IList<TaskOutput>   TaskOutputs
        {
            get { return _taskOutputs; }
        }

        public void Save(IServiceProvider context, XElement element)
        {
            XHelper.AddNamespace(this._xmlNamespaceManager,element); 

            if (!String.IsNullOrEmpty(this.Condition))
            {
                element.SetAttributeValue("condition", this.Condition);
               
            }

            foreach (KeyValuePair<string, string> pair in this._properties)
            {
                element.SetAttributeValue(pair.Key, pair.Value);
            }

            XSerializer ser = new XSerializer(typeof(TaskOutput));
           
            foreach (XElement inline in this._inlineElements)
            {
                XElement newNode = new XElement(inline);
                element.Add(newNode); 
            }

            foreach (TaskOutput output in this.TaskOutputs)
            {
                XElement child = ser.Serialize(output);
                element.Add(child);
            }

        }
        #endregion

        public override void Execute()
        {
            IResourceService resSvc = this.Site.GetService<IResourceService>();
            if (resSvc != null)
            {
                ResourceParameter res = new ResourceParameter("RunTask", new { taskname = this.TaskName, @namespace = this.TaskNamespaceUri });
                using (IResourceHandle handle = resSvc.Request(new ResourceParameter[] { res }))
                {
                    InnerExecute();
                }
            }
            else
            {
                InnerExecute();
            }
        }

        private void InnerExecute()
        {
            IConditionService conditionSvc = (IConditionService)this.Site.GetService(typeof(IConditionService));
            ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));

            if (conditionSvc.Evaluate(this))
            {
                logger.LogMessage(LogCategories.Instruction, "Execute task {0}", this.TaskName);
                IJob job = (IJob)this.Site.GetService(typeof(IJob));
                Type t = job.ResolveInstructionType((XNamespace)this.TaskNamespaceUri + this.TaskName);
                ITask task = (ITask)Activator.CreateInstance(t);
                System.ComponentModel.Design.ServiceContainer site = new System.ComponentModel.Design.ServiceContainer(this.Site);
                site.AddService(typeof(ExecuteTaskInstruction), this);
                site.AddService(typeof(XmlNamespaceManager), this._xmlNamespaceManager);
               
                task.Site = site;

                this.SetInputParams(task);
                try
                {
                    bool success = task.Execute();

                    if (!success)
                    {
                        throw new TaskFailedException(String.Format(Properties.Resources.TaskFailedText, this.TaskName));
                    }
                }
                finally
                {
                    IStatusBarService statusBar = this.Site.GetService<IStatusBarService>();
                    if (statusBar != null)
                    {
                        statusBar.SetStatus("");
                    }
                }
                this.SetOutputParams(task);
            }
            else if(logger != null)
            {
                logger.LogMessage(LogCategories.Instruction, "Skip task {0}", this.TaskName);
            }
        }


        private void SetOutputParams(ITask task)
        {
            Func<MemberInfo, TaskMemberAttribute> getAttribute = mi => ((TaskMemberAttribute)mi.GetCustomAttributes(typeof(TaskMemberAttribute), true)[0]);

            Type taskType = task.GetType();
            IConditionService condition = (IConditionService)this.Site.GetService(typeof(IConditionService));
            foreach (TaskOutput output in this.TaskOutputs.Where(t => condition.Evaluate(t)))
            {
                MemberInfo mi = (from m in taskType.GetMembers( BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField  | BindingFlags.GetProperty )
                                 where m.IsDefined(typeof(TaskMemberAttribute), true) && (getAttribute(m).Flags  & TaskMemberFlags.Output) > 0 && getAttribute(m).Name == output.TaskParameter  
                            select m).SingleOrDefault() ;
                if(mi != null)
                {
                    Type memberType = this.GetMemberInfoType(mi);
                    if (!string.IsNullOrEmpty(output.ItemCategory))
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
                    throw new JobException(String.Format(Properties.Resources.UndefinedTaskOutputParameterText, this.TaskName, output.TaskParameter)); 
                }
 
            }
        }

        private void CreateJobProperty(ITask task, TaskOutput output, MemberInfo mi)
        {
            Type memberType = this.GetMemberInfoType(mi);
            StringBuilder text = new StringBuilder();
            MethodInvoker<object> AppendText = delegate (object o)
            {
                if (o != null)
                {
                    string s;
                    if (o is TaskItem)
                    {
                        s = ((TaskItem)o).Name;
                    }
                    else
                    {
                        TypeConverter cvt = XHelper.Default.CreateXConverter(o.GetType());
                        s = (string)cvt.ConvertFrom(o);
                    }
                    if (!string.IsNullOrEmpty(s))
                    {
                        if (text.Length > 0)
                        {
                            text.Append(";");
                        }
                        text.Append(s);
                    }
                }
            };

            object value = this.GetMemberInfoValue(mi, task);
            if (value != null)
            {
                if (value is Array)
                {
                    foreach (object ele in (Array)value)
                    {
                        AppendText(ele);
                    }
                }
                else
                {
                    AppendText(value);
                }
            }

            IJob job = (IJob)this.Site.GetService(typeof(IJob));
            job.SetProperty(output.PropertyName, text.ToString()); 

        }

        private void CreateTaskItems(ITask task, TaskOutput output, MemberInfo mi)
        {
            Type memberType = this.GetMemberInfoType(mi);

            TaskItem[] items = null;
            if (memberType == typeof(TaskItem))
            {
                TaskItem item = (TaskItem)this.GetMemberInfoValue(mi, task);
                if (item != null)
                {
                    items = new TaskItem[] { item };
                }
            }
            else if (memberType == typeof(TaskItem[]))
            {
                items = (TaskItem[])this.GetMemberInfoValue(mi, task); 
            }
            else if (memberType == typeof(string) || memberType == typeof(string[]) || typeof(IEnumerable<string>).IsAssignableFrom(memberType))
            {
                object v = this.GetMemberInfoValue(mi, task);

                if (v != null)
                {
                    IEnumerable paths;
                    if (memberType == typeof(string))
                    {
                        paths = ((string)v).Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    else
                    {
                        paths = (IEnumerable)v;
                    }

                    IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
                    List<TaskItem> list = new List<TaskItem>();
                    foreach (string path in paths)
                    {
                        string name;
                        if (LongPath.IsPathRooted(path))
                        {
                            name = LongPath.GetRelativePath(engine.JobDirectory + "\\", path);
                        }
                        else
                        {
                            name = path;
                        }
                        list.Add(new TaskItem() { Name = name});
                    }
                    items = list.ToArray();
                }
            }
            else
            {

                throw new JobException(String.Format(Properties.Resources.CannotConverTaskOutputParameterToTaskItemText, output.TaskParameter, this.TaskName));
            }

            if (items != null && items.Length > 0)
            {

                IJob job = (IJob)this.Site.GetService(typeof(IJob));
                TaskItemGroup group = job.AddTaskItemGroup(); 
                foreach (TaskItem item in items)
                {
                    TaskItem newItem = group.AddNewItem(item.Name, output.ItemCategory);
                    newItem.Condition = item.Condition;
                    item.CopyMetaDataTo(newItem);
                }
            }


        }

       

        private void SetInputParams(ITask task)
        {
            Type taskType = task.GetType(); 

            Func<MemberInfo, TaskMemberAttribute> getAttribute = mi =>((TaskMemberAttribute)mi.GetCustomAttributes(typeof(TaskMemberAttribute), true)[0]);

            var query = from mi in taskType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty)
                        where mi.IsDefined(typeof(TaskMemberAttribute), true) && (getAttribute(mi).Flags & TaskMemberFlags.Input) > 0
                        select new {Member = mi, Attribute = getAttribute(mi) };
            foreach (var m in query)
            {
                MemberInfo mi = m.Member;
                if (this._properties.ContainsKey(m.Attribute.Name))
                {
                    try
                    {
                        Type memberType = this.GetMemberInfoType(mi);

                        if (memberType == typeof(TaskItem))
                        {
                            SetTaskItemValue(task, mi, m.Attribute.Name);
                        }
                        else if (memberType == typeof(TaskItem[]))
                        {
                            SetTaskItemsValue(task, mi, m.Attribute.Name);
                        }
                        else if (memberType.IsArray)
                        {
                            SetArrayValue(task, mi, m.Attribute.Name);
                        }
                        else
                        {
                            SetScalarValue(task, mi, m.Attribute.Name);
                        }
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (Exception error)
                    {
                        throw new JobException(string.Format(Properties.Resources.SetTaskParamErrorText, this.TaskName, m.Attribute.Name), error);
                    }
                }
                else if ((m.Attribute.Flags & TaskMemberFlags.Inline) > 0 &&　this._inlineElements.Any(e=>String.Compare(e.Name.LocalName, m.Attribute.Name, true) == 0))
                {
                    Type memberType = this.GetMemberInfoType(mi);
                    if (memberType == typeof(XElement))
                    {
                        this.SetInlineValue(task, mi, m.Attribute.Name, m.Attribute.ParseInline);
                    }
                    else
                    {
                        throw new JobException(string.Format(Properties.Resources.InlineTaskMemberMustBeXElementText, this.TaskName, m.Attribute.Name));
                    }
                }
                else if ((m.Attribute.Flags & TaskMemberFlags.Required) > 0)
                {
                    throw new JobException(string.Format(Properties.Resources.RequireAttrubiteOfTaskText, m.Attribute.Name, this.TaskName));
                }
            }
        }

        private void SetInlineValue(ITask task, MemberInfo mi, string elementName, bool parseInline)
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            XElement element = this._inlineElements.Find(e => String.Compare(e.Name.LocalName, elementName, true) == 0);

            XElement val = parseInline ? parser.Parse(element) : new XElement(element);

            this.SetMemberInfoValue(mi, task, val); 
        }


        private void SetScalarValue(ITask task, MemberInfo mi, string param)
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            string str = parser.Parse(this._properties[param]);
            if (!string.IsNullOrEmpty(str))
            {
                TypeConverter cvt = XHelper.Default.CreateXConverter(this.GetMemberInfoType(mi));
                object value = cvt.ConvertTo(str, this.GetMemberInfoType(mi));
                this.SetMemberInfoValue(mi, task, value);
            }
        }

        private void SetArrayValue(ITask task, MemberInfo mi, string param)
        {

            Type memberType = this.GetMemberInfoType(mi);
            Type elementType = memberType.GetElementType();
            ArrayList temp = new ArrayList();
            IStringParser parser = (IStringParser)this.Site.GetService(typeof(IStringParser));
            string str = parser.Parse(this._properties[param]);
            if (!string.IsNullOrEmpty(str))
            {
               
                string[] strArr = str.Split(';');
                Array arr = Array.CreateInstance(elementType, strArr.Length);
                TypeConverter cvt = XHelper.Default.CreateXConverter(elementType);
                for (int i = 0; i < arr.Length; i++)
                {
                    object value = cvt.ConvertTo(strArr[i], elementType);
                    arr.SetValue(value, i);
                }

                this.SetMemberInfoValue(mi, task, arr); 
            }
        }

       
       
        private void SetTaskItemsValue(ITask task, MemberInfo mi, string param)
        {
            IItemParser parser = this.Site.GetRequiredService<IItemParser>();
            TaskItem[] items = parser.ParseItem(this._properties[param]);
            this.SetMemberInfoValue(mi, task, items);
        }

        private void SetTaskItemValue(ITask task, MemberInfo mi, string param)
        {
            IItemParser parser = this.Site.GetRequiredService<IItemParser>();
            TaskItem[] items = parser.ParseItem(this._properties[param]);
            if (items.Length == 1)
            {
                this.SetMemberInfoValue(mi, task, items[0]);
            }
            else if (items.Length > 1)
            {
                throw new JobException(String.Format(Properties.Resources.InputParamContainsMultipleItemsText, param, this.TaskName, items.Length));
            }
        }

        private Type GetMemberInfoType(MemberInfo mi)
        {
            if (mi.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)mi).FieldType;
            }
            else
            {
                return ((PropertyInfo)mi).PropertyType;  
            }
        }

        private void SetMemberInfoValue(MemberInfo mi,object instance, object value)
        {
            if (mi.MemberType == MemberTypes.Field)
            {
                ((FieldInfo)mi).SetValue(instance, value);
            }
            else
            {
                ((PropertyInfo)mi).SetValue(instance, value, null);
            }
        }

        private object GetMemberInfoValue(MemberInfo mi, object instance)
        {
            if (mi.MemberType == MemberTypes.Field)
            {
                return ((FieldInfo)mi).GetValue(instance);
            }
            else
            {
                return ((PropertyInfo)mi).GetValue(instance, null);
            }
        }
    }
}
