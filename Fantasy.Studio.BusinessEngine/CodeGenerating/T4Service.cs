using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.CodeDom.Compiler;
using Microsoft.VisualStudio.TextTemplating;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class T4Service : ServiceBase, Fantasy.Studio.BusinessEngine.CodeGenerating.IT4Service
    {

        public string ProcessTemplate(string templateFile, object args, out CompilerErrorCollection errors, IServiceProvider services = null)
        {
            
            T4EngineHost host = new T4EngineHost(services??this.Site)
            {

                TemplateFile = templateFile,
            };

            host.AddService(args);

            string templateContent = LongPathFile.ReadAllText(templateFile);

            Engine engine = new Engine();

            string output = engine.ProcessTemplate(templateContent, host);

            errors = host.CompilerErrors;

            return output;

            

            
        }
    }
}
