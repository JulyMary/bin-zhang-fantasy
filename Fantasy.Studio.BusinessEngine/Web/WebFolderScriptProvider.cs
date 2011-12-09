using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.Web
{
    public class WebFolderScriptProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessWebFolder folder = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessWebFolder>(parent);
            return folder.Scripts;
        }

        #endregion
    }
}
