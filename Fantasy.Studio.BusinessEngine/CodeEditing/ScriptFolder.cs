using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.PackageEditing;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class ScriptFolder : ObjectWithSite, IPackageSubfolder
    {



       public Fantasy.BusinessEngine.BusinessPackage Package {get;private set;}

       public ScriptFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
           
            this.Package = package;          
        }

        #region IDocumentTreeViewItem Members

        public string Name
        {
            get { return Resources.ScriptFolderName; }
        }

       

        #endregion
    }
}
