using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    [Serializable]
    public class InvalidJobTemplateException : Exception 
    {
        public InvalidJobTemplateException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public InvalidJobTemplateException(string message)
            : base(message)
        {

        }
    }
}
