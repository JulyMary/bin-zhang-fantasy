﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Controls;
using System.Collections;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    class PackageUserRoleFolderProvider :ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            BusinessPackage package = am.GetAdapter<BusinessPackage>(parent);
            return package.Id != BusinessPackage.RootPackageId ? new object[] { new UserRoleFolder(package)} : new object[0];
        }

        #endregion
    }
}