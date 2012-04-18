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

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    /// <summary>
    /// Interaction logic for BusinessApplicationViewDesignPanel.xaml
    /// </summary>
    public partial class BusinessApplicationViewDesignPanel : UserControl,IObjectWithSite , IDocumentEditingPanel
    {
        public BusinessApplicationViewDesignPanel()
        {
            InitializeComponent();
        }


        public IServiceProvider Site { get; set; }



        #region IDocumentEditingPanel Members

        public void Initialize()
        {
            
        }

        public void Load(object document)
        {
            throw new NotImplementedException();
        }

        public EditingState DirtyState
        {
            get { throw new NotImplementedException(); }
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
            throw new NotImplementedException();
        }

        public UIElement Element
        {
            get { throw new NotImplementedException(); }
        }

        public string Title
        {
            get { throw new NotImplementedException(); }
        }

        public void Closing(System.ComponentModel.CancelEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Closed()
        {
            throw new NotImplementedException();
        }

        public void ViewContentSelected()
        {
            throw new NotImplementedException();
        }

        public void ViewContentDeselected()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
