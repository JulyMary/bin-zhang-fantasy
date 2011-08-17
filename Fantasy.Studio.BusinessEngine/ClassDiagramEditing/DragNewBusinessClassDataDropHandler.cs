using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Syncfusion.Windows.Diagram;
using System.Windows.Controls;

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
                DiagramModel model = this.Site.GetRequiredService<DiagramModel>();
                Point p = e.GetPosition(view.Page);

                Node newNode = new Node(Guid.NewGuid(), "Hello")
                {
                    LabelVisibility = Visibility.Collapsed,
                    //IsLabelEditable=true,
                    Content = new Shapes.BusinessClass() { IsHitTestVisible = true },
                    Shape = Syncfusion.Windows.Diagram.Shapes.Rectangle,
                   
                    Height = double.NaN,
                    OffsetX = p.X,
                    OffsetY = p.Y,
                    AllowRotate = false,
                    GripperVisibility = Visibility.Visible,
                };

                newNode.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                newNode.Width = newNode.DesiredSize.Width;

                model.Nodes.Add(newNode);

                newNode.SetResourceReference(FrameworkElement.StyleProperty, new ComponentResourceKey(typeof(Shapes.BusinessClass), "BusinessClassDSK"));

                view.SelectionList.Clear();
                view.SelectionList.Add(newNode);
                (newNode as FrameworkElement).Focus();        
               
            }
        }

        #endregion
    }
}
