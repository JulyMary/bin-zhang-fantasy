using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Controls;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.ClassEditing;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ObjectModelFolderClassDiagramsProvider : ObjectWithSite, IChildItemsProvider
    {
        public IEnumerable GetChildren(object parent)
        {
            return ((ObjectModelFolder)parent).Package.ClassDiagrams.ToSorted("Name");
        }
    }
}
