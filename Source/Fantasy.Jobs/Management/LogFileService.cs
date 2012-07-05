using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.IO;

namespace Fantasy.Jobs.Management
{
    public class LogFileService : Fantasy.ServiceModel.LogFileService
    {
        private StreamWriter _writer = null;
        private DateTime _loggingDate = DateTime.MinValue.Date;
        private string _directory;

        public override void InitializeService()
        {
            _directory = LongPath.Combine(JobManagerSettings.Default.LogDirectoryFullPath, Environment.MachineName);
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory); 
            }
           
            base.InitializeService();
            this.WriteStart();
        }

        public override void UninitializeService()
        {
           
            base.UninitializeService();
            if (_writer != null)
            {
                _writer.Close();

            }
        }

       
        protected override System.IO.StreamWriter GetWriter()
        {

            if (_loggingDate != DateTime.Now.Date && _writer != null)
            {
                _writer.Close();
                _writer = null;
            }

            if (_writer == null)
            {
                this._loggingDate = DateTime.Now.Date;
                string filename = string.Format("{0}\\{1}.xlog", _directory, DateTime.Now.Date.ToString("yyyy-MM-dd"));
                FileStream fs = LongPathFile.Open(filename, FileMode.Append, FileAccess.Write, FileShare.Read);
                _writer = new StreamWriter(fs, Encoding.UTF8);
            }
            return _writer;
            
        }
    }
}
