using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;

namespace Fantasy.Studio.BusinessEngine.Scripting
{
    public class PackageScriptsProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
