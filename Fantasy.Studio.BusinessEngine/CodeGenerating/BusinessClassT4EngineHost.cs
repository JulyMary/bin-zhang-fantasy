using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TextTemplating;
using System.Reflection;
using Fantasy.IO;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class BusinessClassT4EngineHost : ObjectWithSite, ITextTemplatingEngineHost
    {

        public BusinessClassT4EngineHost()
        {
            this.OutputEncoding = Encoding.Unicode;
        }
        #region ITextTemplatingEngineHost Members

        public object GetHostOption(string optionName)
        {
            object rs = null;
            switch (optionName)
            {
                case Engine.CacheAssembliesOptionString :
                    rs = true;
                    break;
            }

            return rs;
        }

        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = location = string.Empty;

            return false;
        }

        public void LogErrors(System.CodeDom.Compiler.CompilerErrorCollection errors)
        {
            
        }

        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CurrentDomain;
        }

        public string ResolveAssemblyReference(string assemblyReference)
        {
            return string.Empty;
        }

        public Type ResolveDirectiveProcessor(string processorName)
        {
            throw new ApplicationException("Directive Processor not found");
        }

        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            return string.Empty;
        }

        public string ResolvePath(string path)
        {
            string baseDir = LongPath.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            return LongPath.Combine(baseDir, path);
        }


        private string _fileExtension = "cs";

        public void SetFileExtension(string extension)
        {
            this._fileExtension = extension;
        }



        public Encoding OutputEncoding {get;set;} 

        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            this.OutputEncoding = encoding;
        }

        public IList<string> StandardAssemblyReferences
        {
            get 
            {
                return new string[]
                {
                    typeof(System.Uri).AssemblyQualifiedName,
                    typeof(System.Linq.Enumerable).AssemblyQualifiedName,
                    typeof(System.Xml.XmlDocument).AssemblyQualifiedName,
                    typeof(System.Xml.Linq.XElement).AssemblyQualifiedName
                };
            }
        }


     
        public IList<string> StandardImports
        {
            get 
            {
                return new string[]
                {
                    "System",
                    "System.Collections.Generic",
                    "System.Linq",
                    "System.Text"
                };
            }
        }

        public string TemplateFile { get; set; }
       

        #endregion
    }
}
