using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class AssociationEditor :  EntityEditingViewContent
    {
        public override string EditingPanelPath
        {
            get { return "fantasy/studio/businessengine/associationeditor/panels"; }
        }

        public override string CommandBindingPath
        {
            get { return "fantasy/studio/businessengine/associationeditor/commandbindings"; }
        }

        public override string DocumentName
        {
            get { return this.Data != null ? ((BusinessAssociation)this.Data).FullName : string.Empty; }
        }

        public override string DocumentType
        {
            get { return "association"; }
        }
    }
}
