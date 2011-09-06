using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class IsExternalEnum : ConditionBase
    {
        public override bool IsValid(object caller, IServiceProvider services)
        {
            bool rs = false;
            if (caller is BusinessEnum && ((BusinessEnum)caller).IsExternal )
            {
                rs = true;
            }

            return rs;
        }
    }
}
