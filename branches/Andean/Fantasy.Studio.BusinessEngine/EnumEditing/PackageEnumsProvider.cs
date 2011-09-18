﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class PackageEnumsProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            return am.GetAdapter<BusinessPackage>(parent).Enums.ToSorted("Name");
        }

        #endregion
    }
}
