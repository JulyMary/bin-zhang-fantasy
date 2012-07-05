using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;


namespace Fantasy.Jobs.Tasks
{
    [Task("startNewJob", Consts.XNamespaceURI, Description = "Start new jobs")]
    public class StartNewJobTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {

            XElement xt = new XElement(this.Template);
            var includes = from items in xt.Elements() where items.Name.LocalName == "items"
                           from item in items.Elements()
                           where IncludeAttribute(item) != null
                           select item;

            // Traslate include to <item name="" />
            ExecuteTaskInstruction inst = this.Site.GetRequiredService<ExecuteTaskInstruction>();
            IItemParser itemParser = this.Site.GetRequiredService<IItemParser>();
            foreach (XElement include in includes.ToArray())
            {
                XElement items = include.Parent;
                include.Remove();
                TaskItem[] taskItems = itemParser.ParseItem((string)IncludeAttribute(include));
                string prefix = include.GetPrefixOfNamespace(include.Name.NamespaceName);
                string localName = include.Name.LocalName;
                XNamespace ns = include.Name.NamespaceName;
                foreach (TaskItem item in taskItems)
                {
                    XElement itemElement = new XElement(ns + localName);
                    TaskItem newItem = new TaskItem() { Name = item["fullname"] };
                    item.CopyMetaDataTo(newItem);
                    newItem.Save(this.Site, itemElement);
                    items.Add(itemElement);
                }
            }


            IStringParser strParser = this.Site.GetRequiredService<IStringParser>();
            xt = strParser.Parse(xt);    

            IJob job = this.Site.GetRequiredService<IJob>();
            SetProperty(xt, "application", job.GetProperty("application"));
            SetProperty(xt, "user", job.GetProperty("user"));
            SetProperty(xt, "parent", job.ID.ToString());

            string si = xt.ToString();

            IJobQueue queue = this.Site.GetRequiredService<IJobQueue>();
            JobMetaData child = queue.CreateJobMetaData();
            child.LoadXml(si);

            queue.ApplyChange(child);

            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("StartNewJob", "Start a new job {0} ({1}).", child.Name, child.Id); 
            }

            this.Id = child.Id;
            return true;
        }

        private void SetProperty(XElement template, string name, string value)
        {
            var query = from properties in template.Elements()
                        where properties.Name.LocalName == "properties"
                        from property in properties.Elements()
                        where string.Compare(property.Name.LocalName, name, true) == 0
                        select property;
            XElement prop = query.SingleOrDefault();
            if (prop == null)
            {
                var query2 = from properties in template.Elements()
                             where properties.Name.LocalName == "properties"
                             select properties;
                XElement xprops = query2.SingleOrDefault();
                if (xprops == null)
                {
                    xprops = new XElement(template.Name.Namespace + "properties");
                    template.Add(xprops);
                }

                prop = new XElement(template.Name.Namespace + name);
                xprops.Add(prop);
            }
            prop.Value = value;
        }

        private XAttribute IncludeAttribute(XElement element)
        {
            return element.Attributes().SingleOrDefault(a => String.Compare(a.Name.LocalName, "include", true) == 0);   
        }

        #endregion


        [TaskMember("jobStart", Flags= TaskMemberFlags.Input | TaskMemberFlags.Inline | TaskMemberFlags.Required, ParseInline=false,
            Description="A 'jobStart' element which contains job start information")]
        public XElement Template { get; set; }

        [TaskMember("id", Flags=TaskMemberFlags.Output, Description="The id of created job.")] 
        public Guid Id { get; set; }

    }
}
