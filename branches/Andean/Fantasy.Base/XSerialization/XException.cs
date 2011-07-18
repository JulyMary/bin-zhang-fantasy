using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.XSerialization
{
    [Serializable ]
    public class XException : Exception 
    {
        public XException(string message)
            : base(message)
        {

        }

        public XException()
        {

        }

        public XException(string message, Exception innerException)
            : base(message, innerException)

        {

        }
    }
}
