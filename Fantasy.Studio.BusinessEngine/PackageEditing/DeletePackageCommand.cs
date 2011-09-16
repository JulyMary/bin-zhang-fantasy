using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class DeletePackageCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public object Execute(object args)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessPackage package = (BusinessPackage)args;

            package.ParentPackage.ChildPackages.Remove(package);
            package.ParentPackage = null;

            if (package.EntityState != EntityState.New && package.EntityState != EntityState.Deleted)
            {
                es.Delete(package);
            }

            return null;
        }

        #endregion
    }
}
