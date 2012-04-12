using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Security
{
    public class SecurityArgs
    {
        public BusinessApplication Application { get; set; }

        public BusinessUser User { get; set; }       
    }

    public class ClassSecurityArgs : SecurityArgs
    {
        public BusinessClass Class { get; set; }
    }

    public class ObjectSecurityArgs : SecurityArgs
    {
        public BusinessObject Object { get; set; }
    }
}
