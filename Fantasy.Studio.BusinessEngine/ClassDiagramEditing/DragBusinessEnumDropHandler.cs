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
    public class DragBusinessEnumDropHandler  : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {

            be.BusinessEnum @enum = (be.BusinessEnum)e.Data.GetData(typeof(be.BusinessEnum));
            if (@enum != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;


                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
                Point p = e.GetPosition(view.Page);

                Model.EnumGlyph node = new Model.EnumGlyph() { Id = Guid.NewGuid(), Left = p.X, Top = p.Y, EnumId = @enum.Id, Entity = @enum, ShowMember = false};
                diagram.Enums.Add(node);

                ISelectionService sel = this.Site.GetService<ISelectionService>();
                sel.SetSelectedComponents(new object[] { node }, SelectionTypes.Replace);
            }
        }

        #endregion
    }
   
}
