using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class UserScriptProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();
            BusinessUserData user = am.GetAdapter<BusinessUserData>(parent);

            return new BusinessUserScript[] { new BusinessUserScript(user) };
        }

        #endregion
    }
}
