﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.UI
{
    public interface IClientSideScriptFactory
    {
        void Register(string id, string content);
    }
}