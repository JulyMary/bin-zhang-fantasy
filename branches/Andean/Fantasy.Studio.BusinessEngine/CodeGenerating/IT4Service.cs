using System;
namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface IT4Service
    {
        string ProcessTemplate(string templateFile, object args, out System.CodeDom.Compiler.CompilerErrorCollection errors, IServiceProvider services = null);
    }
}
