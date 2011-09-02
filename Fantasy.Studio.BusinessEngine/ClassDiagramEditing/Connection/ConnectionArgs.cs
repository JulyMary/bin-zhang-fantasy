using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class ConnectionArgs
    {
        public MouseEventArgs MouseEventArgs { get; set; }

        public ConnectionAdorner Owner { get; set; }

        public Cursor Cursor { get; set; }

        public bool Handled { get; set; }
    }
}
