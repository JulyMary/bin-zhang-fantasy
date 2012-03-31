using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;
using Fantasy.Adaption;

namespace Fantasy.Studio.BusinessEngine.UserRoleEditing
{
    public class AddUserCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();
            BusinessPackage package = am.GetAdapter<BusinessPackage>(parameter);
            return !(package.EntityState == EntityState.New || package.EntityState == EntityState.Deleted);

        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            IAdapterManager am = this.Site.GetRequiredService<IAdapterManager>();
            BusinessPackage parent = am.GetAdapter<BusinessPackage>(parameter);
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessUserData user = es.AddBusinessUser(parent);

            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();
            IViewContent content = documentService.OpenView(user);
            if (content != null)
            {
                content.WorkbenchWindow.Select();
            }
        }

        #endregion
    }
}
