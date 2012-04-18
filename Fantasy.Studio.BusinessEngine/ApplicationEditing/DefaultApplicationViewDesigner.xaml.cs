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
using Fantasy.Web.Design;

namespace Fantasy.Studio.BusinessEngine.ApplicationEditing
{
    /// <summary>
    /// Interaction logic for DefaultApplicationViewDesigner.xaml
    /// </summary>
    public partial class DefaultApplicationViewDesigner : UserControl,IObjectWithSite, IViewDesigner
    {
        public DefaultApplicationViewDesigner()
        {
            InitializeComponent();
        }

        #region IViewDesigner Members





        public void LoadSettings(Fantasy.BusinessEngine.IBusinessEntity entity, System.Xml.Linq.XElement setting)
        {
            throw new NotImplementedException();
        }

        public System.Xml.Linq.XElement SaveSettings()
        {
            throw new NotImplementedException();
        }

        public UIElement UI
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler SettingChanged;

        protected virtual void OnSettingChanged(EventArgs e)
        {
            if (this.SettingChanged != null)
            {
                this.SettingChanged(this, e);
            }
        }

        #endregion

        #region IObjectWithSite Members

        public IServiceProvider Site
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
