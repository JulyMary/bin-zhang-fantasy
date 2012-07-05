using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs
{
    [Serializable]
    public class TaskFailedException : Exception
    {
       public TaskFailedException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

       public TaskFailedException(string message)
            : base(message)
        {

        }
    }
}
