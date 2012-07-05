using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Jobs.Web.Models
{
    public class ScheduleGroup
    {

        public ScheduleGroup()
        {
            this.ChildGroups = new List<ScheduleGroup>();
        }

        public string DisplayName { get; set; }

        public string Path { get; set; }

        public List<ScheduleGroup> ChildGroups { get; private set; }

    }
}