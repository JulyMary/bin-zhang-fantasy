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
    public partial class ClassNode : ClassDiagramNode
    {
      

        public ClassNode(Guid id)
            : base(id)
        {
            InitializeComponent();
        }

        public ClassNode()
        {
            InitializeComponent();
        }
       

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject container = this.PropertyListBox.ContainerFromElement((DependencyObject)e.Source);

            Model.MemberNode from = (Model.PropertyNode)this.PropertyListBox.ItemContainerGenerator.ItemFromContainer(container);

            Model.ClassGlyph clsGlyph = (Model.ClassGlyph)this.DataContext;

            Model.MemberNode to = clsGlyph.Members[clsGlyph.Members.IndexOf(from) - 1];

            clsGlyph.Members.Lock();

            long temp = from.DisplayOrder;
            from.DisplayOrder = to.DisplayOrder;
            to.DisplayOrder = temp;
            clsGlyph.Members.Unlock();
            clsGlyph.EditingState = EditingState.Dirty;
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject container = this.PropertyListBox.ContainerFromElement((DependencyObject)e.Source);

          

            Model.MemberNode from = (Model.PropertyNode)this.PropertyListBox.ItemContainerGenerator.ItemFromContainer(container);

            Model.ClassGlyph clsGlyph = (Model.ClassGlyph)this.DataContext;

            Model.MemberNode to = clsGlyph.Members[clsGlyph.Members.IndexOf(from) + 1];

            clsGlyph.Members.Lock();

            long temp = from.DisplayOrder;
            from.DisplayOrder = to.DisplayOrder;
            to.DisplayOrder = temp;
            clsGlyph.Members.Unlock();
            clsGlyph.EditingState = EditingState.Dirty;

        }

        private WeakEventListener _propertyNodesChangedListener;

        private Fantasy.ServiceModel.ServiceContainer _childSite;

        private Fantasy.ServiceModel.ServiceContainer ChildSite
        {
            get
            {
                if (_childSite == null)
                {

                    m.ClassGlyph model = this.DataContext as m.ClassGlyph;
                    _childSite = new ServiceModel.ServiceContainer(model.Site);
                    _childSite.AddService(model);
                    _childSite.AddService(model.Diagram);
                    
                }
                return _childSite;
            }

        }

        private void Node_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this._childSite = null;
            m.ClassGlyph model = this.DataContext as m.ClassGlyph;
            this.CommandBindings.Clear();

            ISelectionService selectionService = model.Site.GetService<ISelectionService>();
            selectionService.SelectionChanged += new EventHandler(SelectionService_SelectionChanged);

            if (model != null)
            {
                IMenuService svc = model.Site.GetService<IMenuService>();
                if (svc != null)
                {
                    this.ConentStackPanel.ContextMenu = svc.CreateContextMenu("fantasy/studio/businessengine/classdiagrampanel/classglyph/contextmenu", this, this.ChildSite);
                }

                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/classglyph/commandbindings").BuildChildItems(this, this.ChildSite))
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
                m.ClassGlyph model = this.DataContext as m.ClassGlyph;
                model.ShowMember = true;
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
                    m.ClassGlyph model = this.DataContext as m.ClassGlyph;
                    ISelectionService svc = model.Site.GetRequiredService<ISelectionService>();
                    IEnumerable<object> selected = svc.GetSelectedComponents().Cast<object>();

                    var query = from n in this.PropertyListBox.SelectedItems.Cast<m.MemberNode>()
                                where !selected.Any(o => o == n)
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
                    m.ClassGlyph model = this.DataContext as m.ClassGlyph;
                    var q1 = this.PropertyListBox.SelectedItems.Cast<m.MemberNode>();

                    ISelectionServiceEx svc = model.Site.GetRequiredService<ISelectionServiceEx>();

                    svc.SetSelectedComponents(q1.ToArray(), SelectionTypes.Replace);
                    svc.IsReadOnly = model.IsShortCut;
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
