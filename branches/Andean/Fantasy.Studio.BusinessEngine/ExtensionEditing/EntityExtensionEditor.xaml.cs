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
using Fantasy.Studio.Services;
using Fantasy.Windows;
using System.ComponentModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;
using Fantasy.AddIns;

namespace Fantasy.Studio.BusinessEngine.ExtensionEditing
{
    /// <summary>
    /// Interaction logic for EntityExtensionEditor.xaml
    /// </summary>
    public partial class EntityExtensionEditor : UserControl, IObjectWithSite, IViewContent, IEditingViewContent
    {
        public EntityExtensionEditor()
        {
            InitializeComponent();
        }

        public IServiceProvider Site { get; set; }


        private ExtensionData _data;


        public void Initialize()
        {

        }

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

        private WeakEventListener _entityListener;

        private WeakEventListener _dataListener;

        private EntityExtensionEditorModel _model;

        public void Load(object document)
        {
            this._data = (ExtensionData)document;
            this.DirtyState = this._data.Entity.EntityState == Fantasy.BusinessEngine.EntityState.Clean ? EditingState.Clean : EditingState.Dirty;
            _entityListener = new WeakEventListener((t, sender, e) =>
            {
                switch (((PropertyChangedEventArgs)e).PropertyName)
                {
                    case "EntityState":
                        switch (this._data.Entity.EntityState)
                        {
                            case Fantasy.BusinessEngine.EntityState.New:
                                this.DirtyState = EditingState.Dirty;
                                break;
                            case Fantasy.BusinessEngine.EntityState.Clean:
                                this.DirtyState = EditingState.Clean;
                                break;
                            case Fantasy.BusinessEngine.EntityState.Dirty:
                                this.DirtyState = EditingState.Dirty;
                                break;
                            case Fantasy.BusinessEngine.EntityState.Deleted:
                                this.DirtyState = EditingState.Clean;
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    this.Site.GetRequiredService<IWorkbench>().CloseContent(this);
                                }));

                                break;

                        }
                        return true;
                    default:
                        return false;
                }

            });

            PropertyChangedEventManager.AddListener(this._data.Entity, _entityListener, "EntityState");

            this._dataListener = new WeakEventListener((t, sender, e) =>
            {
                switch (((PropertyChangedEventArgs)e).PropertyName)
                {
                    case "Name":
                        this.OnTitleChanged(EventArgs.Empty);
                        return true;
                    default:
                        return false;
                }
            });

            PropertyChangedEventManager.AddListener(this._data, _dataListener, "Name");


            _model = new EntityExtensionEditorModel(this._data, this.Site);
            this.DataContext = _model;


            foreach (ToolBar bar in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/entityextensioneditor/toolbars").BuildChildItems(this._model, this.Site))
            {
                this.ToolBarTray.ToolBars.Add(bar);
            }

            foreach (CommandBinding cb in AddInTree.Tree.GetTreeNode("fantasy/studio/businessengine/entityextensioneditor/commandbindings").BuildChildItems(this._model, this.Site))
            {
                this.CommandBindings.Add(cb);
            }
        }


        private bool IsReadOnly
        {
            get
            {
                return this._data.Entity.IsSystem && !CommandArgumentsHelper.HasArgument("EditAll");
            }
        }

        private EditingState _dirtyState = EditingState.Clean;
        public virtual EditingState DirtyState
        {
            get
            {
                return _dirtyState;
            }
            protected set
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
            IEntityService es = this.Site.GetRequiredService<IEntityService>();

            es.BeginUpdate();
            try
            {
                es.SaveOrUpdate(this._data.Entity);
                es.EndUpdate(true);
            }
            catch
            {
                es.EndUpdate(false);
                throw;
            }

            this.DirtyState = EditingState.Clean;
        }

        public UIElement Element
        {
            get { return this; }
        }

        public object Data
        {
            get { return this._data; }
        }

        public IWorkbenchWindow WorkbenchWindow { get; set; }


        public string Title
        {
            get { return this._data.Name; }

        }

        public event EventHandler TitleChanged;

        protected virtual void OnTitleChanged(EventArgs e)
        {
            if (this.TitleChanged != null)
            {
                this.TitleChanged(this, e);
            }
        }

        public void Selected()
        {

        }

        public void Deselected()
        {

        }

        public void Closing(CancelEventArgs e)
        {

        }

        public void Closed()
        {

        }



        public string DocumentName
        {
            get { return this._data.Name; }
        }

        public string DocumentType
        {
            get { return _data.Type; }
        }


        private ImageSource _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/extension.png", UriKind.RelativeOrAbsolute));
        public ImageSource Icon
        {

            get
            {
                return _icon;
            }
        }

        public event EventHandler IconChanged;

        protected virtual void OnIconChanged(EventArgs e)
        {
            if (this.IconChanged != null)
            {
                this.IconChanged(this, e);
            }
        }



        public void Dispose()
        {

        }

        private void ExtensionListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var query = from item in this.ExtensionListView.SelectedItems.Cast<EntityExtensionModel>()
                        select item.Extension;
            this._selection.SetSelectedComponents(query.ToArray());

            if(e.AddedItems != null)
            {
                foreach (EntityExtensionModel m in e.AddedItems)
                {
                    this._model.Selected.Add(m.Extension);
                }

            }
            if (e.RemovedItems != null)
            {
                foreach (EntityExtensionModel m in e.RemovedItems)
                {
                    this._model.Selected.Remove(m.Extension);
                }
            }

            this._model.PrimarySelected = this.ExtensionListView.SelectedItem as EntityExtensionModel;
        }

        private void ExtensionListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (!this.IsReadOnly)
            {
                IEntityExtension value = e.Data.GetDataByType<IEntityExtension>();
                if (value != null && this.CanAdd(value))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }
        }

        private bool CanAdd(IEntityExtension value)
        {
            return value.ApplyTo(this.Data);
        }

        private void ExtensionListView_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (!this.IsReadOnly)
            {
                IEntityExtension value = e.Data.GetDataByType<IEntityExtension>();
                if (value != null && this.CanAdd(value))
                {
                    e.Effects = DragDropEffects.Copy;
                }
            }

        }

        private void ExtensionListView_Drop(object sender, DragEventArgs e)
        {
           
            if (!this.IsReadOnly)
            {
                IEntityExtension value = e.Data.GetDataByType<IEntityExtension>();
                if (value != null && this.CanAdd(value) && RemoveOldSingleInstanceExtension(value))
                {
                    this._data.Extensions.Add(value);
                    EntityExtensionModel item = this._model.Extensions.Single(i => i.Extension == value);
                    this.ExtensionListView.SelectedItem = item;

                }
            }

        }

        private bool RemoveOldSingleInstanceExtension(IEntityExtension value)
        {
            var q1 = from t in value.GetType().Flatten(t => t.BaseType)
                     let attr = t.GetCustomAttribute<ExtensionUsageAttribute>(false, false)
                     where attr != null && attr.AllowMultiple == false
                     select t;
            Type type = q1.SingleOrDefault();

            bool rs = true;
            if (type != null)
            {
                IEntityExtension old = this._data.Extensions.FirstOrDefault(ex => type.IsInstanceOfType(ex));
                if (old != null)
                {
                    IMessageBoxService msgBox = this.Site.GetRequiredService<IMessageBoxService>();
                    if (msgBox.Show(String.Format("There is already a {0} in {1}, replace it?", old.Name, this._data.Name),
                        button: MessageBoxButton.YesNo, icon: MessageBoxImage.Question,
                        defaultResult: MessageBoxResult.No) == MessageBoxResult.Yes)
                    {
                        this._data.Extensions.Remove(old);
                       
                    }
                    else
                    {
                        rs = false;
                    }
                }



            }
            return rs;
        }


    }
}
