using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Jobs.Management
{
    public interface IJobStartupFilter
    {
        IEnumerable<JobMetaData> Filter(IEnumerable<JobMetaData> source);
    }
}

   
