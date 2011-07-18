using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public interface  INamedBusinessEntity
    {
        string Name { get; }

        string CodeName { get; }

        string FullName { get; }

        string FullCodeName { get; }
    }
}
