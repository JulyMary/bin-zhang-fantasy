using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.ComponentModel.Design;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramAddBusinessPropertyCommand : ObjectWithSite, ICommand
    {

        #region ICommand Members

        public object Execute(object args)
        {
            ISelectionService selection = this.Site.GetRequiredService<ISelectionService>();
            Model.ClassNode node = (Model.ClassNode)selection.PrimarySelection;
            return new PropertyEditing.AddPropertyCommand() { Site = this.Site }.Execute(node.Entity);
        }

        #endregion
    }
}
