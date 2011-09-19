using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class DeleteRoleCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessRole role = (BusinessRole)args;


            role.Package.Roles.Remove(role);
            role.Package = null;

            foreach (BusinessUser user in role.Users.ToArray())
            {
                user.Roles.Remove(role);

            }
            role.Users.Clear();

            es.Delete(role);

            this.Site.GetRequiredService<IEditingService>().CloseView(role, true);

            return null;
        }

        #endregion
    }
}
