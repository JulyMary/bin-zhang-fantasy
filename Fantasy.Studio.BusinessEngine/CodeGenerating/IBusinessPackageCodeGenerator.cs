using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface IBusinessPackageCodeGenerator
    {
        void RegisterPackage(Fantasy.BusinessEngine.BusinessPackage package);
    }
}
