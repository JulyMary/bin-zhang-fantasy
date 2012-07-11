using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml;
using System.Xml.Linq;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    [Instruction]
    [XSerializable("callTemplate", NamespaceUri = Consts.XNamespaceURI)]
    internal class CallTemplate : AbstractInstruction, IConditionalObject, IXSerializable
    {
       
        public override void Execute()
        {
            IConditionService conditionSvc = (IConditionService)this.Site.GetService(typeof(IConditionService));
            ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
            IJobEngine engine = this.Site.GetRequiredService<IJobEngine>();
           
            if (conditionSvc.Evaluate(this))
            {
                IJob parentJob = this.Site.GetRequiredService<IJob>();

                ServiceContainer childSite = new ServiceContainer();

                _job = new Job() { IsNested = true };
                _job._id = parentJob.ID;

                List<object> services = new List<object>();
                services.AddRange(AddIn.CreateObjects("jobEngine/job.services/service"));
                services.Add(_job);

                childSite.InitializeServices(this.Site, services.ToArray());
                
                _job.Initialize();
                if (_jobElement == null || !parentJob.RuntimeStatus.Local.GetValue("jobinitialized", false))
                {
                    JobStartInfo si = new JobStartInfo();
                    si.ID = engine.JobId;
                    si.Name = parentJob.GetProperty("Name");
                    si.Template = this.Template;
                    si.Target = this.Target;

                    AddInputs(si);

                    _job.LoadStartInfo(si);
                    parentJob.RuntimeStatus.Local["jobinitialized"] = true;
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
                logger.LogMessage(LogCategories.Instruction, "Skip callTemplate {0}", this.Template);
            }

        }

        private void SetOutputParameters()
        {
            IItemParser itemParser = this._job.Site.GetRequiredService<IItemParser>();
            IStringParser strParser = this._job.Site.GetRequiredService<IStringParser>();
            IJob parentJob = this.Site.GetRequiredService<IJob>();
            TaskItemGroup group = null;
            foreach (CallTemplateParameter output in this._outputs)
            {
                if (!String.IsNullOrWhiteSpace(output.ItemCategory))
                {
                    TaskItem[] srcItems = itemParser.ParseItem(output.Include);
                    foreach (TaskItem srcItem in srcItems)
                    {
                        if(group == null)
                        {
                            group = parentJob.AddTaskItemGroup();
                        }
                        TaskItem item = group.AddNewItem(srcItem.Name, output.ItemCategory);
                        srcItem.CopyMetaDataTo(item);
                    }
 
                }
                else if (!string.IsNullOrWhiteSpace(output.PropertyName))
                {
                    string value = strParser.Parse(output.Value);
                    parentJob.SetProperty(output.PropertyName, value);
                }
                else
                {
                    throw new JobException(Fantasy.Jobs.Properties.Resources.InvalidOutputText);
                }
            }
        }

        private void AddInputs(JobStartInfo si)
        {
            IItemParser itemParser = this.Site.GetRequiredService<IItemParser>();
            IStringParser strParser = this.Site.GetRequiredService<IStringParser>();
            TaskItemGroup itemGroup = new TaskItemGroup();
            IConditionService conditionSvc = this.Site.GetRequiredService<IConditionService>();
            si.ItemGroups.Add(itemGroup);

            JobPropertyGroup propGroup = new JobPropertyGroup();
            si.PropertyGroups.Add(propGroup);

            foreach (CallTemplateParameter input in _inputs)
            {
                if (conditionSvc.Evaluate(input))
                {
                    if (!string.IsNullOrWhiteSpace(input.ItemCategory))
                    {
                        TaskItem[] srcItems = itemParser.ParseItem(input.Include);
                        foreach (TaskItem srcItem in srcItems)
                        {
                            TaskItem item = itemGroup.AddNewItem(srcItem.Name, input.ItemCategory);
                            srcItem.CopyMetaDataTo(item);
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(input.PropertyName))
                    {
                        string value = strParser.Parse(input.Value);
                        JobProperty prop = propGroup.AddNewItem(input.PropertyName);
                        prop.Value = value;
                    }
                    else
                    {
                        throw new JobException(Fantasy.Jobs.Properties.Resources.InvalidCallTemplateInputText);
                    }
                }
            }
        }

        [XAttribute("template")]
        public string Template { get; set; }

        [XAttribute("target")]
        public string Target { get; set; }


        [XArray]
        [XArrayItem(Name = "input", Type = typeof(CallTemplateParameter))]
        private IList<CallTemplateParameter> _inputs = new List<CallTemplateParameter>();

        public IList<CallTemplateParameter> Inputs
	    {
            get { return _inputs; }
	    }

        [XArray]
        [XArrayItem(Name = "output", Type = typeof(CallTemplateParameter))]
        private IList<CallTemplateParameter> _outputs = new List<CallTemplateParameter>();

        public IList<CallTemplateParameter> Outputs
        {
            get { return _outputs; }
        }

        private Job _job = null;
        private XElement _jobElement = null;
        

        [XAttribute("condition")]
        public string Condition { get; set; }

#pragma  warning disable 169
        [XNamespace]
        private XmlNamespaceManager _namespaces;
#pragma warning restore 169

        #region IXSerializable Members

        public void Load(IServiceProvider context, XElement element)
        {
            XHelper.Default.LoadByXAttributes(context, element, this);
            XName name = (XNamespace)Consts.XNamespaceURI + "job";
            _jobElement = element.Element(name);
        }

        public void Save(IServiceProvider context, XElement element)
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

        #endregion
    }
}
