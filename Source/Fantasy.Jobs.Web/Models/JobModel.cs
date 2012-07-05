using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.Jobs.Management;

namespace Fantasy.Jobs.Web.Models
{
    public class JobModel
    {
        public JobMetaData MetaData { get; set; }

        public string Status { get; set; }

        public int Progress { get; set; }

        public string Script { get; set; }

        public string ImageUrl {get;set;}
    }
}