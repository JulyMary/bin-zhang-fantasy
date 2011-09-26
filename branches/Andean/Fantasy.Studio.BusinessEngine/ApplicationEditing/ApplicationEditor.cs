﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationEditor :  EntityEditingViewContent
    {

        public ApplicationEditor()
        {

        }

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
            get { return this.Data != null ? ((BusinessApplication)this.Data).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "application"; }
        }
    }
}
