using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.CodeGenerating
{
    public class PackageChangedHandler : ObjectWithSite, IEntityEventHandler
    {

        #region IEntityEventHandler Members

        public void Execute(EntityEventArgs e)
        {
            BusinessPackage package = (BusinessPackage)e.Entity;

            IBusinessPackageCodeGenerator svc = this.Site.GetRequiredService<IBusinessPackageCodeGenerator>();
            if (!package.IsSystem)
            {
                svc.RegisterPackage(package);
            }
        }

        #endregion
    }
}
