using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    [XmlRoot("file")]
    public class FileSyntaxHighlight
    {
        [XmlAttribute("ext")]
        public string Extension { get; set; }

        [XmlAttribute("lang")]
        public string Language { get; set; }
    }
}
