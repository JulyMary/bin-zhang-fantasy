using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Windows.Input;

namespace Fantasy.Studio.Services
{
    public class RoutedCommandService : ServiceBase
    {
        public System.Windows.Input.ICommand FindCommand(string name)
        {
            return _commands.GetValueOrDefault(name);
        }


        private Dictionary<string, System.Windows.Input.ICommand> _commands = new Dictionary<string, System.Windows.Input.ICommand>(StringComparer.OrdinalIgnoreCase); 

    }
}
