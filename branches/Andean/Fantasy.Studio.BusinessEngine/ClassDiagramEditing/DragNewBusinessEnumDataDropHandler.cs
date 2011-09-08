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
    public class DragNewBusinessEnumDataDropHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            DragNewBusinessEnumData data = (DragNewBusinessEnumData)e.Data.GetData(typeof(DragNewBusinessEnumData));
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                Model.ClassDiagram diagram = this.Site.GetRequiredService<Model.ClassDiagram>();
                Point p = e.GetPosition(view.Page);
                be.BusinessClassDiagram cd = this.Site.GetRequiredService<be.BusinessClassDiagram>();

                be.BusinessEnum @enum = this.Site.GetRequiredService<IEntityService>().AddBusinessEnum(cd.Package);

                Model.EnumGlyph node = new Model.EnumGlyph() { Id = Guid.NewGuid(), Left = p.X, Top = p.Y, EnumId = @enum.Id, Entity = @enum, ShowMember = true};
                diagram.Enums.Add(node);
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
