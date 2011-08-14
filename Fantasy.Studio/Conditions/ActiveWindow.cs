using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.Studio.Conditions
{
    public class ActiveWindow : ConditionBase
    {
        public ActiveWindow()
        {
            this.Type = "*";
        }

        public override bool IsValid(object args, IServiceProvider services)
        {
            IWorkbench wb = services.GetRequiredService<IWorkbench>();
            if (wb.ActiveWorkbenchWindow == null)
            {
                return String.IsNullOrEmpty(this.Type);
            }
            else if (this.Type == "*")
            {
                return true;
            }
            else
            {
                return string.Equals(this.Type, wb.ActiveWorkbenchWindow.ViewContent.DocumentType, StringComparison.OrdinalIgnoreCase);
            }
        }

        public string Type { get; set; }
    }
}
