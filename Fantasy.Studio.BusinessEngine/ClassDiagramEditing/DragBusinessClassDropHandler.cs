using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Syncfusion.Windows.Diagram;
using be = Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class DragBusinessClassDropHandler  : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            be.BusinessClass  @class = (be.BusinessClass)e.Data.GetData(typeof(be.BusinessClass));
            if (@class != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;

                
                DiagramView view = this.Site.GetRequiredService<DiagramView>();
                DiagramModel model = this.Site.GetRequiredService<DiagramModel>();
                Point p = e.GetPosition(view.Page);

                be.BusinessClassDiagram cd = this.Site.GetRequiredService<be.BusinessClassDiagram>();

                Shapes.BusinessClassModel cm = new Shapes.BusinessClassModel() { Site = this.Site, Entity = @class, IsShortCut=cd.Package != @class.Package};
                Shapes.BusinessClass c = new Shapes.BusinessClass() { DataContext = cm, Site = this.Site };

                Node newNode = new Node(Guid.NewGuid())
                {
                    LabelVisibility = Visibility.Collapsed,
                    Height = double.NaN,
                    Width = 180,
                    OffsetX = p.X,
                    OffsetY = p.Y,
                    AllowRotate = false,
                    Content = c
                };

                newNode.SetResourceReference(FrameworkElement.StyleProperty, new ComponentResourceKey(typeof(Shapes.BusinessClass), "BusinessClassDSK"));
                model.Nodes.Add(newNode);
                view.SelectionList.Clear();
                view.SelectionList.Add(newNode);
                (newNode as FrameworkElement).Focus();

               
               
            }
        }

        #endregion
    }
   
}
