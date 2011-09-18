using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public interface IPackageSubfolder 
    {
        BusinessPackage Package { get; }
    }
}
