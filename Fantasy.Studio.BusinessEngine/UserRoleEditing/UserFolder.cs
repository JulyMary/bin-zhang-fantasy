using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Studio.BusinessEngine.PackageEditing;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class UserFolder : IPackageSubfolder
    {
      
            public Fantasy.BusinessEngine.BusinessPackage Package { get; private set; }

            public UserFolder(Fantasy.BusinessEngine.BusinessPackage package)
            {
                this.Package = package;
            }



            public string Name
            {
                get { return Resources.UserFolderName; }
            }


        
    }
}
