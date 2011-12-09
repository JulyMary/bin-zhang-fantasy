using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessScriptBase : BusinessEntity
    {
        public virtual string Script
        {
            get
            {
                return (string)this.GetValue("Script", null);
            }
            set
            {
                this.SetValue("Script", value);
            }
        }

        public virtual string BuildAction
        {
            get
            {
                return (string)this.GetValue("BuildAction", null);
            }
            set
            {
                this.SetValue("BuildAction", value);
            }
        }

        public virtual string MetaData
        {
            get
            {
                return (string)this.GetValue("MetaData", null);
            }
            set
            {
                this.SetValue("MetaData", value);
            }
        }

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
    }
}
