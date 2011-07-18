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
    }
}
