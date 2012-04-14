using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class SelectIntermediatePointHandler : IConnectionHandler
    {
        #region IConnectionHandler Members

        public void MouseMove(ConnectionArgs args)
        {
            args.Cursor = Cursors.Arrow;
        }

        public void MouseLeftButtonDown(ConnectionArgs args)
        {

            Point point = args.MouseEventArgs.GetPosition(args.Owner.View);
            Rect rect = new Rect(0, 0, args.Owner.View.ActualWidth, args.Owner.View.ActualHeight);
            if (rect.Contains(point))
            {
                args.Owner.IntermediatePoints.Add(point);
                args.Handled = true;
            }

           
        }

        public void MouseLeftButtonUp(ConnectionArgs args)
        {
            Point point = args.MouseEventArgs.GetPosition(args.Owner.View);
            Rect rect = new Rect(0, 0, args.Owner.View.ActualWidth, args.Owner.View.ActualHeight);
            if (rect.Contains(point))
            {
                args.Handled = true;
            }
        }

        public void MouseRightButtonDown(ConnectionArgs args)
        {
            
        }

        public void MouseRightButtonUp(ConnectionArgs args)
        {
            
        }

        public void MouseEnter(ConnectionArgs args)
        {
            
        }

        public void MouseLeave(ConnectionArgs args)
        {
            
        }

        #endregion
    }
}
