using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.BusinessEngine.PackageEditing;

namespace Fantasy.Studio.BusinessEngine.Scripting
{
    public class ScriptFolder : ObjectWithSite, IPackageSubfolder
    {
        #region IPackageSubfolder Members

        public Fantasy.BusinessEngine.BusinessPackage Package
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }
}
