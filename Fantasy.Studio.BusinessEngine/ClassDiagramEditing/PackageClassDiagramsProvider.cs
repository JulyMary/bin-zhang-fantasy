using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.ClassEditing;
using System.Collections;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class PackageClassDiagramsProvider : ObjectWithSite, IChildItemsProvider
    {
        public IEnumerable GetChildren(object parent)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();

            return am.GetAdapter<BusinessPackage>(parent).ClassDiagrams.ToSorted("Name");
        }
    }
}
