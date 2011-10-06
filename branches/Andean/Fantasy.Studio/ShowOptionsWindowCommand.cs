using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Fantasy.Studio
{
    public class ShowOptionsWindowCommand : ICommand
    {
      

        public bool CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler ICommand.CanExecuteChanged {add {} remove{}}

        public void Execute(object parameter)
        {
            OptionsWindow window = new OptionsWindow();
            window.ShowDialog();
        }

        
    }
}
