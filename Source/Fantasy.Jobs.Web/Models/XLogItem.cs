using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Models
{
    public class XLogItem
    {


        public string Type { get; set; }

        public DateTime Time { get; set; }

        public string Category { get; set; }

        public MessageImportance Importance { get; set; }

        public string Text { get; set; }

        public XElement Exception { get; set; }
    }
}