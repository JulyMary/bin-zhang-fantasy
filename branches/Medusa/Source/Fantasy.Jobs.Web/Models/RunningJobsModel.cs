using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Jobs.Web.Models
{
    public class RunningJobsModel
    {
        public JobModel[] Jobs { get; set; }

        public long Version { get; set; }

        public bool Expired { get; set; }
    }
}