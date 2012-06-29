using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Fantasy.Web.Controllers
{
    public class StandardCollectionController : Controller, ICollectionViewController, ICustomerizableViewController
    {
        #region ICustomerizableViewController Members

        public void LoadSettings(XElement settings)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ICollectionViewController Members

        public ViewResult Default(IEnumerable<BusinessEngine.BusinessObject> collection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}