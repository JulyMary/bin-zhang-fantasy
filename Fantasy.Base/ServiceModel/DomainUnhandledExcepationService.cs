using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Fantasy.ServiceModel
{
    public class DomainUnhandledExcepationService : ServiceBase
    {
        public DomainUnhandledExcepationService()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }


        

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            if (! IsKnownError(error))
            {
                if (this.Site != null)
                {
                    ILogger logger = this.Site.GetService<ILogger>();
                    if (logger != null)
                    {

                        if (error != null)
                        {
                            logger.LogError("Domain", error, "An unhandled CLR  exception is throwed by current domain.");
                        }
                        else
                        {
                            logger.LogError("Domain", "An unhandled none CLR  exception is throwed by current domain. exception object : {0}", e.ExceptionObject.ToString());
                        }

                        if (e.IsTerminating)
                        {
                            logger.LogError("Domain", "CLR is terminating.");
                        }
                    }
                }
            }
        }

        private bool IsKnownError(Exception error)
        {
            bool rs = false;
            if (error != null)
            {
                if (error is ThreadAbortException)
                {
                    rs = true;
                }
                else if(WCFExceptionHandler.CanCatch(error))
                {
                    rs = true;
                }
            }
            return rs;
        }
    }
}
