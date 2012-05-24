using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Mvc;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Mvc
{
    public class RegisterBusinessObjectModelBinderCommand : ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            ModelBinders.Binders.Add(typeof(BusinessObject), new BusinessObjectModelBinder());
            return null;
        }

        #endregion
    }
}