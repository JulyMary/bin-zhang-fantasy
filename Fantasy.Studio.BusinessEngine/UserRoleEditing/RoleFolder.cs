using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Studio.BusinessEngine.PackageEditing;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class RoleFolder : IPackageSubfolder
    {
        public Fantasy.BusinessEngine.BusinessPackage Package { get; private set; }

        public RoleFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
            this.Package = package;
        }

        public string Name
        {
            get { return Resources.RoleFolderName; }
        }
    }
}
