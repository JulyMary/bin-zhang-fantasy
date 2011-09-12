using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class ClassDiagramNode : Syncfusion.Windows.Diagram.Node
    {
        static ClassDiagramNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClassDiagramNode), new FrameworkPropertyMetadata(typeof(ClassDiagramNode)));

        }

        public ClassDiagramNode(Guid id) : base(id)
        {

        }

        public ClassDiagramNode()
        {

        }

       
    }
}
