using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.CodeDom.Compiler;
using Fantasy.Studio.BusinessEngine.Properties;
using System.Diagnostics;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessRoleCodeGenerator : ObjectWithSite, Fantasy.Studio.BusinessEngine.CodeGenerating.IBusinessRoleCodeGenerator
    {

        public void Rename(BusinessRoleData role)
        {
            if (role.Package != null && role.EntityState != EntityState.Deleted)
            {
                if (String.IsNullOrEmpty(role.Script))
                {
                    IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;
                    string output = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.NewBusinessRoleT4Path), role, out errors);

                    role.Script = output;

                    foreach (CompilerError error in errors)
                    {
                        Debug.WriteLine(error.ToString());
                    }
                }
                else
                {
                    IRefactoryService refactory = this.Site.GetRequiredService<IRefactoryService>();
                    string content = refactory.RenameClass(role.Script, role.CodeName);
                    content = refactory.RenameNamespace(content, role.Package.FullCodeName);
                    role.Script = content;
                }
            }
        }
    }
}
