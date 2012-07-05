using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{

    [Task("error", Consts.XNamespaceURI, Description="Throw an error immediately")]
    public class ErrorTask : ObjectWithSite, ITask
    {

        private string _category = LogCategories.CustomError;
        [TaskMember("category", Description="Error category")]
        public string Categroy
        {

            get { return _category; }
            set { _category = value; }
        }

        [TaskMember("message", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Error message")]
        public string Message { get; set; }

        #region ITask Members

        public bool Execute()
        {
            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogError(this.Categroy, this.Message);  
            }

            return false;
        }

        #endregion
    }
}
