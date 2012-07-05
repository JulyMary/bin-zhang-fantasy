using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Fantasy.ServiceModel;

namespace Fantasy.ServiceModel
{
    public class LogDebugService : AbstractService, ILogListener
    {
        public override void InitializeService()
        {
            ILogger logger = (ILogger)((IServiceProvider)this.Site).GetService(typeof(ILogger));
            logger.AddListener(this);
            base.InitializeService();
        }

        public override void UninitializeService()
        {
            base.UninitializeService();
            ILogger logger = (ILogger)((IServiceProvider)this.Site).GetService(typeof(ILogger));
            logger.RemoveListener(this);
        }

        #region IJobLoggerListener Members

        //public void OnStatusChanged(string status)
        //{
        //    Debug.WriteLine("Status: " + status); 
        //}

        public void OnMessage(string category, MessageImportance importance, string message)
        {
            if (category != "Trace")
            {
                Debug.WriteLine("Message: " + message);
            }
        }

        public void OnWaring(string category, Exception exception, MessageImportance importance, string message)
        {
            if (category != "Trace")
            {
                Debug.WriteLine("Warning: " + message);
            }
        }

        public void OnError(string category, Exception exception, string message)
        {
            if (category != "Trace")
            {
                Debug.WriteLine("Error: " + message);
            }
        }

        #endregion
    }
}
