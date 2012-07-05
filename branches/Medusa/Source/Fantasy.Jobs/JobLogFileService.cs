using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.ServiceModel;
using System.IO;
using Fantasy.IO;

namespace Fantasy.Jobs
{
    public class JobLogFileService : LogFileService
    {


        public override void InitializeService()
        {
            IJobEngine engine = (IJobEngine)((IServiceProvider)this.Site).GetService(typeof(IJobEngine));

            string filePath = string.Format("{0}\\{1}.xlog", engine.JobDirectory, engine.JobId);
            FileStream fs = LongPathFile.Open(filePath, FileMode.Append, FileAccess.Write, FileShare.Read);
            _writer = new StreamWriter(fs, Encoding.UTF8);
            WriteStart();

            base.InitializeService();
        }

        public override void UninitializeService()
        {
            base.UninitializeService();
            this._writer.Close();
        }

        public static string GetLogFilePath(string jobsDirectory, Guid jobId)
        {
            return string.Format("{0}\\{1}\\{1}.xlog", jobsDirectory, jobId);
        }

        private StreamWriter _writer;

        protected override StreamWriter GetWriter()
        {
            return this._writer;
        }
    }
}
