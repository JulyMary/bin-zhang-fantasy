using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using System.Collections.ObjectModel;
using Fantasy.BusinessEngine.Collections;
using Fantasy.BusinessEngine;
using Fantasy.Studio.BusinessEngine.PackageEditing;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class UserRoleFolder : IPackageSubfolder
    {
        public Fantasy.BusinessEngine.BusinessPackage Package {get;private set;}

        public UserRoleFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
            this.Package = package;          
        }

        public string Name
        {
            get { return Resources.UserRoleFolderName; }
        }

        
    }
}
