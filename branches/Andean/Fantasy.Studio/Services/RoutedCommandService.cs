using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Windows.Input;
using Fantasy.AddIns;

namespace Fantasy.Studio.Services
{
    public class RoutedCommandService : ServiceBase, Fantasy.Studio.Services.IRoutedCommandService
    {
        public System.Windows.Input.ICommand FindCommand(string name)
        {
            return _commands.GetValueOrDefault(name);
        }

        public override void InitializeService()
        {

            foreach (RoutedCommand command in AddInTree.Tree.GetTreeNode("fantasy/studio/routedcommands").BuildChildItems(this, this.Site))
            {
                this._commands.Add(command.Name, command); 
            }

            base.InitializeService();
        }


        private Dictionary<string, System.Windows.Input.ICommand> _commands = new Dictionary<string, System.Windows.Input.ICommand>(StringComparer.OrdinalIgnoreCase); 

    }
}
