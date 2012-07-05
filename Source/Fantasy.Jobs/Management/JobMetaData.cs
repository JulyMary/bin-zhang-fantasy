using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;
using System.Xml.Linq;

namespace Fantasy.Jobs.Management
{
    partial class JobMetaData
    {

        internal bool IsTerminated
        {
            get
            {
                return (this.State & JobState.Terminated) == JobState.Terminated; 
            }
        }

        private string GetStartInfoProperty(XElement root, string name)
        {
            XElement element = (from props in root.Elements()
                                where props.Name.LocalName == "properties"
                                from prop in props.Elements()
                                where StringComparer.OrdinalIgnoreCase.Compare(prop.Name.LocalName, name) == 0
                                select prop).SingleOrDefault();

            return element != null ? (string)element : null;
        }


        public void LoadXml(string xml)
        {
            XElement root = XElement.Parse(xml);
          

            Guid id = (root.Attribute("id") != null && (string)root.Attribute("id") != string.Empty) ? (Guid)root.Attribute("id") : Guid.Empty;
            if (id == Guid.Empty)
            {
                id = Guid.NewGuid();
            }
            this.Id = id;
            this.Template = (string)root.Attribute("template");

            if (string.IsNullOrWhiteSpace(this.Template))
            {
                throw new JobException("Missing job template");
            }

            string name = this.GetStartInfoProperty(root, "name");

            this.Name = name ?? this.Template;

            try
            {

                this.Application = this.GetStartInfoProperty(root, "application");
            }
            catch
            {
                throw new JobException("Missing job application");
            }


            this.CreationTime = DateTime.Now;
            this.State = JobState.Unstarted;


            try
            {
                this.User = this.GetStartInfoProperty(root, "user");
            }
            catch
            {
                throw new JobException("Missing job user");
            }

            string sp = this.GetStartInfoProperty(root, "priority");
            int priority;
            if (sp != null && Int32.TryParse(sp, out priority))
            {
                this.Priority = priority;
            }

            this.StartInfo = xml;

            this.Tag = this.GetStartInfoProperty(root, "tag"); 
        }

    }
}
