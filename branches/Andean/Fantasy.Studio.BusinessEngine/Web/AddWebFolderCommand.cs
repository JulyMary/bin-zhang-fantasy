using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Web
{
    class AddWebFolderCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            BusinessPackage package = (BusinessPackage)parameter;
            return !(package.EntityState == EntityState.New || package.EntityState == EntityState.Deleted);
        
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            BusinessPackage parent = (BusinessPackage)parameter;
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessWebFolder folder = es.AddStandardWebFolder(parent);
        }

        #endregion
    }
}
