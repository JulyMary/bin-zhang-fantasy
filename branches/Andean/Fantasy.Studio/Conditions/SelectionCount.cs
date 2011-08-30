using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Studio.Services;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace Fantasy.Studio.Conditions
{
    public class SelectionCount : ConditionBase
    {
        public override bool IsValid(object args, IServiceProvider services)
        {

            int selected = 0;

            ISelectionService ss = services.GetService<ISelectionService>();

            if (ss == null)
            {
                IMonitorSelectionService mss = services.GetRequiredService<IMonitorSelectionService>();
                ss = mss.CurrentSelectionService;
            }

            if (ss != null)
            {
                selected = ss.SelectionCount; 
            }

            Debug.WriteLine("Selected : " + selected.ToString());

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
