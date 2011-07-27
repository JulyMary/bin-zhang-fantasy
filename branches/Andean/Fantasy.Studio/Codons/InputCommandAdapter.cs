using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Studio.Codons
{
    public class InputCommandAdapter : IObjectWithSite, System.Windows.Input.ICommand 
    {
        public InputCommandAdapter(ICommand command)
        {
            this._command = command;
            
        }

        private ICommand _command;
        
        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return true;
        }

        event EventHandler System.Windows.Input.ICommand.CanExecuteChanged { add { } remove { } }

        public void Execute(object parameter)
        {
            this._command.Execute(parameter);
        }

        #endregion



        #region IObjectWithSite Members
        private IServiceProvider _site = null;
        public IServiceProvider Site
        {
            get
            {
                return _site;
            }
            set
            {
                _site = value;
                if (this._command is IObjectWithSite)
                {
                    ((IObjectWithSite)_command).Site = value;
                }
            }
        }

        #endregion
    }
}
