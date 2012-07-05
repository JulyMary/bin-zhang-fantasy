using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Jobs.Properties;
using System.Threading;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{

    [Task("log", Consts.XNamespaceURI, Description="Write log")] 
    public class LogTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {
            ILogger logger = (ILogger)this.Site.GetService(typeof(ILogger));
            if (Message == null)
            {
                Message = string.Empty;
            }
            if (logger != null)
            {
                logger.LogMessage(this.Categroy, this.Importance, Message);
            }
            
            return true;
        }

        #endregion

        [TaskMember("message", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Log message.")] 
        public string Message { get; set; }

        
        private string _category = LogCategories.LogTask;

        [TaskMember("category", Description="Log category.")]
        public string Categroy
        {

            get { return _category; }
            set { _category = value; }
        }

        
        private MessageImportance _importance = MessageImportance.Normal;

        [TaskMember("importance", Description="Importance level of log message.")]
        public MessageImportance Importance
        {
            get { return _importance; }
            set { _importance = value; }
        }


        
    }
}
