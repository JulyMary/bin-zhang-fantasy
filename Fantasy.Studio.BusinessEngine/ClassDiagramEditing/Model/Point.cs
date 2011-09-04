using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model
{
    [XSerializable("point", NamespaceUri = Consts.ClassDiagramNamespace)]
    public class Point
    {
        [XAttribute("x")]
        public double X { get; set; }

        [XAttribute("y")]
        public double Y { get; set; }
    }
}
