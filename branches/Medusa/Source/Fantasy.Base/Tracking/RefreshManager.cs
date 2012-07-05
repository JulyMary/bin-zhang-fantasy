using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fantasy.Tracking
{
    internal static class RefreshManager
    {

        private static List<WeakReference> _list = new List<WeakReference>();

        static RefreshManager()
        {
            _refreshThread =  ThreadFactory.CreateThread(new ThreadStart(Run)).WithStart();
           
            
        }

        static void Run()
        {
            do
            {
                List<WeakReference> list;
                lock (typeof(RefreshManager))
                {
                    list = new List<WeakReference>(_list);
                }

                Parallel.ForEach(list, weak =>
                {
                    if (weak.IsAlive)
                    {
                        ((IRefreshable)weak.Target).Refresh();
                    }
                    else
                    {
                        lock (_list)
                        {
                            _list.Remove(weak);
                        }
                    }
                        
                }
                );

                Thread.Sleep(5 * 1000);
               
            } while (true);
        }

        static Thread _refreshThread;

        internal static void Register(IRefreshable obj)
        {
            lock (typeof(RefreshManager))
            {
                _list.Add(new WeakReference(obj));
            }
        }

        internal static void Unregister(IRefreshable obj)
        {
            lock (typeof(RefreshManager))
            {
                WeakReference weak  = (from w in _list where w.Target == obj select w).SingleOrDefault() ;
                if (weak != null)
                {
                    _list.Remove(weak); 
                }
            }
        }

    }
}
