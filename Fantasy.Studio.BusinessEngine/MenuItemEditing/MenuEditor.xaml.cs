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

namespace Fantasy.Studio.BusinessEngine.MenuItemEditing
{
    /// <summary>
    /// Interaction logic for MenuEditor.xaml
    /// </summary>
    public partial class MenuEditor : UserControl, IViewContent, IEditingViewContent, IObjectWithSite
    {
        public MenuEditor()
        {
            InitializeComponent();
        }

        public IServiceProvider Site { get; set; }

        #region IEditingViewContent Members



        public void Load(object data)
        {
            
            this.Data = data;
            _model = new MenuEditorModel(this.Site);
            this.DataContext = _model;
        }

        private MenuEditorModel _model;

        public object Data { get; private set; }
       

        public EditingState DirtyState
        {
            get { return EditingState.Clean; }
        }

        protected virtual void OnDirtyStateChanged(EventArgs e)
        {
            if (this.DirtyStateChanged != null)
            {
                this.DirtyStateChanged(this, e);
            }
        }

        public event EventHandler DirtyStateChanged;

        public void Save()
        {
            this._model.Save();
        }

        #endregion

        #region IViewContent Members

        public UIElement Element
        {
            get { return this; }
        }

        public IWorkbenchWindow WorkbenchWindow {get;set;}
       

        public string Title
        {
            get { return Properties.Resources.MenuEditorName; }
        }

        public void Selected()
        {
           
        }

        public void Deselected()
        {
            
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            
        }

        public void Closed()
        {
            
        }

        event EventHandler IViewContent.TitleChanged {add{} remove{}}

        public string DocumentName
        {
            get { return Properties.Resources.MenuEditorName; }
        }

        public string DocumentType
        {
            get { return "MenuEditor"; }
        }

        private ImageSource _icon;
        public ImageSource Icon
        {
            get
            {
                if (_icon == null)
                {
                    _icon = new BitmapImage(new Uri("/Fantasy.Studio.BusinessEngine;component/images/menuedit.png", UriKind.Relative));
                }
                return _icon;
            }
        }

        event EventHandler IViewContent.IconChanged { add { } remove { } }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
        }

        #endregion

        private void MemberOfListView_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void MemberOfListView_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void MemberOfListView_DragOver(object sender, DragEventArgs e)
        {

        }

        private void MemberOfListView_Drop(object sender, DragEventArgs e)
        {

        }

        private void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveRoleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AutoSelectAllByMouse(object sender, MouseEventArgs e)
        {

        }

        private void AutoSelectAll(object sender, KeyboardFocusChangedEventArgs e)
        {

        }
    }
}
