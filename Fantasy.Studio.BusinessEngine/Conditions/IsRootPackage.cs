using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class IsRootPackage : ConditionBase
    {
        public override bool IsValid(object caller, IServiceProvider services)
        {
            bool rs = false;
            if (caller is BusinessPackage && ((BusinessPackage)caller).Id == BusinessPackage.RootPackageId)
            {
                rs = true;
            }

            return rs;
        }
    }
}
