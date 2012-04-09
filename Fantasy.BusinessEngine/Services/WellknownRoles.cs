using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public class WellknownRoles
    {
        public BusinessRole Everyone { get; internal set; }

        public BusinessRole Users { get; internal set; }

        public BusinessRole Administrators { get; internal set; }
    }
}
