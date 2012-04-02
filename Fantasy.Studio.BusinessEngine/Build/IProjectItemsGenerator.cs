using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Xml.Linq;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public interface IProjectItemsGenerator
    {
        void CreateItems(BusinessPackage package, XElement projectElement, XElement insertBefore, ProjectExportOptions options);
    }
}
