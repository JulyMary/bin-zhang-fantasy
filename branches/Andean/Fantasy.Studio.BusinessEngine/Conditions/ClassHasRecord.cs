using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.AddIns;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.Conditions
{
    public class ClassHasRecord : ConditionBase
    {
        public override bool IsValid(object args, IServiceProvider services)
        {
            BusinessClass cls = (BusinessClass)args;
            IDDLService ddl = services.GetRequiredService<IDDLService>();

            return ddl.GetRecordCount(cls) >0;

        }
    }
}
