using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Scheduling
{
    public interface  IScheduleService
    {
        long Register(DateTime timeToExecute, System.Action action);

        void Unregister(long cookie);
    }
}
