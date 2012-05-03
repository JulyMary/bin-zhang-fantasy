using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Controllers
{
    public class StandardScalarController : Controller, IScalarViewController, ICustomerizableViewController
    {
        #region ICustomerizableViewController Members

        public void LoadSettings(object settings)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IScalarViewController Members

        public ViewResult Default(BusinessEngine.BusinessObject obj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}