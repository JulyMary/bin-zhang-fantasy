using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class DeleteClassCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            BusinessClass @class = (BusinessClass)parameter;
            return @class.ChildClasses.Count == 0 && @class.EntityState != EntityState.Deleted && !@class.IsSystem;
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged {add{} remove{}}

        public void Execute(object parameter)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessClass @class = (BusinessClass)parameter;
           
                @class.Package.Classes.Remove(@class);
                es.Delete(@class);
               
                this.Site.GetRequiredService<IEditingService>().CloseView(@class, true);
           

        }

        #endregion
    }
}
