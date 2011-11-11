using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using System.Web;

namespace Fantasy.Web.Mvc
{
    public class HttpContextBusinessEngineContextProvider : IBusinessEngineContextProvider
    {
        #region IBusinessEngineContextProvider Members

        private const string _businessEngineContextKey = "HTTPCONTEXTBUSINESSENGINECONTEXT";


        public BusinessEngineContext GetCurrent()
        {
            return (BusinessEngineContext)HttpContext.Current.Items[_businessEngineContextKey];
        }

        public void SetCurrent(BusinessEngineContext value)
        {
            HttpContext.Current.Items[_businessEngineContextKey] = value;
        }

        #endregion
    }
}
