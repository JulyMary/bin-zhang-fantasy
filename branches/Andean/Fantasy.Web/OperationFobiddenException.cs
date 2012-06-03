using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web
{
    public class OperationFobiddenException : Exception
    {
        public OperationFobiddenException()
        {

        }

        public OperationFobiddenException(string message)
            : base(message)
        {

        }

        public OperationFobiddenException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}