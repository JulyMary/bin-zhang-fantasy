using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Telerik.Web.Mvc;

namespace Fantasy.Jobs.Web.Models
{
    public class JobsModel
    {
        public IEnumerable Data { get; set; }
        public int TotalCount { get; set; }
        public GridCommand Command { get; set; }
    }
}