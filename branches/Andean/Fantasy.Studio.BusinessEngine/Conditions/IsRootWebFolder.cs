using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class IsRootWebFolder : ConditionBase
    {
        public override bool IsValid(object args, IServiceProvider services)
        {
            BusinessWebFolder folder = (BusinessWebFolder)args;
            return folder.Package != null;
        }
    }
}
