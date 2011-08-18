using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    public class NodeExpander : Expander
    {
        static NodeExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeExpander), new FrameworkPropertyMetadata(typeof(NodeExpander)));

        }

        public NodeExpander()
        {

        }
    }
}
