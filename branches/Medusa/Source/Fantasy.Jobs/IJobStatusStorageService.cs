using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs
{
    public interface IJobStatusStorageService
    {
        void Save(Stream content);
        Stream Load();
        Stream LoadBackup();

        bool Exists { get; }
    }

    public class JobStatusStorageService : AbstractService, IJobStatusStorageService
    {
        #region IJobStorageService Members


        public bool Exists
        {
            get
            {
                return File.Exists(this.GetFileName());
            }
        }

        public void Save(Stream stream)
        {
            string name = this.GetFileName();
            if (File.Exists(name))
            {
                File.Copy(name, name + ".bak", true);
            }
            
            FileStream fs = new FileStream(name, FileMode.Create);
            
            try
            {
                int count = 0;
                byte[] buffer = new byte[1024];
                do
                {

                    count = stream.Read(buffer, 0, buffer.Length);
                    fs.Write(buffer, 0, count);

                } while (count == buffer.Length);
            }
            finally
            {
                fs.Close();
            }
 
        }

        private string GetFileName()
        {
                string rs;
                IJobEngine engine = (IJobEngine)this.Site.GetService(typeof(IJobEngine));
                rs = string.Format("{0}\\{1}.cvjob", engine.JobDirectory, engine.JobId);
                return rs;
        }

       

        public Stream  Load()
        {
            string name = this.GetFileName();
            return InnerLoad(name);
        }

        private static Stream InnerLoad(string name)
        {
            
            MemoryStream rs = new MemoryStream();
            if (File.Exists(name))
            {
                FileStream fs = new FileStream(name, FileMode.Open);
                try
                {
                    int count = 0;
                    byte[] buffer = new byte[1024];
                    do
                    {

                        count = fs.Read(buffer, 0, buffer.Length);
                        rs.Write(buffer, 0, count);

                    } while (count == buffer.Length);
                }
                finally
                {
                    fs.Close();
                }
            }
            rs.Position = 0;

            return rs;
        }

        public Stream LoadBackup()
        {
            string name = this.GetFileName() + ".bak";
            return InnerLoad(name);
        }

        #endregion
    }

}
