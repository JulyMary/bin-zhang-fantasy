using System;
namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface IT4Service
    {
        string ProcessTemplateFile(string templateFile, object args, out System.CodeDom.Compiler.CompilerErrorCollection errors, IServiceProvider services = null);
        string ProcessTemplate(string template, object args, out System.CodeDom.Compiler.CompilerErrorCollection errors, IServiceProvider services = null);
    }
}
