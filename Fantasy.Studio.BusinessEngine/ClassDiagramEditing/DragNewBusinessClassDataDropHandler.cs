using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Syncfusion.Windows.Diagram;
using System.Windows.Controls;
using be = Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.ComponentModel.Design;
using System.Windows.Threading;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragNewBusinessClassDataDropHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            DragNewBusinessClassData data = (DragNewBusinessClassData)e.Data.GetData(typeof(DragNewBusinessClassData));
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
                Point p = e.GetPosition(view.Page);
                be.BusinessClassDiagram cd = this.Site.GetRequiredService<be.BusinessClassDiagram>();

                be.BusinessClass @class = this.Site.GetRequiredService<IEntityService>().AddBusinessClass(cd.Package);

                Model.ClassNode node = new Model.ClassNode() { Left = p.X, Top = p.Y, ClassId = @class.Id, Entity = @class, ShowMember = true, ShowProperties = true, ShowRelations = true };
                diagram.Classes.Add(node);
                ISelectionService sel = this.Site.GetService<ISelectionService>();
                if (sel != null)
                {
                    sel.SetSelectedComponents(new object[] { node }, SelectionTypes.Replace);
                }
            }
        }

        #endregion
    }
}
