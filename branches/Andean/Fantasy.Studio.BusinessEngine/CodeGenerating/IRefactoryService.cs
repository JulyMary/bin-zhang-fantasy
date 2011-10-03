using System;
namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public interface IRefactoryService
    {
        string RenameClass(string content, string newClassName);
        string RenameNamespace(string content, string newNamespace);
    }
}
