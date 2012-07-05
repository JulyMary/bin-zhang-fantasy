using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Fantasy.Jobs.Management
{
  
    public class CpuLoadFactorEvaluator : ObjectWithSite, IComputerLoadFactorEvaluator
    {

        private Thread _refreshThread;
        PerformanceCounter _cpuCounter;
        public CpuLoadFactorEvaluator()
        {
            _cpuCounter = new PerformanceCounter() { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };
            _cpuCounter.NextValue();
            double idle = (100.0 - _cpuCounter.NextValue()) / 100.0;
            for (int i = 0; i < 10; i++)
            {
                _idle[i] = idle;
            }
            _refreshThread = ThreadFactory.CreateThread(Refresh).WithStart();
           
        }

        private double[] _idle = new double[10]; 

        private void Refresh()
        {
            

            int n = 0;
            while (true)
            {
                Thread.Sleep(1 * 1000);
                _idle[n] = (100 - _cpuCounter.NextValue()) / 100;
                n++;
                n %= 10;
            }
        }


        #region IComputerLoadFactorEvaluator Members

        public double Evaluate()
        {
            IJobController controller = this.Site.GetService<IJobController>();
            return _idle.Sum() / 10 * controller.GetAvailableProcessCount();
        }

        #endregion
    }
}
