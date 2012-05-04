using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Properties;

namespace Fantasy.BusinessEngine
{
    public class PropertyNotFoundException : Exception
    {
        public PropertyNotFoundException(BusinessClass @class, string property)
            : base(String.Format(Resources.PropertyNotFoundMessage, property, @class.FullName)) 
        {
            this.Property = property;
            this.Class = @class;
        }

        public string Property { get; private set; }

        public BusinessClass Class { get; private set; }
    }
}
