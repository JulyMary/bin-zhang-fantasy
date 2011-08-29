using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Syncfusion.Windows.Diagram;
using be = Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.ComponentModel.Design;
using System.Windows.Threading;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragBusinessClassDropHandler  : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {

            be.BusinessClass @class = (be.BusinessClass)e.Data.GetData(typeof(be.BusinessClass));
            if (@class != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;


                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
                Point p = e.GetPosition(view.Page);

                Model.ClassNode node = new Model.ClassNode() { Left = p.X, Top = p.Y, ClassId = @class.Id, Entity=@class, ShowMember=false, ShowProperties=true, ShowRelations=true };
                diagram.Classes.Add(node);

                ISelectionService sel = this.Site.GetService<ISelectionService>();
                sel.SetSelectedComponents(new object[] { node }, SelectionTypes.Replace);
            }
        }

        #endregion
    }
   
}
