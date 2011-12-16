using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class PackageScriptFolderProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessPackage package = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessPackage>(parent);

            if (package.Id != BusinessPackage.RootPackageId)
            {

                return new object[] { new ScriptFolder(package) };
            }
            else
            {
                return new object[] { };
            }
        }

        #endregion
    }
}
