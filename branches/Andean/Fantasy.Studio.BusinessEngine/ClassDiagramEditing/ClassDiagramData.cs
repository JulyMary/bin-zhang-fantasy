using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{

    [XSerializable("diagram", NamespaceUri=Consts.ClassDiagramNamespace)]
    public class ClassDiagramData
    {
        public ClassDiagramData()
        {
            this.Classes = new List<ClassNodeData>(); 
        }

        [XArray(Name="classes"),
        XArrayItem(Name="class", Type=typeof(ClassNodeData))]
        public IList<ClassNodeData> Classes { get; private set; }

    }
}
