using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class DragBusinessUserOverHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {

        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {
            BusinessUserData data = e.Data.GetDataByType<BusinessUserData>();
            if (data != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
        }

        #endregion
    }
   
}
