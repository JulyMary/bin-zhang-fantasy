﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using System.Collections;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class ChildPackagesProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            return am.GetAdapter<BusinessPackage>(parent).ChildPackages.ToSorted("Name");
        }

        #endregion
    }
}
