using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns
{
    public class ConditionCollection : List<ICondition>
    {
        public ConditionCollection()
        {

        }

        public ConditionCollection(ConditionCollection conditions) : base(conditions)
        {

        }

        public ConditionFailedAction GetCurrentConditionFailedAction(object caller)
        {
            ConditionFailedAction action = ConditionFailedAction.Nothing;
            foreach (ICondition condition in this)
            {
                if (!condition.IsValid(caller))
                {
                    switch (condition.Action)
                    {
                        case ConditionFailedAction.Nothing:
                            break;
                        case ConditionFailedAction.Disable:
                            action = ConditionFailedAction.Disable;
                            break;
                        case ConditionFailedAction.Exclude:
                            return ConditionFailedAction.Exclude;

                    }

                }
            }

            return action;
        }
    }
}
