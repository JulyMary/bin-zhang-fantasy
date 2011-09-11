using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine;
using Fantasy.ServiceModel;
using Fantasy.Studio.Services;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.PackageEditing
{
    public class AddPackageCommand : System.Windows.Input.ICommand
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
            IEntityService es = ServiceManager.Services.GetRequiredService<IEntityService>();
            BusinessPackage child = es.AddBusinessPackage(parent);

            //BusinessClassDiagram diagram = es.CreateEntity<BusinessClassDiagram>();
            //diagram.Name = Resources.DefaultNewBusinessClassDiagramName;
            //child.ClassDiagrams.Add(diagram);
            //diagram.Package = child;

            IEditingService documentService = ServiceManager.Services.GetRequiredService<IEditingService>();
            documentService.OpenView(child); 
        }

        #endregion
    }
}
