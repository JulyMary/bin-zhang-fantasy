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


        private ClassDiagramPanelModel _model;


        private IServiceProvider CreateChildSite()
        {
            Fantasy.ServiceModel.ServiceContainer rs = new ServiceModel.ServiceContainer(this.Site);
            rs.AddService(this._selectionService);
            rs.AddService(this._model.DiagramModel);
            rs.AddService(this._model.ViewModel);
            rs.AddService(this.diagramControl);
            rs.AddService(this.diagramControl.View);
            rs.AddService(this._entity);

            return rs;
        }

        private ISelectionService _selectionService = new SelectionService(null);

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

        public void Load(Fantasy.BusinessEngine.IBusinessEntity data)
        {

            this._model = new ClassDiagramPanelModel();

            this._entity = (BusinessClassDiagram)data;

            //TODO: deserialize diagrammodel from entity


            this._entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this._entity.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Entity_PropertyChanged);
           
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            this.DataContext = this._model;
        }

        void Entity_PropertyChanged(object sender, Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs e)
        {
            
        }

        void EntityStateChanged(object sender, EventArgs e)
        {
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            CommandManager.InvalidateRequerySuggested();
        }

        private EditingState _dirtyState = EditingState.Clean;
        public EditingState DirtyState
        {
            get { return _dirtyState; }
            set
            {
                if (_dirtyState != value)
                {
                    this._dirtyState = value;
                    this.OnDirtyStateChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler DirtyStateChanged;

        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
        }

        public void Save()
        {
            ISession session = this.Site.GetRequiredService<IEntityService>().DefaultSession;

            session.BeginUpdate();
            try
            {
                //TODO: serialize diagram model to entity

                session.SaveOrUpdate(this._entity);
                session.EndUpdate(true);
            }
            catch
            {
                session.EndUpdate(false);
                throw;
            }
            this.DirtyState = EditingState.Clean;
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
            this._entity.EntityStateChanged -= new EventHandler(EntityStateChanged);
            this._entity.PropertyChanged -= new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Entity_PropertyChanged);
        }

        #endregion

       

        private void DiagramView_DragEnter(object sender, DragEventArgs e)
        {
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/enterhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.CreateChildSite());
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
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/overhandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.CreateChildSite());
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
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/leavehandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.CreateChildSite());
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
            IEnumerable<IEventHandler<DragEventArgs>> handlers = AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/classdiagrampanel/dragdrop/drophandlers").BuildChildItems<IEventHandler<DragEventArgs>>(this, this.CreateChildSite());
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
    }
}
