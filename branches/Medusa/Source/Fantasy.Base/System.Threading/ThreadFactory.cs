using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace System.Threading
{
    public class ThreadFactory
    {
        public static Thread CreateThread(ParameterizedThreadStart start)
        {
            Thread rs = new Thread(start);
            InitThread(rs);
            return rs;
        }

        public static Thread CreateThread(ThreadStart start)
        {
            Thread rs = new Thread(start);
            InitThread(rs);
            return rs;
        }


        private static void InitThread(Thread thread)
        {
            thread.CurrentCulture = Thread.CurrentThread.CurrentCulture;
            thread.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
            thread.Priority = ThreadPriority.Lowest;
            thread.IsBackground = true;
        }

        public static bool QueueUserWorkItem(WaitCallback callBack, Object state = null)
        {
            CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            CultureInfo uici = Thread.CurrentThread.CurrentUICulture;

            return ThreadPool.QueueUserWorkItem((o) => {
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                callBack(o);
            }, state); 

        }

    }
}
