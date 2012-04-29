using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public static class BusinessPackageExtension
    {
        public static string GetItemsFolder(this BusinessPackage package)
        {
            var query = from p in package.Flatten(p => p.ParentPackage) where p.Id != BusinessPackage.RootPackageId select p.CodeName;
            return string.Join(System.IO.Path.DirectorySeparatorChar.ToString(), query.Reverse());

        }
    }
}
