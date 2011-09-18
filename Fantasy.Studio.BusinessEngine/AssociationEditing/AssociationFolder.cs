using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Studio.BusinessEngine.PackageEditing;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class AssociationFolder : IPackageSubfolder
    {
        public Fantasy.BusinessEngine.BusinessPackage Package {get;private set;}

        public AssociationFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
           
            this.Package = package;          
        }

        #region IDocumentTreeViewItem Members

        public string Name
        {
            get { return Resources.AssociationFolderName; }
        }

       

        #endregion
    }
}
