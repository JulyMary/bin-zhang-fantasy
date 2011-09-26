using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;


namespace Fantasy.Studio.BusinessEngine.ClassDiagramEditing
{
    public class ClassDiagramModeCommand : ObjectWithSite, ICommand
    {
       

        public ClassDiagramMode Mode { get; set; }

    

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged {add{} remove{}}

        void ICommand.Execute(object parameter)
        {
            IWorkbench wb = this.Site.GetRequiredService<IWorkbench>();
            ClassDiagramPanel panel = (ClassDiagramPanel)((IDocumentEditingPanelContainer)wb.ActiveWorkbenchWindow.ViewContent).ActivePanel;
            panel.Mode = this.Mode;
           
        }

       
    }
}
