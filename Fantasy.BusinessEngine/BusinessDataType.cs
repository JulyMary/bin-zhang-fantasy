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


        public virtual bool IsEnum
        {
            get
            {
                return this.Id == EnumId;
            }
        }

        public virtual bool IsBusinessClass
        {
            get
            {
                return this.Id == BusinessClassId; 
            }
        }

        public static readonly Guid EnumId = new Guid("f1e72c1d-2432-4da6-82d6-aa2ddeda84ed");

        public static readonly Guid BusinessClassId = new Guid("24473090-539e-4c13-be25-46e6f0dd9051");
    }
}
