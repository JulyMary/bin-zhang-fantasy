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
    public abstract class LogFileService : ServiceBase, ILogListener 
    {

        private XmlDocument _helperDocument = new XmlDocument();

        private const string _timePattern = "u";

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

        #region IJobLoggerListener Members


        private object _syncRoot = new object();
        protected virtual void Log(XmlElement element)
        {
            lock (this._syncRoot)
            {
                StreamWriter writer = this.GetWriter();
                writer.WriteLine(element.OuterXml);
                writer.Flush(); 
            }
        }

        public void OnMessage(string category, MessageImportance importance, string message)
        {
            XmlElement element = this._helperDocument.CreateElement("message");
            element.SetAttribute("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttribute("category", category);
            element.SetAttribute("importance", importance.ToString());
            element.SetAttribute("text", message);
            this.Log(element);
        }


        protected virtual void WriteStart()
        {
            XmlElement element = this._helperDocument.CreateElement("start");
            element.SetAttribute("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttribute("category", "Start");
            element.SetAttribute("importance", MessageImportance.High.ToString());
            element.SetAttribute("text", String.Format("Start. Culture : {0}; UI Culture: {1}.", Thread.CurrentThread.CurrentCulture, Thread.CurrentThread.CurrentUICulture) );
            this.Log(element);
        }

        public void OnWaring(string category, Exception exception, MessageImportance importance, string message)
        {
            XmlElement element = this._helperDocument.CreateElement("warning");
            element.SetAttribute("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttribute("category", category);
            element.SetAttribute("importance", importance.ToString());
            element.SetAttribute("text", message);
            if(exception != null)
            {
                element.AppendChild(this.CreateExceptionElement(exception));
            }

            this.Log(element);
        }

        private XmlElement CreateExceptionElement(Exception exception)
        {
            XmlElement element = this._helperDocument.CreateElement("exception");
            element.SetAttribute("type", exception.GetType().ToString()); 
            element.SetAttribute("message", exception.Message);
            element.SetAttribute("source", exception.Source);
            element.SetAttribute("targetSite", exception.TargetSite.ToString());
            element.SetAttribute("stackTrace", exception.StackTrace);
            element.SetAttribute("helpLink", exception.HelpLink);

            var query = from prop in exception.GetType().GetProperties()
                        where prop.DeclaringType != typeof(Exception) && prop.GetIndexParameters().Length == 0
                        select prop;

            foreach (PropertyInfo pi in query)
            {
                object value = pi.GetValue(exception, null);
                element.SetAttribute(pi.Name, value != null ? value.ToString() : "null");
            }

            if (exception.InnerException != null)
            {
                element.AppendChild(this.CreateExceptionElement(exception.InnerException));
            }

            return element;
        }

        public void OnError(string category, Exception exception, string message)
        {
            XmlElement element = this._helperDocument.CreateElement("error");
            element.SetAttribute("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttribute("category", category);
            element.SetAttribute("importance", MessageImportance.High.ToString());
            element.SetAttribute("text", message);
            if (exception != null)
            {
                element.AppendChild(this.CreateExceptionElement(exception));
            }

            this.Log(element);
        }

        #endregion
    }

   
}
