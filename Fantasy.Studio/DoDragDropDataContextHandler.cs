using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio
{
    public class DoDragDropDataContextHandler : ObjectWithSite, IEventHandler<DoDragDropEventArgs>
    {
        public DoDragDropDataContextHandler()
        {

        }

        #region IEventHandler<DoDragDropEventArgs> Members

        public void HandleEvent(object sender, DoDragDropEventArgs e)
        {
            if (e.DataContext != null)
            {
                e.Data = e.DataContext;
                e.AllowedEffects = this.AllowedEffects;
               
            }

        }

        #endregion

      
        public DragDropEffects AllowedEffects { get; set; }
    }
}
