using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class UserEditor : EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/usereditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/usereditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Entity != null ? ((BusinessUser)this.Entity).Name : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "user"; }
        }
    }
}
