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
using NHibernate;
using System.ComponentModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.AddIns;
using System.ComponentModel.Design;
using Fantasy.Studio.Services;
using Fantasy.XSerialization;
using System.Xml.Linq;
using Syncfusion.Windows.Diagram;
using System.Diagnostics;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    /// <summary>
    /// Interaction logic for ClassDiagramPanel.xaml
    /// </summary>
    public partial class ClassDiagramPanel : UserControl, IEntityEditingPanel, IObjectWithSite 
    {
        public ClassDiagramPanel()
        {
            InitializeComponent();

        }


        private bool _selecting = false;

        void ViewSelectionCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!this._selecting)
            {
                this._selecting = true;
                try
                {
                    var query = from shape in this.diagramView.SelectionList.FilterAndCast<ContentControl>() select shape.DataContext;

                    if (query.Count() > 0)
                    {

                        this._selectionService.SetSelectedComponents(query.ToArray(), SelectionTypes.Replace);
                        this._selectionService.IsReadOnly = query.FilterAndCast<Model.ClassGlyph>().Any(n=>n.IsShortCut);
 
                    }
                    else
                    {
                        this._selectionService.SetSelectedComponents(new object[] {this._entity}, SelectionTypes.Replace);
                    }

                }
                finally
                {
                    this._selecting = false;
                }
            }
        }

        void SelectionService_SelectionChanged(object sender, EventArgs e)
        {
            if (!this._selecting)
            {

                this._selecting = true;
                try
                {

                    object[] selected = this._selectionService.GetSelectedComponents().Cast<object>().ToArray();

                    foreach (ContentControl shape in this.diagramView.SelectionList.Cast<ContentControl>().ToArray())
                    {
                        if (Array.IndexOf(selected, shape.DataContext) <= 0)
                        {
                            this.diagramView.SelectionList.Remove(shape);
                        }
                    }

                    var query = from shape in this.diagramControlModel.Nodes.Cast<ContentControl>().Union(this.diagramControlModel.Connections.Cast<ContentControl>())
                                where Array.IndexOf(selected, shape.DataContext) >= 0 && this.diagramView.SelectionList.IndexOf(shape) < 0
                                select shape;
                    this.diagramView.SelectionList.AddRange(query.ToArray());
                }
                finally
                {
                    this._selecting = false;
                }

                
            }
        }



        public ClassDiagramMode Mode
        {
            get { return (ClassDiagramMode)GetValue(ModeProperty); }
            set 
            {
                if (this.Mode != value)
                {
                    SetValue(ModeProperty, value);
                    this.OnModeChanged();
                }
            }
        }

        private void OnModeChanged()
        {
            switch (this.Mode)
            {

                case ClassDiagramMode.InheritanceConnection:
                    {
                        Connection.ConnectionAdorner adorner = new Connection.ConnectionAdorner(this.diagramView, this.GetChildSite());
                        adorner.ValidateFirstNode += new EventHandler<Connection.ValidatNodeArgs>(ValidateSubclass);
                        adorner.ValidateSecondNode += new EventHandler<Connection.ValidatNodeArgs>(ValidateParentClass);
                        adorner.CreatConnection += new EventHandler(CreateInheritance);
                        adorner.Exit += new EventHandler(ExitCreateInheritance);
                    }

                    break;
                case ClassDiagramMode.RelationConnection:
                    break;
               
            }
        }

        void ExitCreateInheritance(object sender, EventArgs e)
        {
            Connection.ConnectionAdorner adorner = (Connection.ConnectionAdorner)sender;
            adorner.ValidateFirstNode -= new EventHandler<Connection.ValidatNodeArgs>(ValidateSubclass);
            adorner.ValidateSecondNode -= new EventHandler<Connection.ValidatNodeArgs>(ValidateParentClass);
            adorner.CreatConnection -= new EventHandler(CreateInheritance);
            adorner.Exit -= new EventHandler(ExitCreateInheritance);
            this.Mode = ClassDiagramMode.Default;
        }

        void CreateInheritance(object sender, EventArgs e)
        {
            Connection.ConnectionAdorner adorner = (Connection.ConnectionAdorner)sender;



            Model.InheritanceGlyph inheritance = new Model.InheritanceGlyph()
            {
                DerivedClass = (Model.ClassGlyph)adorner.FirstNode.DataContext,
                DerivedGlyphId = adorner.FirstNode.ID,
                BaseClass = (Model.ClassGlyph)adorner.SecondNode.DataContext,
                BaseGlyphId = adorner.SecondNode.ID,
                EditingState = EditingState.Dirty,
            };

            BusinessClass childEntity = inheritance.DerivedClass.Entity;
            BusinessClass parentEntity = inheritance.BaseClass.Entity;

            if (childEntity.ParentClass != parentEntity)
            {
                if (childEntity.ParentClass != null)
                {
                    childEntity.ParentClass.ChildClasses.Remove(childEntity);
                    parentEntity.ChildClasses.Add(childEntity);
                    childEntity.ParentClass = parentEntity;
                }

            }

            this._classDiagram.Inheritances.Add(inheritance);

            this._selectionService.SetSelectedComponents(new object[] { inheritance }, SelectionTypes.Replace);
           
        }

        void ValidateParentClass(object sender, Connection.ValidatNodeArgs e)
        {
            Model.ClassGlyph parent = (Model.ClassGlyph)e.Node.DataContext;

            Connection.ConnectionAdorner adorner = (Connection.ConnectionAdorner)sender;

            Model.ClassGlyph child = (Model.ClassGlyph)adorner.FirstNode.DataContext;

            if (child.Entity.EntityState == EntityState.New)
            {
                e.IsValid = !parent.Entity.Flatten(c => c.ParentClass).Any(c => c == child.Entity); 
            }
            else
            {
                e.IsValid = child.Entity.ParentClass == parent.Entity;
            }

             
        }

        void ValidateSubclass(object sender, Connection.ValidatNodeArgs e)
        {
            Model.ClassGlyph glyph = (Model.ClassGlyph)e.Node.DataContext;

            var hasInheritance = from inheritance in this._classDiagram.Inheritances where inheritance.DerivedClass.Entity == glyph.Entity select inheritance;

            if (!glyph.IsShortCut && !hasInheritance.Any())
            {
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
            }


        }

        // Using a DependencyProperty as the backing store for Mode.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register("Mode", typeof(ClassDiagramMode), typeof(ClassDiagramPanel), new UIPropertyMetadata(ClassDiagramMode.Default));

        private IServiceProvider _site;
        [Browsable(false)]
        public IServiceProvider Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
              

            }
        }


        private ClassDiagramPanelModel _viewModel;


        private ServiceModel.ServiceContainer _childSite;

        private IServiceProvider GetChildSite()
        {
            if (_childSite == null)
            {
                _childSite = new ServiceModel.ServiceContainer(this.Site);
                _childSite.AddService(this._selectionService);
                _childSite.AddService(this._classDiagram);
                _childSite.AddService(this.diagramControlModel);
                _childSite.AddService(this.diagramControl);
                _childSite.AddService(this.diagramControl.View);
                _childSite.AddService(this);

                _childSite.AddService(this._entity);
            }

            return _childSite;
        }

        private SelectionService _selectionService;

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = _selectionService;
            }
            base.OnGotKeyboardFocus(e);
        }

        #region IEntityEditingPanel Members

        public void Initialize()
        {
           
        }

        private BusinessClassDiagram _entity;
        private Model.ClassDiagram _classDiagram;
        private ClassDiagramModelBinder _binder; 

        public void Load(Fantasy.BusinessEngine.IBusinessEntity data)
        {

            this._selectionService = new SelectionService(this.Site);
            this._selectionService.SelectionChanged += new EventHandler(SelectionService_SelectionChanged);
            this.diagramView.SelectionList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(ViewSelectionCollectionChanged);


            this._entity = (BusinessClassDiagram)data;

            this._classDiagram = new Model.ClassDiagram();
            Fantasy.ServiceModel.ServiceContainer site = new ServiceModel.ServiceContainer(this.Site);
            site.AddService(this._selectionService);
            site.AddService(this);
            site.AddService(this._entity);

            this._classDiagram.Site = site;

            this._classDiagram.LoadDiagram(this._entity); 

            this._viewModel = new ClassDiagramPanelModel();
            this._viewModel.Site = this.GetChildSite();

            this._binder = new ClassDiagramModelBinder(this._classDiagram, this.diagramControlModel);

            this._classDiagram.PropertyChanged += new PropertyChangedEventHandler(ClassDiagramPropertyChanged);

            this.DataContext = this._viewModel;
            

            if (this._classDiagram.EditingState == EditingState.Dirty)
            {
                this.OnDirtyStateChanged(EventArgs.Empty);
            }

            this._selectionService.SetSelectedComponents(new object[] { this._entity });



            IMenuService svc = this.Site.GetService<IMenuService>();
            if (svc != null)
            {
                this.diagramView.NodeContextMenu = this.diagramView.ContextMenu = svc.CreateContextMenu("fantasy/studio/businessengine/classdiagrampanel/contextmenu", this, this.GetChildSite());
            }

            foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/commandbindings").BuildChildItems(this, this.GetChildSite()))
            {
                this.diagramView.CommandBindings.Add(cb);
            }

            
        }

        void ClassDiagramPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "EditingState")
            {
                OnDirtyStateChanged(EventArgs.Empty);
            }
        }


      
 
        public EditingState DirtyState
        {
            get
            {
                return this._classDiagram != null ? this._classDiagram.EditingState : EditingState.Clean;  
            }
           
        }

        public event EventHandler DirtyStateChanged;

        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
            CommandManager.InvalidateRequerySuggested();
        }

        public void Save()
        {
            this._classDiagram.Save();
            this._classDiagram.EditingState = EditingState.Clean; 
        }

        public UIElement Element
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.ClassDiagramPanelTitle; }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Closed()
        {
            this._classDiagram.PropertyChanged -= new PropertyChangedEventHandler(ClassDiagramPropertyChanged);
            this._classDiagram.Unload();
        }

        #endregion

       

        private void DiagramView_DragEnter(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/enterhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void DiagramView_DragOver(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/overhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void DiagramView_DragLeave(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/leavehandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void DiagramView_Drop(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/drophandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.GetChildSite());
            foreach (IEventHandler<DragEventArgs> handler in handlers)
            {
                handler.HandleEvent(this, e);
                if (e.Handled)
                {
                    break;
                }
            }

            if (!e.Handled)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void UserControl_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            //Debug.WriteLine(e.NewFocus.GetType().ToString());
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            
            base.OnIsKeyboardFocusWithinChanged(e);

            if (this.IsKeyboardFocusWithin && this._classDiagram != null)
            {
                this._classDiagram.SyncEntities();  
            }

        }

        public void ViewContentSelected()
        {
            
        }

        public void ViewContentDeselected()
        {
           
        }

       
    }
}
