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

        private ObjectBuilder _handlerBuilder = null;
        [Template("_handlerBuilder")]
        public object Handler { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition)
        {
            AddInCommandBinding rs = new AddInCommandBinding() { Command = Command, Condition = condition, Owner = owner};
            if (this._handlerBuilder != null)
            {
                rs.Handler = this._handlerBuilder.Build<object>();
            }

            return rs;
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
