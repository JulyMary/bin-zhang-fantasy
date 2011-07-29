using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessDataType : BusinessEntity
    {
        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }

        public virtual string CodeName
        {
            get
            {
                return (string)this.GetValue("CodeName", null);
            }
            set
            {
                this.SetValue("CodeName", value);
            }
        }

        public virtual string DefaultDatabaseType
        {
            get
            {
                return (string)this.GetValue("DefaultDatabaseType", null);
            }
            set
            {
                this.SetValue("DefaultDatabaseType", value);
            }
        }

        public virtual int DefaultLength
        {
            get
            {
                return (int)this.GetValue("DefaultLength", 0);
            }
            set
            {
                this.SetValue("DefaultLength", value);
            }
        }

        public virtual int DefaultPrecision
        {
            get
            {
                return (int)this.GetValue("DefaultPrecision", 0);
            }
            set
            {
                
                this.SetValue("DefaultPrecision", value);
            }
        }


        public static class WellknownIds
        {
            public static readonly Guid Binary = new Guid("192d0683-3c32-44b2-bf6f-51cb88e98445");

            public static readonly Guid Boolean = new Guid("b9ceb0d0-ef32-44ff-8cdc-3b609a65d27f");

            public static readonly Guid Byte = new Guid("ac55ad0f-eb2e-4082-9404-ae145cf8ff15");

            public static readonly Guid DateTime = new Guid("a2883f92-473f-4a07-8d96-85e3fcf31588");

            public static readonly Guid DateTimeOffset = new Guid("5d09014d-c22c-4daa-9657-5c92a4081c36");

            public static readonly Guid Decimal = new Guid("abc026e2-a379-44a4-a59c-ebe636b00f11");

            public static readonly Guid Double = new Guid("5baf8a91-4204-420c-b888-8bd26e9289bf");

            public static readonly Guid Float = new Guid("adf5890d-3bbd-4375-ad11-4e0594c5e776");

            public static readonly Guid Guid = new Guid("a2b55595-6959-4ec7-8daf-23d2ba6b2c99");

            public static readonly Guid Int16 = new Guid("54be637f-8fe7-4872-a471-e8bd1c902995");

            public static readonly Guid Int32 = new Guid("44288a16-267e-4fda-a588-b82fba83bb13");

            public static readonly Guid Int64 = new Guid("d5b4d680-c2ec-4294-982d-b1bf3b89b81e");

            public static readonly Guid UInt16 = new Guid("1bd22188-8558-432f-9c8d-c01b748a2b65");

            public static readonly Guid UInt32 = new Guid("2d3b89de-6ced-4421-9216-d311ea4fbbee");

            public static readonly Guid UInt64 = new Guid("1d101c89-cd94-4203-a571-b6c3ddcff4ec");

            public static readonly Guid String = new Guid("1e12aa32-dd17-4f37-8d4c-5ffe9b53dfe1");

            public static readonly Guid Text = new Guid("e10218c6-1b0b-4269-a879-df0617f1e6e0");

            public static readonly Guid Bitmap = new Guid("0ff5fd67-2cad-48ed-bdc3-042b5dd321a1");

            public static readonly Guid Enum = new Guid("f1e72c1d-2432-4da6-82d6-aa2ddeda84ed");

            public static readonly Guid Class = new Guid("24473090-539e-4c13-be25-46e6f0dd9051");
        }

    }
}
