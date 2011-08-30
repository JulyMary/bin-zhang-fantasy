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
using System.ComponentModel.Design;
using System.Collections;
using System.Collections.Specialized;

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

            Model.PropertyNode node = (Model.PropertyNode)this.PropertyListBox.ItemContainerGenerator.ItemFromContainer(container);
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

            if (this.PropertyListBox.SelectedItem != node)
            {
                this.PropertyListBox.SelectedItem = node;
            }

        }

        private WeakEventListener _propertyNodesChangedListener;

        private void Node_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            m.ClassNode model = this.DataContext as m.ClassNode;
            this.CommandBindings.Clear();

            ISelectionService selectionService = model.Site.GetService<ISelectionService>();
            selectionService.SelectionChanged += new EventHandler(SelectionService_SelectionChanged);

            if (model != null)
            {
                IMenuService svc = model.Site.GetService<IMenuService>();
                if (svc != null)
                {
                    this.ConentStackPanel.ContextMenu = svc.CreateContextMenu("fantasy/studio/businessengine/classdiagrampanel/classnode/contextmenu", this, model.Site);
                }

                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/classnode/commandbindings").BuildChildItems(this, model.Site))
                {
                    this.ConentStackPanel.CommandBindings.Add(cb);
                }

                _propertyNodesChangedListener = new WeakEventListener(this.AutoSelectNewProperty);
                CollectionChangedEventManager.AddListener(model.Properties, _propertyNodesChangedListener);

            }
            else
            {
                this.ContextMenu = null;
            }


        }

        private bool AutoSelectNewProperty(Type managerType, object sender, EventArgs e)
        {
            if (this.IsKeyboardFocusWithin)
            {
                NotifyCollectionChangedEventArgs args = (NotifyCollectionChangedEventArgs)e;
                m.ClassNode model = this.DataContext as m.ClassNode;
                model.ShowMember = model.ShowProperties = true;
                if (args.NewItems != null)
                {

                    this.PropertyListBox.SelectedItems.Clear();
                    foreach(m.PropertyNode p in args.NewItems)
                    {
                        this.PropertyListBox.SelectedItems.Add(p);
                    }
                   
                }
                if (!this.PropertyListBox.IsKeyboardFocusWithin)
                {
                    this.PropertyListBox.Focus();
                }
            }

            return true;
        }

        void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            if (!_selecting)
            {
                this._selecting = true;
                try
                {
                    m.ClassNode model = this.DataContext as m.ClassNode;
                    ISelectionService svc = model.Site.GetRequiredService<ISelectionService>();
                    IEnumerable<object> selected = svc.GetSelectedComponents().Cast<object>();

                    var query = from n in this.PropertyListBox.SelectedItems.Cast<m.PropertyNode>()
                                where !selected.Any(o => o == n.Entity)
                                select n;

                    foreach(object o in query.ToArray())
                    {
                        this.PropertyListBox.SelectedItems.Remove(o);
                    };


                }
                finally
                {
                    this._selecting = false;
                }
            }
        }


        private bool _selecting = false;

        private void PropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_selecting)
            {
                this._selecting = true;
                try
                {
                    m.ClassNode model = this.DataContext as m.ClassNode;
                    var q1 = this.PropertyListBox.SelectedItems.Cast<m.PropertyNode>().Select(n => n.Entity);

                    ISelectionService svc = model.Site.GetRequiredService<ISelectionService>();

                    svc.SetSelectedComponents(q1.ToArray(), SelectionTypes.Replace);


                }
                finally
                {
                    this._selecting = false;
                }
            }
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.ConentStackPanel.IsKeyboardFocusWithin)
            {
                this.Focus();
            }
        }

    }
}
