using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class DragBusinessRoleOverHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {

        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            BusinessRoleData data = e.Data.GetDataByType<BusinessRoleData>();
            if (data != null && ! data.IsComputed)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        #endregion
    }
   
}
