using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Fantasy.ServiceModel;
using System.Text.RegularExpressions;

namespace Fantasy.Jobs
{
    public class LogTraceListener : TraceListener
    {
        private ILogger _logger;

        public LogTraceListener()
        {
            
        }

        private bool _disposed = false;

        private Regex _regex = new Regex(@"\{\d+(:[^\}]*)?\}", RegexOptions.Multiline);
       

        public LogTraceListener(ILogger logger)
        {
            this._logger = logger;
        }



        public override void Write(string message)
        {
            if (!this._disposed && _logger != null)
            {
                message = this.RemoveFormat(message);
                _logger.LogMessage("Trace", message); 
            }
        }

        private string RemoveFormat(string message)
        {
            lock (_regex)
            {
                return _regex.Replace(message, string.Empty);
            }
        }

        public override void WriteLine(string message)
        {

            if (!this._disposed && _logger != null)
            {
                message = this.RemoveFormat(message);
                _logger.LogMessage("Trace", message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _disposed = true;
            base.Dispose(disposing);
        }
    }
}
