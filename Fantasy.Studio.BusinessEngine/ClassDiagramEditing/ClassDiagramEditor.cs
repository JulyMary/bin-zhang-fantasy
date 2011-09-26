using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramEditor : EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/classdiagrameditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/classdiagrameditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Data != null ? ((BusinessClassDiagram)this.Data).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "classdiagram"; }
        }
    }
}
