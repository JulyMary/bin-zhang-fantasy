using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Threading;
using System.Reflection;

namespace Fantasy.ServiceModel
{
    public abstract class TextLogService : AbstractService, ILogListener
    {

        protected abstract StreamWriter GetWriter();

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

        #region ILogListener Members

        public void OnMessage(string category, MessageImportance importance, string message)
        {
            this.GetWriter().WriteLine(string.Format("[{0}] Message : {1}", DateTime.Now,  message));
        }

        public void OnWaring(string category, Exception exception, MessageImportance importance, string message)
        {
            this.GetWriter().WriteLine(string.Format("[{0}] Warning : {1}", DateTime.Now, message));
            if (exception != null)
            {
                this.WriteException(exception, 0);
            }
        }

        private void WriteException(Exception exception, int indent)
        {
            WriteIndent(indent);
            this.GetWriter().WriteLine("Type: " + exception.GetType().ToString());
            WriteIndent(indent);
            this.GetWriter().WriteLine("Message: " + exception.Message);
            WriteIndent(indent);
            this.GetWriter().WriteLine("Source: " + exception.Source);
            WriteIndent(indent);
            this.GetWriter().WriteLine("TargetSite: " + exception.TargetSite);
            WriteIndent(indent);
            this.GetWriter().WriteLine("StackTrace: " + exception.StackTrace);
            WriteIndent(indent);
            this.GetWriter().WriteLine("HelpLink: " + exception.HelpLink);

            var query = from prop in exception.GetType().GetProperties()
                        where prop.DeclaringType != typeof(Exception) && prop.GetIndexParameters().Length == 0
                        select prop;

            foreach (PropertyInfo pi in query)
            {
                object value = pi.GetValue(exception, null);
                WriteIndent(indent);
                this.GetWriter().WriteLine(String.Format("{0} : {1}", pi.Name, value ?? "null")); 
            }

            if (exception.InnerException != null)
            {
                WriteIndent(indent);
                this.GetWriter().WriteLine("InnerException");
                this.WriteException(exception.InnerException, indent + 1);
            }


        }

        private void WriteIndent(int indent)
        {
            for (int i = 0; i < indent; i++)
            {
                GetWriter().Write("  ");
            }
        }

        public void OnError(string category, Exception exception, string message)
        {
            this.GetWriter().WriteLine(string.Format("[{0}] Error : {1}", DateTime.Now, message));
            if (exception != null)
            {
                this.WriteException(exception, 0);
            }
        }

        #endregion
    }

}
