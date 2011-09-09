using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramAddBusinessEnumCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {
            ISelectionService selection = this.Site.GetRequiredService<ISelectionService>();
            Model.EnumGlyph node = (Model.EnumGlyph)selection.PrimarySelection;
            return new EnumEditing.AddEnumValueCommand() { Site = this.Site }.Execute(node.Entity);
        }

        #endregion
    }
}
