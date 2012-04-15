using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;

namespace Fantasy.Web
{
    public interface IScalarViewController
    {
        ViewResult Default(BusinessObject obj);
    }
}