using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public interface IBusinessEngineContextProvider
    {
        BusinessEngineContext GetCurrent();

        void SetCurrent(BusinessEngineContext value);
    }
}
