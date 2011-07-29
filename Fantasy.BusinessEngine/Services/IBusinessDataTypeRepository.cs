using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface IBusinessDataTypeRepository
    {
        BusinessDataType[] All{get;}

        BusinessDataType Binary { get; }

        BusinessDataType Boolean { get; }

        BusinessDataType Byte { get; }

        BusinessDataType DateTime { get; }

        BusinessDataType DateTimeOffset { get; }

        BusinessDataType Decimal { get; }

        BusinessDataType Double { get; }

        BusinessDataType Float { get; }

        BusinessDataType Guid { get; }

        BusinessDataType Int16 { get; }

        BusinessDataType Int32 { get; }

        BusinessDataType Int64 { get; }

        BusinessDataType UInt16 { get; }

        BusinessDataType UInt32 { get; }

        BusinessDataType UInt64 { get; }

        BusinessDataType String { get; }

        BusinessDataType Text { get; }

        BusinessDataType Bitmap { get; }

        BusinessDataType Enum { get; }

        BusinessDataType Class { get; }

    }
}
