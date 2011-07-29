using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.Adaption;

namespace Fantasy.Studio.Codons
{
    public class CommandBinding : CodonBase
    {
        public CommandBinding()
        {
            this.ParameterSource = ParameterSource.Parameter;
        }

        public System.Windows.Input.ICommand Command { get; set; }

        private ObjectBuilder _handlerBuilder = null;
        [Template("_handlerBuilder")]
        public object Handler { get; set; }

        public override object BuildItem(object owner, System.Collections.IEnumerable subItems, ConditionCollection condition, IServiceProvider services)
        {
            AddInCommandBinding rs = new AddInCommandBinding() { Command = this.Command, Condition = condition, Owner = owner, ParameterSource = ParameterSource, ParameterMember=this.ParameterMember};
            rs.Handler = (System.Windows.Input.ICommand)this.Handler; 
            if (this._handlerBuilder != null)
            {
                rs.Handler = services.GetRequiredService<IAdapterManager>().GetAdapter<System.Windows.Input.ICommand>(this._handlerBuilder.Build<object>());
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

        public ParameterSource ParameterSource { get; set; }

        public string ParameterMember { get; set; }
      


    }
}
