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

                DiagramModel model = this.Site.GetRequiredService<DiagramModel>();
                Node EssentialWPF = new Node(Guid.NewGuid(), "EssentialWPF");

                EssentialWPF.Shape = Syncfusion.Windows.Diagram.Shapes.FlowChart_Card;

                EssentialWPF.Width = 100;

                EssentialWPF.Height = 50;

                EssentialWPF.OffsetX = 300;

                EssentialWPF.OffsetY = 300;
                EssentialWPF.Content = "Hello";//new Button() { Content = "Hello", IsHitTestVisible = false};
               

                //Button b = new Button();

                //b.Content = "Click ME!";

                //EssentialWPF.Content = b;

                //b.IsHitTestVisible = true;


                model.Nodes.Add(EssentialWPF);

                //Node newNode = new Node(Guid.NewGuid())
                //{ 
                //    LabelVisibility = Visibility.Collapsed,
                //    Content = new Shapes.BusinessClass(),
                //    Shape= Syncfusion.Windows.Diagram.Shapes.Rectangle
                //};

                //DiagramModel model = this.Site.GetRequiredService<DiagramModel>();
                DiagramControl dc = this.Site.GetRequiredService<DiagramControl>();

                //model.Nodes.Add(newNode);

                //DiagramView view = this.Site.GetRequiredService<DiagramView>();
                //if (!view.Page.Children.Contains(newNode))
                //{

                //}
                //Point p = e.GetPosition(view.Page);

                ////newNode.Position = p;
                //newNode.Width = 50;
                //newNode.Height = 50;

                //view.SelectionList.Clear();
                //view.SelectionList.Add(newNode);
                //(newNode as FrameworkElement).Focus();        
               
            }
        }

        #endregion
    }
}
