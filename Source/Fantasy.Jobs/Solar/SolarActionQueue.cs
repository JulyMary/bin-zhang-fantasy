using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.Threading;

namespace Fantasy.Jobs.Solar
{
    public class SolarActionQueue : AbstractService, ISolarActionQueue
    {

        private List<Action<ISolar>> _queue = new List<Action<ISolar>>();

        private Thread _retryThread;

        public override void InitializeService()
        {
            _retryThread = ThreadFactory.CreateThread(Refresh).WithStart();
            
            base.InitializeService();
        }


        private void Refresh()
        {
            while (true)
            {
                Thread.Sleep(15 * 1000);
                List<Action<ISolar>> list;
                lock (_queue)
                {
                    list = new List<Action<ISolar>>(_queue);
                }
                try
                {
                    using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
                    {
                        foreach (Action<ISolar> action in list)
                        {
                            action(client.Client);
                            lock (_queue)
                            {
                                _queue.Remove(action);
                            }
                        }
                    }
                }
                catch
                {
                }

               
            }
        }

        public override void UninitializeService()
        {
            _retryThread.Abort();
            _retryThread.Join();
            base.UninitializeService();
        }


        public void Enqueue(Action<ISolar> action)
        {
            try
            {
                using (ClientRef<ISolar> client = ClientFactory.Create<ISolar>())
                {
                    action(client.Client);
                }
            }
            catch(Exception error)
            {
                if (WCFExceptionHandler.CanCatch(error))
                {
                    lock (_queue)
                    {
                        _queue.Add(action);
                    }
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
