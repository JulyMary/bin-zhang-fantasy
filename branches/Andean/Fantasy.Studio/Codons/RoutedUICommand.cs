using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.Studio.Codons
{
    public class RoutedUICommand : CodonBase
    {
        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            if (command == null)
            {
                command = new System.Windows.Input.RoutedUICommand(this.Text, this.Name, this.GetType()); 
            }

            return command;
        }

        private System.Windows.Input.RoutedUICommand command = null;

        public string Text { get; set; }

        public string Name { get; set; }


    }
}
