﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine.Events
{
    public class BusinessClassCreateTableHandler : ObjectWithSite, IEntityEventHandler
    {
        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessClass @class = (BusinessClass)e.Entity;
            IDDLService ddl = this.Site.GetRequiredService<IDDLService>();
            ddl.CreateClassTable(@class);
        }

        #endregion
    }
}