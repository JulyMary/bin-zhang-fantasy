using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows;
using Fantasy.Studio.Services;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Build
{
    public class ImportProjectCommand : ObjectWithSite, ICommand
    {
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            IWorkbench wb = this.Site.GetRequiredService<IWorkbench>();
            var dirtyViews = wb.Views.OfType<IEditingViewContent>().Where(v => v.DirtyState == EditingState.Dirty);
            bool allowImport = !dirtyViews.Any();

            if (!allowImport)
            {
                string text = "\n" + string.Join("\n", dirtyViews.Select(v => v.Title));
                IMessageBoxService msgBox = this.Site.GetRequiredService<IMessageBoxService>();

                if (msgBox.Show(string.Format(Fantasy.Studio.BusinessEngine.Properties.Resources.ConfirmImportProjectMessage, text), button: MessageBoxButton.YesNo, defaultResult: MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {

                    allowImport = true;
                }

            }


            if (allowImport)
            {

                if (String.IsNullOrEmpty(Properties.Settings.Default.SolutionPath))
                {
                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.DefaultExt = ".sln"; // Default file extension
                    dlg.Filter = Properties.Resources.SolutionFileDialogFilter; // Filter files by extension
                    dlg.Title = Properties.Resources.SolutionFileDialogTitle;

                    dlg.AddExtension = true;
                    if ((bool)dlg.ShowDialog())
                    {
                        Properties.Settings.Default.SolutionPath = dlg.FileName;
                    }
                }

                if (!string.IsNullOrEmpty(Properties.Settings.Default.SolutionPath))
                {
                    IEntityService es = this.Site.GetRequiredService<IEntityService>();
                    es.BeginUpdate();
                    try
                    {
                        foreach (IEditingViewContent view in dirtyViews)
                        {
                            view.Save();
                        }
                        ProjectExporter exporter = new ProjectExporter() { Site = this.Site };
                        exporter.Run();
                    }
                    finally
                    {
                        es.EndUpdate(true);
                    }
                }
            }
        }

        #endregion
    }
}
