using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessWebScript : BusinessScriptBase
    {
        public virtual BusinessWebFolder WebFolder
        {
            get
            {
                return (BusinessWebFolder)this.GetValue("WebFolder", null);
            }
            set
            {
                this.SetValue("WebFolder", value);
            }
        }
    }
}
