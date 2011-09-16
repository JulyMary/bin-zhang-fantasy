using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.ComponentModel.Design;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class IsSelectedShortcut : ConditionBase
    {

        public override bool IsValid(object args, IServiceProvider services)
        {
            bool rs = false;

            ISelectionService ss = services.GetService<ISelectionService>();

            if (ss == null)
            {
                IMonitorSelectionService mss = services.GetRequiredService<IMonitorSelectionService>();
                ss = mss.CurrentSelectionService;
            }

            if (ss != null)
            {
                rs = ss.GetSelectedComponents().OfType<IBusinessEntityGlyph>().Any(g => g.IsShortCut);
                
            }

            return rs;



        }
    }
}
