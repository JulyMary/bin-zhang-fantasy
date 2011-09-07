using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class AssemblyReferenceProvider : ObjectWithSite, IChildItemsProvider
    {
        public System.Collections.IEnumerable GetChildren(object parent)
        {
            BusinessAssemblyReferenceGroup group = (BusinessAssemblyReferenceGroup)parent;
            return group.References.ToSorted("Name");
        }
    }
}
