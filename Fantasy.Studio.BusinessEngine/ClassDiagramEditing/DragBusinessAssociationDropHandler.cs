using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.Windows;
using be = Fantasy.BusinessEngine;
using Syncfusion.Windows.Diagram;
using System.ComponentModel.Design;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragBusinessAssociationDropHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {

            be.BusinessAssociation association = e.Data.GetDataByType<be.BusinessAssociation>();
            if (association != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;


                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
                Point p = e.GetPosition(view.Page);


                p = new Point(p.X , p.Y < 100 ? 0 : p.Y - 100);




                Model.ClassGlyph leftNode = new Model.ClassGlyph() { Id = Guid.NewGuid(), Left = p.X, Top = p.Y, ClassId = association.LeftClass.Id, Entity = association.LeftClass, ShowMember = false, ShowProperties = true, ShowRelations = true };
                diagram.Classes.Add(leftNode);

                Model.ClassGlyph rightNode = new Model.ClassGlyph() { Id = Guid.NewGuid(), Left = p.X, Top = p.Y + 200, ClassId = association.RightClass.Id, Entity = association.RightClass, ShowMember = false, ShowProperties = true, ShowRelations = true };
                diagram.Classes.Add(rightNode);


                Model.AssociationGlyph node = new Model.AssociationGlyph()
                {
                    LeftClass = leftNode,
                    LeftGlyphId = leftNode.Id,
                    RightClass = rightNode,
                    RightGlyphId = rightNode.Id,
                    EditingState = EditingState.Dirty,
                    Entity = association,
                    AssociationId = association.Id
                };

                diagram.Associations.Add(node);

                ISelectionService sel = this.Site.GetService<ISelectionService>();
                sel.SetSelectedComponents(new object[] {node }, SelectionTypes.Replace);
            }
        }

        #endregion
    }
}
