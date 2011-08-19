using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio
{
    public class DoDragDropEventArgs : EventArgs 
    {
        public DoDragDropEventArgs()
        {

        }

        public DoDragDropEventArgs(object dataContext)
        {
            this.DataContext = dataContext; 
        }

        public object DataContext { get; private set; }

        public Object Data {get;set;}
        public DragDropEffects AllowedEffects {get;set;}

    }
}
