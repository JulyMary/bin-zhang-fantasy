using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessApplication : ObjectWithSite
    {
        public BusinessApplication(BusinessApplicationData data)
        {
            this.Data = data;
        }


        public BusinessApplicationData Data { get; private set; }
    }
}
