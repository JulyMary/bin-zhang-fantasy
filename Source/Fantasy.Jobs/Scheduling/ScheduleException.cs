using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Scheduling
{
    [Serializable]
    public class ScheduleException : Exception 
    {
        public ScheduleException(string message):base(message)
        {

        }
    }
}
