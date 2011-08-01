using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;
using System.Collections;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class IsSystemEntity : ConditionBase
    {
        public override bool IsValid(object caller)
        {

            bool rs = false;
            if (caller is IBusinessEntity && ((IBusinessEntity)caller).IsSystem )
            {
                rs = true;
            }
            else if (caller is IEnumerable)
            {
                foreach (object item in (IEnumerable)caller)
                {
                    if (item is IBusinessEntity && ((IBusinessEntity)item).IsSystem)
                    {
                        rs = true;
                    }
                }
            }
            //return ((IBusinessEntity)caller).IsSystem;
            return false;
        }
    }
}
