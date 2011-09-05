using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumEditor : EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/enumeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/enumeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Entity != null ? ((BusinessEnum)this.Entity).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "enum"; }
        }
    }
}
