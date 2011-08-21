using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    [XSerializable("class", NamespaceUri= Consts.ClassDiagramNamespace)]
    public class ClassNodeData
    {

        [XAttribute("left")]
        public int Left { get; set; }

        [XAttribute("top")]
        public int Top { get; set; }

        [XAttribute("width")]
        public int Width { get; set; }

        [XAttribute("class")]
        public Guid ClassId { get; set; }
        
        [XAttribute("showMember")]
        public bool ShowMembers { get; set; }

        [XAttribute("showProperties")]
        public bool ShowProperties { get; set; }

        [XAttribute("showRelations")]
        public bool ShowRelations { get; set; }


    }
}
