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
    public class DeleteUserCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessUser user = (BusinessUser)args;


            user.Package.Users.Remove(user);
            user.Package = null;

            foreach (BusinessRole role in user.Roles.ToArray())
            {
                role.Users.Remove(user);

            }
            user.Roles.Clear();

            es.Delete(user);

            this.Site.GetRequiredService<IEditingService>().CloseView(user, true);

            return null;
        }

        #endregion
    }
}
