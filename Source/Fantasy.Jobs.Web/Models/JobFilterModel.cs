using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Jobs.Web.Models
{
    public class JobFilterModel
    {
        public string Name { get; set; }

        public bool AdvancedOptionsVisible { get; set; }

        public int StateIndex { get; set; }
        public JobStateModel[] States { get; set; }

        public int TemplateIndex { get; set; }

        public string[] Templates { get; set; }

        public int ApplicationIndex { get; set; }

        public string[] Applications { get; set; }
        
        public int UserIndex { get; set; }

        public string[] Users { get; set; } 

        public DateTime? CreationTimeLow { get; set; }

        public DateTime? CreationTimeHigh { get; set; }

       

        //public DateTime? StartTimeLow { get; set; }

        //public DateTime? StartTimeHigh { get; set; }

        //public DateTime? EndTimeLow { get; set; }

        //public DateTime? EndTimeHigh { get; set; }

       

        //public bool IsTerminated { get; set; }

    }
}
