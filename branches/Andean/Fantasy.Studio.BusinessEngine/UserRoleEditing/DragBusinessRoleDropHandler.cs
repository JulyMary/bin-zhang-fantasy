﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.Windows;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class DragBusinessRoleDropHandler : ObjectWithSite, IEventHandler<DragEventArgs>
    {
        #region IEventHandler<DragEventArgs> Members

        public void HandleEvent(object sender, DragEventArgs e)
        {

            BusinessRole role = e.Data.GetDataByType<BusinessRole>();
            if (role != null && ! role.IsComputed )
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;

                BusinessUser user = this.Site.GetRequiredService<BusinessUser>();

                if (user.Roles.IndexOf(role) < 0)
                {
                    user.Roles.Add(role);
                    role.Users.Add(user);
                   
                }
            }
        }

        #endregion
    }
}