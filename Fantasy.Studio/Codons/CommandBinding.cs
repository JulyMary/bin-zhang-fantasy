using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;

namespace Fantasy.Studio.Codons
{
    public class CommandBinding : CodonBase
    {

        public System.Windows.Input.ICommand Command { get; set; }

        public Type Handler { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            return new AddInCommandBinding() { Command = Command, Condition = condition, Owner = owner, Handler = Activator.CreateInstance (Handler) };
        }

        public override bool HandleCondition
        {
            get
            {
                return true;
            }
        }
    }
}
