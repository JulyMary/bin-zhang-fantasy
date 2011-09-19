using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public interface IGernateCodeBusinessEntity : INamedBusinessEntity
    {
        string CodeName { get; }

        string FullCodeName { get; }
    }
}
