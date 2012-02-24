using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessWebView : BusinessScriptBase
    {
        public virtual BusinessWebController Controller
        {
            get
            {
                return (BusinessWebController)this.GetValue("Controller", null);
            }
            set
            {
                this.SetValue("Controller", value);
            }
        }


    }
}
