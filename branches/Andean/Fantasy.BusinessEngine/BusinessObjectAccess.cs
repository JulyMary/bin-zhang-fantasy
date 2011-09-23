using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    [Flags]
    public enum BusinessObjectAccess
    {
        None = 0x00,
        Read = 0x01,
        Write = 0x02,
        Create = 0x04,
        Delete = 0x08,
        All = unchecked((int)0xFFFFFFFF)
    }
}
