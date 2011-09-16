using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class ExitConnectionHandler : IConnectionHandler
    {

        #region IClassConnectionAdornerHandler Members

        public void MouseMove(ConnectionArgs args)
        {
            
        }

        public void MouseDown(ConnectionArgs args)
        {
            args.Owner.Exit();
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
