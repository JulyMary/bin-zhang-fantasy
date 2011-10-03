using System;
using Fantasy.BusinessEngine;
namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface IBusinessClassCodeGenerator
    {
        void RegisterClass(BusinessClass @class);
        void UpdateAutoScript(BusinessClass @class);
        void Rename(BusinessClass @class);

    }
}
