using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Fantasy.Jobs.Management
{
    public interface IJobDispatcher
    {
        void Start();

        void TryDispatch();

       
    }


    public class StartDispatchingCommand : ICommand, IObjectWithSite
    {
        
        public IServiceProvider Site
        {
            get;
            set;
        }


        public object Execute(object args)
        {
            IJobDispatcher disp = (IJobDispatcher)this.Site.GetService(typeof(IJobDispatcher));
            if (disp != null)
            {
                Task.Factory.StartNew(() =>
                {
                  
                    disp.Start();
                });
            }


            return null;

        }

       
    }

}
