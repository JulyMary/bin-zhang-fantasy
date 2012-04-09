using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc
{
    public class SetControllerFactoryCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IControllerFactory cf = this.Site.GetRequiredService<IControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(cf);
            return null;

        }

        #endregion
    }
}
