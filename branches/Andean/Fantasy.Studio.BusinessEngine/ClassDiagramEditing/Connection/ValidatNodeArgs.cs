using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syncfusion.Windows.Diagram;
using System.Windows.Input;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Connection
{
    class ValidatNodeArgs : EventArgs
    {
        public Node Node { get; set; }

        public bool IsValid { get; set; }

       
    }
}
