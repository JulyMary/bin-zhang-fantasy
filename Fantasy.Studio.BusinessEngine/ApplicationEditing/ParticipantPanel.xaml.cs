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
using Fantasy.Studio.Services;
using Fantasy.Windows;
using System.Collections.Specialized;
using Fantasy.AddIns;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    /// <summary>
    /// Interaction logic for ParticipantPanel.xaml
    /// </summary>
    public partial class ParticipantPanel : UserControl, IObjectWithSite, IEntityEditingPanel
    {
        public ParticipantPanel()
        {
            InitializeComponent();
        }

        private void ParticipantsTreeView_PreviewDragEnter(object sender, DragEventArgs e)
        {
            
        }

        private void ParticipantsTreeView_PreviewDragLeave(object sender, DragEventArgs e)
        {
            
        }

        private void ParticipantsTreeView_PreviewDragOver(object sender, DragEventArgs e)
        {
            BusinessClass data = e.Data.GetDataByType<BusinessClass>();

            if (data != null && this.Entity.Participants.Count == 0)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void ParticipantsTreeView_PreviewDrop(object sender, DragEventArgs e)
        {
            BusinessClass data = e.Data.GetDataByType<BusinessClass>();

            if (data != null && this.Entity.Participants.Count == 0)
            {
                e.Effects = DragDropEffects.Copy;
                e.Handled = true;
                this._model.AddRootNodeForClass(data);
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }


        public IServiceProvider Site { get; set; }


        private SelectionService _selection = new SelectionService(null);

        protected override void OnGotKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnGotKeyboardFocus(e);
            if (this.Site != null)
            {
                IMonitorSelectionService monitor = this.Site.GetRequiredService<IMonitorSelectionService>();
                monitor.CurrentSelectionService = this._selection;
            }
        }

        private WeakEventListener _participantsChangedListener;

        #region IEntityEditingPanel Members

        public void Initialize()
        {
            
        }

        private ServiceModel.ServiceContainer _childSite;

        private IServiceProvider GetChildSite()
        {
            if (_childSite == null)
            {
                _childSite = new ServiceModel.ServiceContainer(this.Site);

                _childSite.AddService(this);

                _childSite.AddService(this.Entity);
            }

            return _childSite;
        }

        private ParticipantPanelModel _model;
        public BusinessApplication Entity { get; private set; }


        public void Load(Fantasy.BusinessEngine.IBusinessEntity entity)
        {
            this.Entity = (BusinessApplication)entity;
            this.DirtyState = this.Entity.Participants.Any(p => p.EntityState != EntityState.Clean) ? EditingState.Dirty : EditingState.Clean;

            this._participantsChangedListener = new WeakEventListener((t, sender, e) =>
                {
                    this.DirtyState = EditingState.Dirty;
                    return true;
                });

            CollectionChangedEventManager.AddListener(this.Entity.Participants, this._participantsChangedListener); 

            this._model = new ParticipantPanelModel(this.Entity, this.GetChildSite());
            this.DataContext = _model;
        }

        private EditingState _dirtyState;
        public EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            set
            {
                if (this._dirtyState != value)
                {
                    this._dirtyState = value;
                    if (this.DirtyStateChanged != null)
                    {
                        this.DirtyStateChanged(this, EventArgs.Empty);
                    }
                }
            }
        }


        public event EventHandler DirtyStateChanged;

        public void Save()
        {
            this._model.Save();
            this.DirtyState = EditingState.Clean; 
 
        }

        public UIElement Element
        {
            get { return this; }
        }

        public string Title
        {
            get { return Properties.Resources.ParticipantPanelTitle; }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            
        }

        public void ViewContentSelected()
        {
            if (this._model != null)
            {
                this._model.Refresh();
            }
        }

        public void ViewContentDeselected()
        {
            
        }

        #endregion


        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnIsKeyboardFocusWithinChanged(e);
            
        }


    }
}
