using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.Adaption;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ClassAutoScriptProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();
            BusinessClass @class = (BusinessClass)am.GetAdapter<IBusinessEntity>(parent);
            return (@class.ScriptOptions & ScriptOptions.NoScript) != ScriptOptions.NoScript ?

                new object[] { new BusinessClassAutoScript(@class) } : new object[0];

        }

        #endregion
    }
}
