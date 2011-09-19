using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class DragBusinessUserDropHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {

            BusinessUser user = e.Data.GetDataByType<BusinessUser>();
            if (user != null)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;

                BusinessRole role = this.Site.GetRequiredService<BusinessRole>();

                if (role.Users.IndexOf(user) < 0)
                {
                    role.Users.Add(user);
                    user.Roles.Add(role);
                   
                }
            }
        }

        #endregion
    }
}
