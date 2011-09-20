using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.PackageEditing;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    public class ApplicationFolder : IPackageSubfolder
    {
        public Fantasy.BusinessEngine.BusinessPackage Package { get; private set; }

        public ApplicationFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
            this.Package = package;
        }

        public string Name
        {
            get { return Resources.ApplicationFolderName; }
        }
    }
}
