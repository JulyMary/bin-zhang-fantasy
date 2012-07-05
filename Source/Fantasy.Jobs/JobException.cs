using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Fantasy.Jobs
{
    [Serializable]
    public class JobException : Exception
    {

        protected JobException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public JobException()
        {

        }
        public JobException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        public JobException(string message)
            : base(message)
        {

        }
    }
}
