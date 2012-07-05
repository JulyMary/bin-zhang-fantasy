using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Tasks
{
    [Task("filterItems",Consts.XNamespaceURI, Description="Divides exclude items from input items")]
    public class FilterItemsTask:  ObjectWithSite, ITask
    {
        [TaskMember("input",Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items whose elements are not also in exclude will be returned")]
        public TaskItem[] Input { get; set; }

        [TaskMember("exclude", Flags = TaskMemberFlags.Input | TaskMemberFlags.Required, Description="The list of items to remove form the source items")]
        public TaskItem[] Exclude { get; set; }

        [TaskMember("result", Flags = TaskMemberFlags.Output, Description="The list of items that contains the set difference of the items of input and exclude items." )]
        public TaskItem[] Result { get; set; }


        #region ITask Members

        public bool Execute()
        {
            if (Exclude == null)
            {
                Result = Input;
            }
            else
            {
                if (Input != null)
                {
                    var query = from item in Exclude
                                select item["fullname"];
                    List<string> exclude = query.ToList();
                    exclude.Sort(StringComparer.OrdinalIgnoreCase);
                    List<TaskItem> result = new List<TaskItem>();
                    foreach (TaskItem item in Input)
                    {
                        if (exclude.BinarySearch(item["fullname"], StringComparer.OrdinalIgnoreCase) < 0)
                        {
                            result.Add(item);
                        }
                    }
                    Result = result.ToArray();
                }
            }
            return true;
        }

        #endregion
    }
}
