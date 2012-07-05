using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Solar
{
    public interface IJobStartupFilter
    {
        IEnumerable<JobStartupData> Filter(IEnumerable<JobStartupData> source);
    }
}
