using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;


namespace Fantasy.Studio.BusinessEngine
{
    public class EditPackageCommand : ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }



        public void Execute(object parameter)
        {
            BusinessPackage package = (BusinessPackage)parameter;
            IEditingService documentService = Fantasy.ServiceModel.ServiceManager.Services.GetRequiredService<IEditingService>();
           
            documentService.OpenView(package); 
        }

        #endregion
    }
}
