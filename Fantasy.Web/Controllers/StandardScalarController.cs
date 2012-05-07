﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Controllers
{
    
    public class StandardScalarController : Controller, IScalarViewController//, ICustomerizableViewController
    {
        #region ICustomerizableViewController Members

        public void LoadSettings(object settings)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region IScalarViewController Members

        public ViewResult Default(Guid objId)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
            BusinessObject obj = es.Get<BusinessObject>(objId);

            return View(obj);
        }

        #endregion
    }
}