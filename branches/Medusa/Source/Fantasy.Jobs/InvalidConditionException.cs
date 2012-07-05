using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
 

    [Serializable]
    public class InvalidConditionException : Exception
    {
        public InvalidConditionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public InvalidConditionException(string message)
            : base(message)
        {

        }
    }
}
