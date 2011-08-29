using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Fantasy.BusinessEngine;
using Fantasy.Windows;
using m = Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model;
using Fantasy.Studio.Services;
using Fantasy.AddIns;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    /// <summary>
    /// Interaction logic for ClassNode.xaml
    /// </summary>
    public partial class ClassNode : Syncfusion.Windows.Diagram.Node
    {
        static ClassNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ClassNode), new FrameworkPropertyMetadata(typeof(ClassNode)));

        }


        public ClassNode()
        {
            InitializeComponent();

        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject container = this.PropertyListBox.ContainerFromElement((DependencyObject)e.Source);

            Model.PropertyNode node = ( Model.PropertyNode)this.PropertyListBox.ItemContainerGenerator.ItemFromContainer(container);
            BusinessProperty from = node.Entity;

            BusinessClass cls = from.Class;

            int index = cls.Properties.IndexOf(from);

            BusinessProperty to = cls.Properties[index - 1];

            long temp = from.Order;
            from.Order = to.Order;
            to.Order = temp;

            cls.Properties.Swap(index, index - 1);
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject container = this.PropertyListBox.ContainerFromElement((DependencyObject)e.Source);

            Model.PropertyNode node = (Model.PropertyNode)this.PropertyListBox.ItemContainerGenerator.ItemFromContainer(container);

            BusinessProperty from = node.Entity;

            BusinessClass cls = from.Class;

            int index = cls.Properties.IndexOf(from);

            BusinessProperty to = cls.Properties[index + 1];

            long temp = from.Order;
            from.Order = to.Order;
            to.Order = temp;

            cls.Properties.Swap(index, index + 1);
        }

        private void Node_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            m.ClassNode model = this.DataContext as m.ClassNode;
             this.CommandBindings.Clear();
            if(model != null)
            {
                IMenuService svc = model.Site.GetService<IMenuService>();
                if (svc != null)
                {
                    this.ContextMenu = svc.CreateContextMenu("fantasy/studio/businessengine/classdiagrampanel/classnode/contextmenu", this, model.Site);
                }
                
                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/classnode/commandbindings").BuildChildItems(this, model.Site))
                {
                    this.CommandBindings.Add(cb);
                }
            }
            else
            {
                this.ContextMenu = null;
            }
 
          
        }

    }
}
