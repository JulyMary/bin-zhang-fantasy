using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationEditor :  EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/applicationeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/applicationeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Entity != null ? ((BusinessApplication)this.Entity).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "application"; }
        }
    }
}
