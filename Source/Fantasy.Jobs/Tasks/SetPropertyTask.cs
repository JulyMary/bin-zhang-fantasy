using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{

    [Task("property", Consts.XNamespaceURI, Description="Set property value. This task is deprecated. Please use 'properties' element in instruction to set property value")]
    public class SetPropertyTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {
            IStringParser parser = this.Site.GetRequiredService<IStringParser>();
            ILogger logger = this.Site.GetService<ILogger>(); 
            IJob job = this.Site.GetRequiredService<IJob>();

            string name = parser.Parse(this.Name);
            string value = parser.Parse(this.Value);

            job.SetProperty(name, value);
            if (logger != null)
            {
                logger.LogMessage("property", MessageImportance.Low,  "set property {0} as {1}", name, value);  
            }

            return true;
        }

        #endregion


        [TaskMember("name", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required )] 
        public string Name { get; set; }

        [TaskMember("value", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required)]
        public string Value { get; set; }
    }
}
