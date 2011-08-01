using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.BusinessEngine.Services
{
    public interface ISequenceService
    {
        int Next(string name);

        long LongNext(string name);

        int Current(string name);

        long LongCurrent(string name);
    }
}
