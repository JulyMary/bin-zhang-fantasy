using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public class WellknownUsers
    {
        public BusinessUser Administrator { get; internal set; }

        public BusinessUser Guest { get; internal set; }
    }
}
