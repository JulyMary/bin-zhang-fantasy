﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using System.Web.Mvc;

namespace Fantasy.Web
{
    public interface INavigationViewController : IController
    {
        ViewResult Default(Guid? objId);
    }
}