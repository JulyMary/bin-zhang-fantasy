using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Threading
{
    public static class ThreadExtension
    {
        public static Thread WithStart(this Thread thread)
        {
            thread.Start();
            return thread;
        }

        public static Thread WithStart(this Thread thread, object state)
        {
            thread.Start(state);
            return thread;
        }
    }
}
