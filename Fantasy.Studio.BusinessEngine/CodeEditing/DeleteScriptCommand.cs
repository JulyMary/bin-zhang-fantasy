using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.CodeEditing
{
    public class DeleteScriptCommand : ObjectWithSite, System.Windows.Input.ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            if (parameter == null)
            {
                return false;
            }
            BusinessScript script = (BusinessScript)parameter;
            return script.EntityState != EntityState.Deleted && !script.IsSystem;
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged {add{} remove{}}

        public void Execute(object parameter)
        {
            IEntityService es = this.Site.GetRequiredService<IEntityService>();
            IEditingService edtingSvc = this.Site.GetRequiredService<IEditingService>();
            BusinessScript script = (BusinessScript)parameter;
            BusinessPackage package = script.Package;
            BusinessScript[] scripts = script.Flatten(s=>package.Scripts.Where(s1=>String.Equals(s1["DependentUpon"], s.Name, StringComparison.OrdinalIgnoreCase))).ToArray(); 
            foreach(BusinessScript s in scripts)
            {
                package.Scripts.Remove(s);
                es.Delete(s);
                edtingSvc.CloseView(s, true);

            }

        }

        #endregion
    }
   
}
