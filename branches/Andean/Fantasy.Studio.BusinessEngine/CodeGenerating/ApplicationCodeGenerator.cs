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
    public class ApplicationCodeGenerator : ObjectWithSite, Fantasy.Studio.BusinessEngine.CodeGenerating.IApplicationCodeGenerator
    {

        public void Rename(BusinessApplicationData app)
        {
            if (app.Package != null && app.EntityState != EntityState.Deleted && !string.IsNullOrEmpty(app.CodeName))
            {
                if (String.IsNullOrEmpty(app.Script))
                {
                    IT4Service t4 = this.Site.GetRequiredService<IT4Service>();
                    CompilerErrorCollection errors;
                    string output = t4.ProcessTemplateFile(Settings.ExtractToFullPath(Settings.Default.NewBusinessApplicationT4Path), app, out errors);

                    app.Script = output;

                    foreach (CompilerError error in errors)
                    {
                        Debug.WriteLine(error.ToString());
                    }
                }
                else
                {
                    IRefactoryService refactory = this.Site.GetRequiredService<IRefactoryService>();
                    string content = refactory.RenameClass(app.Script, app.CodeName);
                    content = refactory.RenameNamespace(content, app.Package.FullCodeName);
                    app.Script = content;
                }
            }
        }
    }
}
