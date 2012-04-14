using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class NoCursorHandler : IConnectionHandler
    {
       

        public void MouseMove(ConnectionArgs args)
        {
            args.Cursor = Cursors.No;
        }

       

        public void MouseEnter(ConnectionArgs args)
        {
            
        }

        public void MouseLeave(ConnectionArgs args)
        {
           
        }

        public void MouseLeftButtonDown(ConnectionArgs args)
        {
         
        }

        public void MouseLeftButtonUp(ConnectionArgs args)
        {
            
        }

        public void MouseRightButtonDown(ConnectionArgs args)
        {
            
        }

        public void MouseRightButtonUp(ConnectionArgs args)
        {
            
        }

      
    }
}
