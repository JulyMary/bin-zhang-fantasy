using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessEngineContext : IServiceProvider
    {




        #region IServiceProvider Members

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        #endregion

        public BusinessUser User { get; set; }

        public BusinessApplication Application { get; set; }

        public static BusinessEngineContext Current { get; set; }
    }
}
