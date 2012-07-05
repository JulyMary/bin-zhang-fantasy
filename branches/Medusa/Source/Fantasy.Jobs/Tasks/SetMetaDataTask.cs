using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Tasks
{
    [Task("setMetaData", Consts.XNamespaceURI, Description="Set meta data for input items")]
    public class SetMetaDataTask : ObjectWithSite, ITask
    {
        #region ITask Members

        public bool Execute()
        {
            if (!String.IsNullOrEmpty(this.Items) && Values != null)
            {
                IItemParser parser = this.Site.GetRequiredService<IItemParser>();
                TaskItem[] items = parser.ParseItem(this.Items);

                ILogger logger = this.Site.GetService<ILogger>();
                for (int i = 0; i < items.Length && i < this.Values.Length; i++)
                {
                    items[i][this.Name] = Values[i];
                    if (logger != null)
                    {
                        logger.LogMessage("setMetaData", "set item {0}'s meta data {1} as {2}", items[i].Name, this.Name, Values[i]);
                    }
                }
            }

            return true;
        }

        #endregion


        [TaskMember("items", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of item to set metadata for." )]
        public string Items { get; set; }

        [TaskMember("name", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="Name of metadata")]
        public string Name { get; set; }

        [TaskMember("values", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of metadata value to set. If values only contains one element, all items will be set with the same value; otherwise values will be set to with coresponding order of items." )]
        public string[] Values { get; set; }

    }
}
