using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class NoCursorHandler : IConnectionHandler
    {
        #region IConnectionHandler Members

        public void MouseMove(ConnectionArgs args)
        {
            args.Cursor = Cursors.No;
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

        public void MouseUp(ConnectionArgs args)
        {
           
        }

        #endregion
    }
}
