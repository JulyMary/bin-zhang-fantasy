using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fantasy.Tracking
{
    interface IRefreshable : IDisposable 
    {
        void Refresh();
    }
}
