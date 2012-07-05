using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    [Serializable]
    public class InvalidJobStartInfoException : Exception 
    {
        public InvalidJobStartInfoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public InvalidJobStartInfoException(string message)
            : base(message)
        {

        }
    }
}
