using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{
    [Task("createDirectory", Consts.XNamespaceURI, Description="Create directories")]
    public class CreateDirectoryTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {
            if (Path != null && Path.Length > 0)
            {
                ILogger logger = this.Site.GetService<ILogger>();
                foreach (string path in this.Path)
                {
                    if (!LongPathDirectory.Exists(path))
                    {
                        LongPathDirectory.Create(path);
                        if (logger != null)
                        {
                            logger.LogMessage("createDirectory", "Create directory {0}.", path);
                        }

                    }
                    else
                    {
                        logger.LogWarning("createDirectory", MessageImportance.Low, "Directory {0} already exists.", path);
                    }
                }

            }

            return true;
        }

        #endregion


        [TaskMember("path", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of directory path to create." )]
        public string[] Path { get; set; }
    }
}
