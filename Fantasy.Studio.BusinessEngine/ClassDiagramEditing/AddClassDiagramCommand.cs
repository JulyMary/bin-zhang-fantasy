using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Studio.Services;

namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class AddClassDiagramCommand: ObjectWithSite, System.Windows.Input.ICommand
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
            BusinessClassDiagram diagram = es.CreateEntity<BusinessClassDiagram>();
           
            diagram.Name = UniqueNameGenerator.GetName(Resources.DefaultNewBusinessClassDiagramName, parent.ClassDiagrams.Select(c=>c.Name));
            parent.ClassDiagrams.Add(diagram);
            diagram.Package = parent;
            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();
            documentService.OpenView(diagram); 
        }

        #endregion
    }
   
}
