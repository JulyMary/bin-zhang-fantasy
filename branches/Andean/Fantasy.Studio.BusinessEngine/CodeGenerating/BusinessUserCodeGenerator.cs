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
    public class BusinessUserCodeGenerator : ObjectWithSite, Fantasy.Studio.BusinessEngine.CodeGenerating.IBusinessUserCodeGenerator
    {

        public void Rename(BusinessUserData user)
        {
            if (user.Package != null && user.EntityState != EntityState.Deleted && !string.IsNullOrEmpty(user.CodeName))
            {
                if (String.IsNullOrEmpty(user.Script))
                {
                    IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;
                    string output = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.NewBusinessUserT4Path), user, out errors);

                    user.Script = output;

                    foreach (CompilerError error in errors)
                    {
                        Debug.WriteLine(error.ToString());
                    }
                }
                else
                {
                    IRefactoryService refactory = this.Site.GetRequiredService<IRefactoryService>();
                    string content = refactory.RenameClass(user.Script, user.CodeName);
                    content = refactory.RenameNamespace(content, user.Package.FullCodeName);
                    user.Script = content;
                }
            }
        }
    }
}
