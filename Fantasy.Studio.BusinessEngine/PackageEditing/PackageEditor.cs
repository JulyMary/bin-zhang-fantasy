using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class PackageEditor : EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/packageeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/packageeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Data != null ? ((BusinessPackage)this.Data).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "package"; }
        }
    }
}
