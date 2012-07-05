using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Scheduling
{
    public enum InstancesPolicy
    {
        Parallel,
        Queue,
        IgnoreNew,
        StopExisting
    }
}
