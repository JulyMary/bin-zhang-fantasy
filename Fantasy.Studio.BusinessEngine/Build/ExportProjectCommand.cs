using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using System.Diagnostics;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ExportProjectCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged{add {} remove{}}

        public void Execute(object parameter)
        {
            if (String.IsNullOrEmpty(Properties.Settings.Default.SolutionPath))
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".sln"; // Default file extension
                dlg.Filter = Properties.Resources.SolutionFileDialogFilter; // Filter files by extension
                dlg.Title = Properties.Resources.SolutionFileDialogTitle;
              
                dlg.AddExtension = true;
                if ((bool)dlg.ShowDialog())
                {
                    Properties.Settings.Default.SolutionPath = dlg.FileName;
                }
            }

            string slnPath = Properties.Settings.ExtractToFullPath(Properties.Settings.Default.SolutionPath);

            if (!string.IsNullOrEmpty(slnPath))
            {
                ProjectExporter exporter = new ProjectExporter() { Site = this.Site };
                exporter.Run();
            }

            EnvDTE.DTE dte = Msdev.GetIDEInstance(slnPath);
            if (dte != null)
            {
                Msdev.ShowIDE(dte);
            }
            else
            {
                Process.Start(slnPath);
            }
        }

        #endregion
    }
}
