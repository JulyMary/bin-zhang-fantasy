using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ClassEditor : EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/classeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/classeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Entity != null ? ((BusinessClass)this.Entity).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "class"; }
        }
    }
}
