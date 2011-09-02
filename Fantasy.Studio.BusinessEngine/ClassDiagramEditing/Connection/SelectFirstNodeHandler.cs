using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.Windows;
using System.Windows.Input;
using Syncfusion.Windows.Diagram;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class SelectFirstNodeHandler : IConnectionHandler
    {
        #region IClassConnectionAdornerHandler Members

        public void MouseMove(ConnectionArgs args)
        {
            Node shape = HitTest(args);
            if (shape != null && CanbeFirstNode(args.Owner, shape))
            {
                args.Cursor = Cursors.Cross;
                args.Handled = true;
            }

        }

        private bool CanbeFirstNode(ConnectionAdorner owner, Node shape)
        {
            ValidatNodeArgs e = new ValidatNodeArgs() { Node = shape, IsValid = false };
            owner.OnValidateFirstNode(e);
            return e.IsValid;
            
        }


      

        private Node HitTest(ConnectionArgs args)
        {
            Node rs = null;;
            Point p = args.MouseEventArgs.GetPosition(args.Owner.View);
            DependencyObject obj = args.Owner.View.InputHitTest(p) as DependencyObject;

            if (obj != null)
            {
                rs = obj.GetAncestor<Node>();
            }

            return rs;
        }

        public void MouseDown(ConnectionArgs args)
        {
            if (args.MouseEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                args.Owner.FirstPoint = args.MouseEventArgs.GetPosition(args.Owner.View);
                Node shape = HitTest(args);
                if (shape != null && CanbeFirstNode(args.Owner, shape))
                {
                    args.Owner.FirstNode = shape;

                    args.Owner.View.CaptureMouse();

                    args.Owner.Handlers.Clear();
                    args.Owner.Handlers.AddRange(new IConnectionHandler[] { new NoCursorHandler(), new SelectSecondNodeHandler(), new ExitConnectionHandler() });
                    
                    args.Handled = true;
                }
            }
        }

        public void MouseEnter(ConnectionArgs args)
        {
           
        }

        public void MouseLeave(ConnectionArgs args)
        {
            
        }

        public void MouseUp(ConnectionArgs args)
        {
           
        }

        #endregion
    }
}
