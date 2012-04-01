using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptDependentUpProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessScript script = this.Site.GetRequiredService<IAdapterManager>().GetAdapter<BusinessScript>(parent);
            return script.Package.Scripts.ToFiltered(s => String.Equals(s["DependentUpon"], script.Name, StringComparison.OrdinalIgnoreCase)).ToSorted("Name");

        }

        #endregion
    }
}
