using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Tasks
{
    [Task("createItem", Consts.XNamespaceURI, Description="Create job service items. This task is deprecated. Please use 'items' element in instructions to create new items")]
    public class CreateItemTask : ObjectWithSite, ITask
    {
        public CreateItemTask()
        {
            this.PreserveExistingMetadata = true;

        }
        #region ITask Members

        public bool Execute()
        {
            List<TaskItem> items = new List<TaskItem>();
            List<string[]> additional = new List<string[]>();
            if (this.AdditionalMetadata != null)
            {
                foreach (string expr in this.AdditionalMetadata)
                {
                    string[] kv = expr.Split(new char[] {'='}, 2);
                    if (kv.Length == 1)
                    {
                        kv = new string[] { kv[0], string.Empty };
                    }
                    additional.Add(kv);
                }
            }

            foreach (TaskItem src in this.Include)
            {
                if (!IsExclude(src.Name))
                {
                    TaskItem newItem = new TaskItem() { Name = src.Name };
                    if (this.PreserveExistingMetadata)
                    {
                        src.CopyMetaDataTo(newItem); 
                    }

                    foreach (string[] kv in additional)
                    {
                        src[kv[0]] = src[kv[1]];
                    }
                   
                }
            }

            this.Include = items.ToArray();

            return true;
        }


        private bool IsExclude(string name)
        {
            if (Exclude != null)
            {
                var query = from item in Exclude where StringComparer.OrdinalIgnoreCase.Compare(item.Name, name) == 0 select item;
                return query.Any();
            }
            return false;
        }

        #endregion


        [TaskMember("include", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required | TaskMemberFlags.Output)] 
        public TaskItem[] Include { get; set; }


        [TaskMember("exclude")]
        public TaskItem[] Exclude { get; set; }

        [TaskMember("preserveExistingMetadata")]
        public bool PreserveExistingMetadata { get; set; }

        [TaskMember("additionalMetadata")] 
        public string[] AdditionalMetadata { get; set; }
    }
}
