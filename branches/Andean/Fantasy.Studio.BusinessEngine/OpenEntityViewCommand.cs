﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Fantasy.BusinessEngine;
using Fantasy.Studio.Services;


namespace Fantasy.Studio.BusinessEngine
{
    public class OpenEntityViewCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }



        public void Execute(object parameter)
        {
            
            IEditingService documentService = this.Site.GetRequiredService<IEditingService>();

            IViewContent content = documentService.OpenView(parameter);
            if (content != null)
            {
                content.WorkbenchWindow.Select();
            }  
            
        }

        #endregion
    }
}
