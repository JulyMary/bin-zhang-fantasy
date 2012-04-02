using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Reflection;
using System.IO;
using System.Windows;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.IO;
using Fantasy.Studio.BusinessEngine.Properties;
using Fantasy.Reflection;

namespace Fantasy.Studio.BusinessEngine.AssemblyReference
{
    public class AddReferenceCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

      
        public object Execute(object args)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
           
            dlg.DefaultExt = ".dll"; // Default file extension
            dlg.Filter = Resources.AddReferenceDialogFilter; // Filter files by extension
            dlg.Title = Resources.AddReferenceDialogTitle;
            dlg.Multiselect = true;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                IEntityService es = this.Site.GetRequiredService<IEntityService>();
                foreach (string filename in dlg.FileNames)
                {

                    try
                    {
                           
                        es.AddBusinessAssemblyReference(filename);
                           
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message, Application.Current.MainWindow.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }


            return null;

        }



        #endregion
    }
}
