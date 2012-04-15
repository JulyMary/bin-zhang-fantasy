using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Design
{
    public class StandardNavigationViewDesigner : ObjectWithSite, IViewDesigner
    {

        private BusinessApplicationData _entity;

        #region IViewDesigner Members

        public void LoadSettings(IBusinessEntity entity, System.Xml.Linq.XElement setting)
        {
            this._entity = (BusinessApplicationData)entity;
        }

        public System.Xml.Linq.XElement SaveSettings(System.Xml.Linq.XElement setting)
        {
            throw new NotImplementedException();
        }


        private StandardNavigationViewDesignerView _ui;

        public System.Windows.UIElement UI
        {
            get
            {
                if (this._ui == null)
                {
                    this._ui = new StandardNavigationViewDesignerView();

                }
            }
        }

        public event EventHandler SettingChanged;

        #endregion
    }
}
