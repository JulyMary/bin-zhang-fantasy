using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio
{
    public class DoDragDropEventArgs : EventArgs 
    {
        public Object Data {get;set;}
        public DragDropEffects AllowedEffects {get;set;}
    }
}
