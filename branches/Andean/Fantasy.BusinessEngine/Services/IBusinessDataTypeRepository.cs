using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessDataTypeRepository
    {
        BusinessDataType[] All{get;}

        BusinessDataType Enum { get; }

        BusinessDataType Class { get; }

        //BusinessDataType Int { get; }

        //BusinessDataType String { get; }

        //BusinessDataType Text { get; }



    }
}
