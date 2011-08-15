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

        [Browsable(false)]
        public IServiceProvider Site { get; set; }

        #region IEntityEditingPanel Members

        public void Initialize()
        {
            
        }

        private BusinessClassDiagram _entity;

        public void Load(Fantasy.BusinessEngine.IBusinessEntity data)
        {
            this._entity = (BusinessClassDiagram)data;
            this._entity.EntityStateChanged += new EventHandler(EntityStateChanged);
            this._entity.PropertyChanged += new EventHandler<Fantasy.BusinessEngine.Events.EntityPropertyChangedEventArgs>(Entity_PropertyChanged);
           
            this.DirtyState = this._entity.EntityState == EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
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

        private void DiagramControl_DragEnter(object sender, DragEventArgs e)
        {
            
        }
    }
}
