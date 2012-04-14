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

            if (!this._startSelection)
            {
                Node shape = HitTest(args);
                if (shape != null && CanbeFirstNode(args.Owner, shape))
                {
                    args.Cursor = Cursors.Cross;
                    args.Handled = true;
                }
            }
            else
            {

                Point p2 = args.MouseEventArgs.GetPosition(args.Owner.View);
                if (Math.Abs((p2 - this._selectionPoint).Length) >= 3)
                {
                    args.Owner.FirstNode = this._firstNode;
                    args.Owner.Handlers.Clear();
                    args.Owner.Handlers.AddRange(new IConnectionHandler[] { new NoCursorHandler(), new SelectSecondNodeHandler() {IsDragMode=true}, new ExitConnectionHandler() });
                    args.Owner.StartPoint = this._selectionPoint;
                    args.Handled = true;
                }
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


        private bool _startSelection = false;
        private Point _selectionPoint;
        private Node _firstNode;



        public void MouseLeftButtonDown(ConnectionArgs args)
        { 
            Node shape = HitTest(args);
            if (shape != null && CanbeFirstNode(args.Owner, shape))
            {
                this._startSelection = true;
                this._selectionPoint = args.MouseEventArgs.GetPosition(args.Owner.View);
                _firstNode = shape;
                args.Owner.View.CaptureMouse();
                args.Handled = true;
            }
            
        }

        public void MouseEnter(ConnectionArgs args)
        {
           
        }

        public void MouseLeave(ConnectionArgs args)
        {
            
        }

        public void MouseLeftButtonUp(ConnectionArgs args)
        {
            if (this._startSelection)
            {
                args.Owner.FirstNode = this._firstNode;
                args.Owner.Handlers.Clear();
                args.Owner.Handlers.AddRange(new IConnectionHandler[] { new NoCursorHandler(), new SelectSecondNodeHandler() {IsDragMode = false}, new SelectIntermediatePointHandler(), new ExitConnectionHandler() });
                args.Owner.StartPoint = this._selectionPoint;
                args.Handled = true;
            }
        }

       


        public void MouseRightButtonDown(ConnectionArgs args)
        {
            
        }

        public void MouseRightButtonUp(ConnectionArgs args)
        {
            
        }

        #endregion
    }
}
