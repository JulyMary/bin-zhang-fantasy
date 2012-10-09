using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Org._12306.Tickets.ServiceModel
{
    public class LogService : AbstractService, ILogger
    {
        private object _syncRoot = new object();

        private void FireEvent(Action<ILogListener>  method)
        {
            ILogListener[] listeners;
            lock (_syncRoot)
            {
                 listeners = this._listeners.ToArray();
            }

            List<ILogListener> expired = new List<ILogListener>();
            foreach(ILogListener listener in listeners)
            {
                try
                {
                    method(listener);
                }
                catch
                {
                    expired.Add(listener);
                }
            }
            lock(_syncRoot)
            {
                foreach (ILogListener listener in expired)
                {
                    _listeners.Remove(listener);
                }
            }
        }


        private List<ILogListener> _listeners = new List<ILogListener>();
        #region IJobLogger Members

        public const int MAX_MESSAGE_LENGTH = 1024;
        public void LogMessage(string category, MessageImportance importance, string message, params object[] messageArgs)
        {
            message = string.Format(message, messageArgs);
            if (message != null && message.Length > MAX_MESSAGE_LENGTH)
                message = message.Substring(0, MAX_MESSAGE_LENGTH) +"...";
            this.FireEvent(delegate(ILogListener listener) { listener.OnMessage(category, importance, message); });   
        }

        public void LogMessage(string category, string message, params object[] messageArgs)
        {
            this.LogMessage(category, MessageImportance.Low, message, messageArgs);   
        }

        public void LogWarning(string category, Exception exception, MessageImportance importance, string message, params object[] messageArgs)
        {
            message = string.Format(message, messageArgs);
            if (message != null && message.Length > MAX_MESSAGE_LENGTH)
                message = message.Substring(0, MAX_MESSAGE_LENGTH) + "...";
            this.FireEvent(delegate(ILogListener listener) { listener.OnWaring(category, exception, importance, message); });  
        }

        public void LogWarning(string category, MessageImportance importance, string message, params object[] messageArgs)
        {
            this.LogWarning(category, null, importance, message, messageArgs);  
        }

        public void LogWarning(string category, string message, params object[] messageArgs)
        {
            this.LogWarning(category, null, MessageImportance.Low, message, messageArgs);  
        }

        public void LogError(string category, Exception exception, string message, params object[] messageArgs)
        {
            message = string.Format(message, messageArgs);
            this.FireEvent(delegate(ILogListener listener) { listener.OnError(category, exception, message);});  
        }

        public void LogError(string category, string message, params object[] messageArgs)
        {
            this.LogError(category, null, message, messageArgs); 
        }

        public void AddListener(ILogListener listener)
        {
            lock (_syncRoot)
            {
                this._listeners.Add(listener);
            }
        }

        public void RemoveListener(ILogListener listener)
        {
            lock (_syncRoot)
            {
                this._listeners.Remove(listener);
            }
        }

        #endregion
    }
}
