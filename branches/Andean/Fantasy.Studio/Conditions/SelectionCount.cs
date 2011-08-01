using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;

namespace Fantasy.Studio.Conditions
{
    public class SelectionCount : ConditionBase
    {
        public override bool IsValid(object args)
        {

            int selected = 0;

            IMonitorSelectionService mss = ServiceManager.Services.GetRequiredService<IMonitorSelectionService>();
            ISelectionService ss = mss.CurrentSelectionService;

            if (ss != null)
            {
                selected = ss.SelectionCount; 
            }

            switch (this.Count)
            {
                case "+":
                    return selected > 0;
                case "?":
                    return selected <= 1;
                default:
                    int c = Int32.Parse(this.Count);
                    return selected == c;
            }
            
        }

        public string Count { get; set; }
    }
}
