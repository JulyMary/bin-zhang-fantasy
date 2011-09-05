using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    class AddEnumCommand  : ObjectWithSite, System.Windows.Input.ICommand
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
            BusinessEnum @enum = es.AddBusinessEnum(parent);
           
            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();
            IViewContent content = documentService.OpenView(@enum);
            if (content != null)
            {
                content.WorkbenchWindow.Select();
            }
        }

        #endregion
    }
   
}
