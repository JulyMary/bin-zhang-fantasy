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

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class ClassFolder
    {
        public Fantasy.BusinessEngine.BusinessPackage Package {get;private set;}

        public ClassFolder(Fantasy.BusinessEngine.BusinessPackage package)
        {
           
            this.Package = package;          
        }

        #region IDocumentTreeViewItem Members

        public string Name
        {
            get { return Resources.PakcageClassFolderName; }
        }

       

        #endregion
    }
}
