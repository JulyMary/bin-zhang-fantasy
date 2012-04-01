using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class PackageScriptsProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessPackage package = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessPackage>(parent);

            return package.Scripts.ToFiltered(s => String.IsNullOrEmpty(s["DependentUpon"]) || !package.Scripts.Any(s1 => String.Equals(s["DependentUpon"], s1.Name, StringComparison.OrdinalIgnoreCase))
               
            ).ToSorted("Name");
        }

        #endregion
    }
}
