using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Scheduling
{
    public class ScheduleService : AbstractService , IScheduleService
    {

        public override void InitializeService()
        {

            this._logger = this.Site.GetService<ILogger>(); 
            _workTread = ThreadFactory.CreateThread(this.Scheduling).WithStart();
           
          

            base.InitializeService();
        }

        private ILogger _logger;

        public override void UninitializeService()
        {
            base.UninitializeService();
        }

       

        private void Scheduling()
        {

            ThreadTaskScheduler taskScheduler = new ThreadTaskScheduler(20);
            TaskFactory taskfactory = new TaskFactory(taskScheduler);
            try
            {
                do
                {

                    ScheduledAction action;

                    do
                    {
                        action = null;
                        lock (_syncRoot)
                        {
                            if (_queue.Count > 0 && _queue[0].TimeToExecute <= DateTime.Now)
                            {
                                action = _queue[0];
                                _queue.RemoveAt(0);
                            }
                        }
                        if (action != null)
                        {
                            taskfactory.StartNew((a) =>
                            {
                                try
                                {
                                    ((ScheduledAction)a).Action();
                                }
                                catch (ThreadAbortException)
                                {
                                }
                                catch (Exception err)
                                {
                                    if (_logger != null)
                                    {
                                        _logger.LogError("Schedule", err, "An exception was thrown when executed a scheduled action.");
                                    }
                                }
                            }, action);
                        }

                    } while (action != null);


                    Thread.Sleep(1000);

                } while (true);
            }
            finally
            {
                taskScheduler.AbortAll(false, TimeSpan.Zero);
            }
        }

        public long Register(DateTime timeToExecute, System.Action action)
        {

            lock (_syncRoot)
            {
                ScheduledAction item = new ScheduledAction()
                {
                    Cookie = _seed ++,
                    TimeToExecute = timeToExecute,
                    Action = action
                };

                int pos = _queue.BinarySearchBy(timeToExecute, i => i.TimeToExecute);
                if (pos < 0)
                {
                    pos = ~pos;
                }
                _queue.Insert(pos, item);
               
                return item.Cookie;
               
            }
        }

        public void Unregister(long cookie)
        {
            lock (_syncRoot)
            {
                ScheduledAction item = _queue.Find(i => i.Cookie == cookie);
                if (item != null)
                {
                    _queue.Remove(item);
                   
                }
            }
        }

        private Thread _workTread;
       // private AutoResetEvent _waitHandle = new AutoResetEvent(false);
        private long _seed = 0;

        private List<ScheduledAction> _queue = new List<ScheduledAction>();

        private object _syncRoot = new object();


        private class ScheduledAction
        {
            public long Cookie;
            public DateTime TimeToExecute;
            public System.Action Action;
        }

       
    }
}
