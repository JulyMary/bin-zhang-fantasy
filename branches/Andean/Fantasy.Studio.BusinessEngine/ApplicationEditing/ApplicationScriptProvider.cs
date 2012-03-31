using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationScriptProvider : ObjectWithSite, IChildItemsProvider
    {

        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();
            BusinessApplicationData app = am.GetAdapter<BusinessApplicationData>(parent);

            return new BusinessApplicationScript[] { new BusinessApplicationScript(app) };
        }

        #endregion
    }
}
