using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplicationParticipant : BusinessEntity
    {

        public virtual BusinessClass Class
        {
            get
            {
                return (BusinessClass)this.GetValue("Class", null);
            }
            set
            {
                this.SetValue("Class", value);
            }
        }

        public virtual bool IsEntry
        {
            get
            {
                return (bool)this.GetValue("IsEntry", false);
            }
            set
            {
                this.SetValue("IsEntry", value);
            }
        }



    }
}
