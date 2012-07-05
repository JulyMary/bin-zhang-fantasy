using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Controllers
{
    public class JobServiceErrorsAttribute : FilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter Members

        public void OnException(ExceptionContext filterContext)
        {
            if (WCFExceptionHandler.CanCatch(filterContext.Exception))
            {
                filterContext.HttpContext.Response.Redirect("/Home/ServiceUnavailable");
            }
            else
            {
                filterContext.ExceptionHandled = false;
            }
        }

        #endregion
    }
}