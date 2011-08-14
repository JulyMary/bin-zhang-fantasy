using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio.Controls
{
    public class ToolBoxItemModel
    {
        public object Icon { get; set; }

        public string Text { get; set; }

        public string Category { get; set; }
        
        public ICommand DoubleClick { get; set; }
        
        public ICommand Selected { get; set; }
       
        public ICommand Unselected { get; set; }

        public ICommand DoDragDrop { get; set; }

        public object CommandParameter { get; set; }
    }
}
