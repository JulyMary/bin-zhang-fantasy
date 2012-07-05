using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Fantasy.Jobs.Tasks
{
    [Task("changeExtension", Consts.XNamespaceURI, Description="Change file extension for input files. Please note this task does not rename files. It is only for creating items with different extensios.")]
    public class ChangeExtensionTask : ObjectWithSite, ITask 
    {

        [TaskMember("include", Flags= TaskMemberFlags.Input | TaskMemberFlags.Output | TaskMemberFlags.Required,
            Description="The items to modify")]
        public TaskItem[] Include
        {
            get;
            set;
        }

        [TaskMember("extension", Flags=TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The new extension (with or without a leading period). Specity empty to remove an existing extension from path")]
        public string Extension { get; set; }

        [TaskMember("preserveExistingMetadata", Description="true if changeExtension task should copy items metadata to new created items; otherwise, false" )]
        public bool PreserveExistingMetadata { get; set; }

        #region ITask Members

        public bool Execute()
        {
            if (this.Include != null)
            {
                TaskItem[] items = new TaskItem[Include.Length];
                for (int i = 0; i < items.Length; i++)
                {
                    items[i] = new TaskItem() { Name = Path.ChangeExtension(Include[i].Name, this.Extension) };
                    if (this.PreserveExistingMetadata)
                    {
                        this.Include[i].CopyMetaDataTo(items[i]);
                    }
                }

                this.Include = items;
            }

            return true;
        }

        #endregion
    }
}
