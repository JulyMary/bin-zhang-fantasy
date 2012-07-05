using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Fantasy.Jobs.Management;
using Fantasy.IO;

namespace Fantasy.Jobs.Web.Models
{
    public class ManagerLogModel
    {
        public DateTime Date { get; set; }

        public XElement Log { get; set; }

        public DateTime[] AvailableDates { get; set; }

        public DateTime MaxDate { get; set; }

        public DateTime MinDate { get; set; }

        public string SelectedServer { get; set; }

        public string[] Servers { get; set; }
       


    }
}