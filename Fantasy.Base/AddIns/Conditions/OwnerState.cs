using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns.Conditions
{
    public class OwnerState : ConditionBase
    {
        public string State { get; set; }


        public override bool IsValid(object args)
        {
            IStateObject owner = (IStateObject)args;
            Type t = owner.State.GetType();
            object state = Enum.Parse(t, this.State, true); 
            if (t.IsDefined(typeof(FlagsAttribute), false))
            {
                return (Convert.ToInt64(state) & Convert.ToInt64(owner.State)) == Convert.ToInt64(state);
            }
            else
            {
                return object.Equals(state, owner.State); 
            }
        }
    }
}
