using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.ServiceModel;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc
{
    public class RegisterSystemGlobalFiltersCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            return null;
        }

        #endregion
    }
}