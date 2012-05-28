using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine;

namespace Fantasy.Web
{
    public class CreateArgs
    {

        public CreateArgs(BusinessClass @class, BusinessObject parent, string roleName)
        {
            this.BusinessClass = @class;
            this.Parent = parent;
            this.RoleName = roleName;
        }

        public BusinessClass BusinessClass { get; private set; }

        public BusinessObject Parent { get; private set; }

        public string RoleName { get; private set; }

        public BusinessObject NewInstance { get; set; }

        public BusinessObject CreateInstance()
        {
            throw new NotImplementedException();
        }
    }
}
