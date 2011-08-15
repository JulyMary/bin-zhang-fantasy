using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows;

namespace Fantasy.Studio
{
    public class DoDragDropHandler : ObjectWithSite, IEventHandler<DoDragDropEventArgs>
    {

        #region IEventHandler<DoDragDropEventArgs> Members

        public void HandleEvent(object sender, DoDragDropEventArgs e)
        {
            e.AllowedEffects = this.AllowedEffects;
            e.Data = this.Data;
        }

        #endregion

        public Object Data { get; set; }
        public DragDropEffects AllowedEffects { get; set; }
    }
}
