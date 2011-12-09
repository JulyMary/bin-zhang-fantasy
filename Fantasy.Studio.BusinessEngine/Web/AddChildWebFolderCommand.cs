using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Web
{
    public class AddChildWebFolderCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            BusinessWebFolder parent = (BusinessWebFolder)parameter;
            return !(parent.EntityState == EntityState.New || parent.EntityState == EntityState.Deleted);
        
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            BusinessWebFolder parent = (BusinessWebFolder)parameter;
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessWebFolder folder = es.AddChildWebFolder(parent);

           
        }

        #endregion
    }
   
}
