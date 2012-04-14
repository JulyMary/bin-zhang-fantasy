using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    interface IConnectionHandler
    {
        void MouseMove(ConnectionArgs args);

        void MouseLeftButtonDown(ConnectionArgs args);
        void MouseLeftButtonUp(ConnectionArgs args);

        void MouseRightButtonDown(ConnectionArgs args);
        void MouseRightButtonUp(ConnectionArgs args);
        void MouseEnter(ConnectionArgs args);

        void MouseLeave(ConnectionArgs args);

        
    }
}
