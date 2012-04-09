using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine
{
    public class BusinessUser : ObjectWithSite
    {
       
        public string Name
        {
            get;
            internal set;
        }

        public Guid Id
        {
            get;
            internal set;
        }

        public virtual void OnSignIn()
        {

        }

        public virtual void OnSignOut()
        {

        }
    }
}
