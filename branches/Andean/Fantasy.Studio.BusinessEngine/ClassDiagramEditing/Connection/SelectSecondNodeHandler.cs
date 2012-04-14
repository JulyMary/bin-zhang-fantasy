using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;
using Fantasy.Windows;
using Syncfusion.Windows.Diagram;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class SelectSecondNodeHandler : IConnectionHandler
    {
        #region IClassConnectionAdornerHandler Members

        public void MouseMove(ConnectionArgs args)
        {

            args.Owner.CurrentPoint = args.MouseEventArgs.GetPosition(args.Owner.View);

            Node shape = HitTest(args);
            if (shape != null && CanbeSecondNode(args.Owner,shape))
            {
                args.Cursor = Cursors.Cross;
                args.Handled = true;
            }

        }

        private bool CanbeSecondNode(ConnectionAdorner owner, Node shape)
        {
            ValidatNodeArgs e = new ValidatNodeArgs() { Node = shape, IsValid = false };
            owner.OnValidateSecondNode(e);
            return e.IsValid;

        }
       

        private Node HitTest(ConnectionArgs args)
        {
            Node rs = null;
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
            
        }

        public void MouseEnter(ConnectionArgs args)
        {

        }

        public void MouseLeave(ConnectionArgs args)
        {

        }


        private void SetSecondNode(ConnectionArgs args)
        {
            Node shape = HitTest(args);
            if (shape != null && CanbeSecondNode(args.Owner, shape))
            {
                args.Owner.SecondNode = shape;

                args.Owner.OnCreatConnection(EventArgs.Empty);

                args.Owner.Exit();
                args.Handled = true;
            }
        }

        public void MouseLeftButtonUp(ConnectionArgs args)
        {

            if (IsDragMode)
            {
                SetSecondNode(args);
            }
          
        }

        public void MouseLeftButtonDown(ConnectionArgs args)
        {
            if (!IsDragMode)
            {
                SetSecondNode(args);
            }
        }

        public void MouseRightButtonDown(ConnectionArgs args)
        {
            
        }

        public void MouseRightButtonUp(ConnectionArgs args)
        {
            
        }

        #endregion


        public bool IsDragMode { get; set; }
    }
}
