using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class IsSystemEntity : ConditionBase
    {
        public override bool IsValid(object caller)
        {
            //return ((IBusinessEntity)caller).IsSystem;
            return false;
        }
    }
}
