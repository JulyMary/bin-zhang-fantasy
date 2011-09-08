using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Studio.Services;


namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class ImportEnumCommand : ObjectWithSite, System.Windows.Input.ICommand
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
            using(ImportEnumDialogModel model = new ImportEnumDialogModel(this.Site))
            {
            
                ImportEnumDialog dialog = new ImportEnumDialog();
                dialog.DataContext = model;

                if (dialog.ShowDialog() == true)
                {
                    Type enumType = model.SelectedEnum.Type;

                    BusinessPackage parent = (BusinessPackage)parameter;
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    BusinessEnum @enum = es.AddExternalBusinessEnum(parent, enumType);

                    IEditingService documentService = this.Site.GetRequiredService<IEditingService>();
                    IViewContent content = documentService.OpenView(@enum);
                    if (content != null)
                    {
                        content.WorkbenchWindow.Select();
                    }
                }
            }
        
        }

        #endregion
    }
}
