using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    public class IsShortcut : ConditionBase
    {

        public override bool IsValid(object args, IServiceProvider services)
        {
            bool rs = false;
            IBusinessEntityGlyph glyph = args as IBusinessEntityGlyph;
            if (glyph != null)
            {
                rs = glyph.IsShortCut;
            }

            return rs;
        }
    }
}
