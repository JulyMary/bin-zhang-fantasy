using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class DeleteEnumCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            BusinessEnum @enum = (BusinessEnum)parameter;
            return @enum.EntityState != EntityState.Deleted && !@enum.IsSystem;
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged {add{} remove{}}

        public void Execute(object parameter)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            BusinessEnum @enum = (BusinessEnum)parameter;
            es.DefaultSession.BeginUpdate();
            try
            {
                @enum.Package.Enums.Remove(@enum);

                if (@enum.EntityState != EntityState.New)
                {
                    es.DefaultSession.Delete(@enum);
                }
                es.DefaultSession.EndUpdate(true);
                this.Site.GetRequiredService<IEditingService>().CloseView(@enum, true);
            }
            catch
            {
                es.DefaultSession.EndUpdate(false);
            }

        }

        #endregion
    }
}
