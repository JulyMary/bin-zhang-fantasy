using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using System.ComponentModel.Design;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class IsSelectedSystemEntity : ConditionBase
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
                rs = ss.GetSelectedComponents().FilterAndCast<IBusinessEntityGlyph>().Any(g => g.Entity.IsSystem);

            }

            return rs;
        }
    }
}
