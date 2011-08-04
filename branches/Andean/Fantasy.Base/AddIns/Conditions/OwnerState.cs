using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Fantasy.AddIns.Conditions
{
    public class OwnerState : ConditionBase
    {
        public OwnerState()
        {
            this.StateMember = "State";
        }
        public string State { get; set; }

        

        public override bool IsValid(object owner)
        {
            object currentState = Invoker.Invoke(owner, StateMember);
            Type t = currentState.GetType();
            object state = Enum.Parse(t, this.State, true); 
            if (t.IsDefined(typeof(FlagsAttribute), false))
            {
                return (Convert.ToInt64(state) & Convert.ToInt64(currentState)) == Convert.ToInt64(state);
            }
            else
            {
                return object.Equals(state, currentState); 
            }
        }

        public string StateMember { get; set; }

        
    }
}
