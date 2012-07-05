using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Fantasy.Jobs.Tasks
{
    public abstract class XmlTaskBase : ObjectWithSite, ITask
    {

        public XmlTaskBase()
        {
            this.Encoding = "utf-8";
            this.ConformanceLevel = ConformanceLevel.Document;
            this.Indent = false;
            this.IndentChars = "  ";
            this.NamespaceHandling = NamespaceHandling.Default;
            this.NewLineChars = "\r\n";
            this.NewLineHandling = NewLineHandling.Replace;
            this.NewLineOnAttributes = false;
            this.OmitXmlDeclaration = false;
        }

        private XmlWriterSettings _xmlWriterSettings;
        protected XmlWriterSettings XmlWriterSettings
        {
            get
            {
                if (_xmlWriterSettings == null)
                {
                    _xmlWriterSettings = new XmlWriterSettings()
                {
                    CloseOutput = true,
                    Encoding = System.Text.Encoding.GetEncoding(this.Encoding),
                    Indent = this.Indent,
                    IndentChars = this.IndentChars,
                    CheckCharacters = this.CheckCharacters,
                    ConformanceLevel = this.ConformanceLevel,
                    NewLineChars = this.NewLineChars,
                    NewLineHandling = this.NewLineHandling,
                    NewLineOnAttributes = this.NewLineOnAttributes,
                    OmitXmlDeclaration = this.OmitXmlDeclaration,
                    NamespaceHandling = this.NamespaceHandling,
                }; 
                }

                return _xmlWriterSettings;
            }
        }


        public abstract bool Execute();
        

        [TaskMember("encoding", Description="The type of text encoding to use.")]
        public string Encoding { get; set; }

        [TaskMember("checkCharacters", Description="Sets a valude indicating whether to do character checking.")]
        public bool CheckCharacters { get; set; }

        [TaskMember("conformanceLevel", Description="The level of conformance which the XmlWriter compiles with.")]
        public ConformanceLevel ConformanceLevel { get; set; }

        [TaskMember("indent", Description="A value indicating whether to indent elements.")]
        public bool Indent { get; set; }

        [TaskMember("indentChars", Description=" The character string to use when indenting. This setting is used when the 'indent' property is set to true.")]
        public string IndentChars { get; set; }

        [TaskMember("namespaceHandling", Description="A value indicates whether the XmlWriter should remove duplicate namespace declarations when writing XML content. The default behavior is for the writer to output all namespace declarations that are present in the writer's namespace resolver.")]
        public NamespaceHandling NamespaceHandling { get; set; }

        [TaskMember("newLineChars", Description="The character string to use for line break.")]
        public string NewLineChars { get; set; }

        [TaskMember("newLineHandling", Description="A value indicating whether to normalize line breaks in the output.")]
        public NewLineHandling NewLineHandling { get; set; }

        [TaskMember("newLineOnAttributes", Description="A value indicating whether ot write attributes on a new line.")]
        public bool NewLineOnAttributes { get; set; }

        [TaskMember("omitXmlDeclaration", Description="A value indicating whether ot write an XML declaration.")]
        public bool OmitXmlDeclaration { get; set; }
        
    }
}
