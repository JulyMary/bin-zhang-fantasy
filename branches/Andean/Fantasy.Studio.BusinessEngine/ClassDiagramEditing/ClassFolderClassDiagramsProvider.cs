using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.TreeViewModel;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.ClassEditing;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassFolderClassDiagramsProvider : ObjectWithSite, IChildrenProvider
    {
        public IEnumerable<object> GetChildren(object parent)
        {
            return ((ClassFolder)parent).Package.ClassDiagrams;
        }
    }
}
