using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;

namespace Org._12306.Tickets.ServiceModel
{
    public abstract class LogFileService : AbstractService, ILogListener 
    {

      

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
        protected virtual void Log(XElement element)
        {
            lock (this._syncRoot)
            {
                StreamWriter writer = this.GetWriter();
                writer.WriteLine(element.ToString(SaveOptions.DisableFormatting | SaveOptions.OmitDuplicateNamespaces));
                writer.Flush(); 
            }
        }

        public void OnMessage(string category, MessageImportance importance, string message)
        {
            XElement element = new XElement("message");
            element.SetAttributeValue("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttributeValue("category", category);
            element.SetAttributeValue("importance", importance.ToString());
            element.SetAttributeValue("text", message);
            this.Log(element);
        }


        protected virtual void WriteStart()
        {
            XElement element = new XElement("start");
            element.SetAttributeValue("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttributeValue("category", "Start");
            element.SetAttributeValue("importance", MessageImportance.High.ToString());
            element.SetAttributeValue("text", String.Format("Start. Culture : {0}; UI Culture: {1}.", Thread.CurrentThread.CurrentCulture, Thread.CurrentThread.CurrentUICulture));
            this.Log(element);
        }

        public void OnWaring(string category, Exception exception, MessageImportance importance, string message)
        {
            XElement element = new XElement("warning");
            element.SetAttributeValue("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttributeValue("category", category);
            element.SetAttributeValue("importance", importance.ToString());
            element.SetAttributeValue("text", message);
            if(exception != null)
            {
                element.Add(this.CreateExceptionElement(exception));
            }

            this.Log(element);
        }

        private XElement CreateExceptionElement(Exception exception)
        {
            XElement element = new XElement("exception");
            element.SetAttributeValue("type", exception.GetType().ToString());
            element.SetAttributeValue("message", exception.Message);
            element.SetAttributeValue("source", exception.Source);
            element.SetAttributeValue("targetSite", exception.TargetSite.ToString());
            element.SetAttributeValue("stackTrace", exception.StackTrace);
            element.SetAttributeValue("helpLink", exception.HelpLink);

            var query = from prop in exception.GetType().GetProperties()
                        where prop.DeclaringType != typeof(Exception) && prop.GetIndexParameters().Length == 0
                        select prop;

            foreach (PropertyInfo pi in query)
            {
                object value = pi.GetValue(exception, null);
                element.SetAttributeValue(pi.Name, value != null ? value.ToString() : "null");
            }

            if (exception.InnerException != null)
            {
                element.Add(this.CreateExceptionElement(exception.InnerException));
            }

            return element;
        }

        public void OnError(string category, Exception exception, string message)
        {
            XElement element = new XElement("error");
            element.SetAttributeValue("time", DateTime.Now.ToUniversalTime().ToString(_timePattern));
            element.SetAttributeValue("category", category);
            element.SetAttributeValue("importance", MessageImportance.High.ToString());
            element.SetAttributeValue("text", message);
            if (exception != null)
            {
                element.Add(this.CreateExceptionElement(exception));
            }

            this.Log(element);
        }

        #endregion
    }

   
}
