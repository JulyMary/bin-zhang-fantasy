using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Threading
{
    public class ThreadTaskScheduler : TaskScheduler
    {
        public ThreadTaskScheduler(int maximumConcurrencyLevel)
        {
            if (maximumConcurrencyLevel <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumConcurrencyLevel");
            }
            this._maximumConcurrencyLevel = maximumConcurrencyLevel;
        }

        private int _maximumConcurrencyLevel = -1;
        public override int MaximumConcurrencyLevel
        {
            get
            {
                return _maximumConcurrencyLevel;
            }
            
           
        }

        private object _syncRoot = new object();
        private List<Task> _scheduledTasks = new List<Task>();
        private List<Thread> _threads = new List<Thread>();
      
        private bool _aborting = false;

        public bool  AbortAll(bool waitForExit, TimeSpan timeout)
        {
            lock (_syncRoot)
            {
                _aborting = true;
                _scheduledTasks.Clear(); 
            }

            foreach (Thread thread in this._threads)
            {
                thread.Abort();
            }

            if (waitForExit)
            {
                Thread waitThread = ThreadFactory.CreateThread(() =>
                {
                    Thread[] threads;
                    lock (_syncRoot)
                    {
                        threads = this._threads.ToArray(); 
                    }

                    foreach (Thread thread in threads)
                    {
                        thread.Join(timeout);
                    }

                }).WithStart();
               
                return waitThread.Join(timeout);

            }
            else
            {
                return true;
            }
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_syncRoot)
            {
                return this._scheduledTasks.ToArray();
            }
        }

        protected override bool TryDequeue(Task task)
        {
            lock (_syncRoot)
            {
                bool rs = base.TryDequeue(task);
                if (rs)
                {
                    this._scheduledTasks.Remove(task);
                }
                return rs;
            }
        }

        protected override void QueueTask(Task task)
        {
            lock (_syncRoot)
            {
                if (!_aborting)
                {
                    if (this._threads.Count < this.MaximumConcurrencyLevel)
                    {
                        this.RunTask(task);
                    }
                    else
                    {
                        this._scheduledTasks.Add(task);
                    }
                }
                
            }
        }

        private void RunTask(Task task)
        {
            lock (_syncRoot)
            {
               
                Thread thread = ThreadFactory.CreateThread((currentTask) => 
                {
                    try
                    {
                        base.TryExecuteTask((Task)currentTask);
                    }
                    finally
                    {
                        lock (_syncRoot)
                        {
                            this._threads.Remove(Thread.CurrentThread);
                            if (! _aborting && this._scheduledTasks.Count > 0 && this._threads.Count < this.MaximumConcurrencyLevel)
                            {
                                Task next = this._scheduledTasks[0];
                                this._scheduledTasks.RemoveAt(0);
                                this.RunTask(next);
                            }
                        }
                    }
                });
                this._threads.Add(thread);
                
                thread.Start(task);
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }
    }
}
