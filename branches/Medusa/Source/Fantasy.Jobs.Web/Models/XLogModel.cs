using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Fantasy.Jobs.Web.Models
{
    public class XLogModel
    {
        public bool Pageable { get; set; }

        public XElement Log { get; set; }
    }
}