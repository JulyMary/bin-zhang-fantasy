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
using m = Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Model;
using System.ComponentModel.Design;
using Fantasy.Studio.Services;
using Fantasy.AddIns;
using Fantasy.Windows;
using System.Collections.Specialized;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing.Shapes
{
    /// <summary>
    /// Interaction logic for EnumNode.xaml
    /// </summary>
    public partial class EnumNode : ClassDiagramNode
    {
        public EnumNode(Guid id) : base(id)
        {
            InitializeComponent();
        }

        public EnumNode()
        {
            InitializeComponent();
        }

       
        private void ClassDiagramNode_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            m.EnumGlyph model = this.DataContext as m.EnumGlyph;
            this.CommandBindings.Clear();

            ISelectionService selectionService = model.Site.GetService<ISelectionService>();
            selectionService.SelectionChanged += new EventHandler(SelectionService_SelectionChanged);

            if (model != null)
            {
                IMenuService svc = model.Site.GetService<IMenuService>();
                if (svc != null)
                {
                    this.ConentStackPanel.ContextMenu = svc.CreateContextMenu("fantasy/studio/businessengine/classdiagrampanel/enumglyph/contextmenu", this, model.Site);
                }

                foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/enumglyph/commandbindings").BuildChildItems(this, model.Site))
                {
                    this.ConentStackPanel.CommandBindings.Add(cb);
                }

                _propertyNodesChangedListener = new WeakEventListener(this.AutoSelectNewProperty);
                CollectionChangedEventManager.AddListener(model.EnumValues, _propertyNodesChangedListener);

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
                m.EnumGlyph model = this.DataContext as m.EnumGlyph;
                model.ShowMember = true;
                if (args.NewItems != null)
                {

                    this.EnumValueListBox.SelectedItems.Clear();
                    foreach (m.EnumValueNode p in args.NewItems)
                    {
                        this.EnumValueListBox.SelectedItems.Add(p);
                    }

                }
                if (!this.EnumValueListBox.IsKeyboardFocusWithin)
                {
                    this.EnumValueListBox.Focus();
                }
            }

            return true;
        }

        private WeakEventListener _propertyNodesChangedListener;

        void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            if (!_selecting)
            {
                this._selecting = true;
                try
                {
                    m.EnumGlyph model = this.DataContext as m.EnumGlyph;
                    ISelectionService svc = model.Site.GetRequiredService<ISelectionService>();
                    IEnumerable<object> selected = svc.GetSelectedComponents().Cast<object>();

                    var query = from n in this.EnumValueListBox.SelectedItems.Cast<m.EnumValueNode>()
                                where !selected.Any(o => o == n.Entity)
                                select n;

                    foreach (object o in query.ToArray())
                    {
                        this.EnumValueListBox.SelectedItems.Remove(o);
                    };


                }
                finally
                {
                    this._selecting = false;
                }
            }
        }


        private bool _selecting = false;

        private void EnumValueListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_selecting)
            {
                this._selecting = true;
                try
                {
                    m.EnumGlyph model = this.DataContext as m.EnumGlyph;
                    var q1 = this.EnumValueListBox.SelectedItems.Cast<m.EnumValueNode>().Select(n => n.Entity);

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


        private void ConentStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.ConentStackPanel.IsKeyboardFocusWithin)
            {
                this.Focus();
            }
        }
    }
}
