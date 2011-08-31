using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ClassChildClassesProvider : ObjectWithSite, IChildItemsProvider
    {
        #region IChildItemsProvider Members

        public IEnumerable GetChildren(object parent)
        {
            return ((BusinessClass)parent).ChildClasses.ToSorted("FullName");
        }

        #endregion
    }
}
