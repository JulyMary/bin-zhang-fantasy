using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.Jobs.Management;
using System.Xml.Linq;

namespace Fantasy.Jobs.Web.Models
{
    public class JobLogModel
    {
        public JobMetaData Job { get; set; }
        public XElement Log { get; set; }
    }
}