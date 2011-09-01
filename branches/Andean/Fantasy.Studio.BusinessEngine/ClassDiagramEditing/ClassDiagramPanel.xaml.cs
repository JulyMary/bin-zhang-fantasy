﻿using System;
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
                    var query = from shape in this.diagramView.SelectionList.Cast<Node>() select shape.DataContext;

                    if (query.Count() > 0)
                    {

                        this._selectionService.SetSelectedComponents(query.ToArray(), SelectionTypes.Replace);
                        this._selectionService.IsReadOnly = query.Any(n => ((Model.ClassNode)n).IsShortCut); 
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

                    foreach (Node shape in this.diagramView.SelectionList.Cast<Node>().ToArray())
                    {
                        if (Array.IndexOf(selected, shape.DataContext) <= 0)
                        {
                            this.diagramView.SelectionList.Remove(shape);
                        }
                    }

                    var query = from shape in this.diagramControlModel.Nodes.Cast<Node>()
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

        public new UIElement Content
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
