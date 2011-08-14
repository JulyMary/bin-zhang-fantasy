using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.AddIns.Conditions
{
    public class IsTypeOf : ConditionBase
    {
        public Type Type { get; set; }
        public override bool IsValid(object args, IServiceProvider services)
        {
            return Type.IsInstanceOfType(args);
        }
    }
}
