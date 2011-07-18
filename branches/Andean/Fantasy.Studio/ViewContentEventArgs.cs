using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio
{
    public class ViewContentEventArgs : EventArgs 
    {
        public ViewContentEventArgs(IViewContent content)
        {
            this.Content = content;
        }
        public IViewContent Content { get; private set; }
    }
}
