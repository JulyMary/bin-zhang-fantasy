using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.IO;
using System.Threading.Tasks;
using Fantasy.IO;
using System.Xml.Linq;
using Fantasy.Configuration;
using System.Reflection;

namespace Fantasy.Jobs.Management
{


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI )]
    public class JobService  : WCFSingletonService, IJobService
    {
       
        private IJobQueue Queue
        {
            get
            {
                return JobManager.Default.GetRequiredService<IJobQueue>();
            }
        }

       

        public JobMetaData StartJob(string startInfo)
        {
           
            JobMetaData job = Queue.CreateJobMetaData();
            job.LoadXml(startInfo);
            Queue.ApplyChange(job);
            return job; 
        }

        public void Resume(Guid id)
        {
            Queue.Resume(id);
        }

        public void Cancel(Guid id)
        {
            Queue.Cancel(id);
        }

        public void Pause(Guid id)
        {
            Queue.UserPause(id);
        }

        private JobMetaData[] GetJobMetaDataByIds(Guid[] ids)
        {
            if (ids != null)
            {
                List<JobMetaData> rs = new List<JobMetaData>();
                foreach (Guid id in ids)
                {
                    JobMetaData job = Queue.FindJobMetaDataById(id);
                    if (job != null)
                    {
                        rs.Add(job);
                    }
                }
                return rs.ToArray();
            }
            else
            {
                return new JobMetaData[0]; 
            }
        }

        private void Resume(JobMetaData[] jobs)
        {
            foreach (JobMetaData job in jobs)
            {
                try
                {
                    if (job.State == JobState.UserPaused)
                    {
                        Queue.Resume(job.Id);
                    }
                }
                catch
                {
                }
            }
        }

       

        public void Resume(Guid[] ids)
        {
            JobMetaData[] jobs = GetJobMetaDataByIds(ids);
            Resume(jobs);
        }

        public void ResumeByFilter(string filter, string[] args = null)
        {
            int total;
            JobMetaData[] jobs = Queue.FindUnterminated(out total, filter, args, null, 0, Int32.MaxValue).ToArray();
            this.Resume(jobs);
        }


        private void Cancel(JobMetaData[] jobs)
        {
            var query = from job in jobs where !job.IsTerminated orderby job.State != JobState.Running select job;
            foreach (JobMetaData job in query)
            {
                try
                {
                    Queue.Cancel(job.Id);
                }
                catch
                {
                }
            }
        }

        public void Cancel(Guid[] ids)
        {
            JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
            this.Cancel(jobs);
        }


        public void CancelByFilter(string filter, string[] args = null)
        {
            int total;
            JobMetaData[] jobs = Queue.FindUnterminated(out total, filter, args, null, 0, Int32.MaxValue).ToArray();
            Cancel(jobs);
           
        }

        private void Pause(JobMetaData[] jobs)
        {
            var query = from job in jobs where job.IsTerminated == false && job.State != JobState.UserPaused orderby job.State != JobState.Running select job;
            foreach (JobMetaData job in query)
            {
                try
                {
                    Queue.UserPause(job.Id); 
                }
                catch
                {
                }
            }
        }

        public void Pause(Guid[] ids)
        {
            JobMetaData[] jobs = this.GetJobMetaDataByIds(ids);
            this.Pause(jobs);
        }

       

        

        public void PauseByFilter(string filter, string[] args = null)
        {
            int total;
            JobMetaData[] jobs = Queue.FindUnterminated(out total, filter, args, null, 0, Int32.MaxValue).ToArray();
            Pause(jobs);
        }


        public JobMetaData FindJobById(Guid id)
        {
            return Queue.FindJobMetaDataById(id);
        }

        public JobMetaData[] FindAllJobs()
        {
            return Queue.FindAll().ToArray(); 
        }

        public JobMetaData[] FindUnterminatedJob(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
            return Queue.FindUnterminated(out totalCount, filter, args, order, skip, take).ToArray();
        }

        public JobMetaData[] FindTerminatedJob(out int totalCount, string filter, string[] args, string order, int skip, int take)
        {
           
            return Queue.FindTerminated(out totalCount, filter, args, order, skip, take).ToArray();
           
        }

        public string GetJobLog(Guid id)
        {
            string path = String.Format("{0}\\{1}\\{1}.xlog", JobManagerSettings.Default.JobDirectoryFullPath, id);
            string rs = null;
            try
            {
                rs = LongPathFile.ReadAllText(path);
            }
            catch (IOException)
            {

            }

            return rs;
        }

        public string GetJobScript(Guid id)
        {
            string rs = null;
            string path = String.Format("{0}\\{1}\\{1}.cvjob", JobManagerSettings.Default.JobDirectoryFullPath, id);
            if (!File.Exists(path))
            {
                path += ".bak";
            }
            if (File.Exists(path))
            {
                try
                {
                    rs = File.ReadAllText(path);
                }
                catch (IOException)
                {

                }
            }

            return rs;
        }

        public string GetManagerLog(DateTime date)
        {
            XElement rs = new XElement("xlog");
            string path = JobManagerSettings.Default.LogDirectoryFullPath;
            path = LongPath.Combine(path, date.ToString("yyyy-MM-dd") + ".xlog");
            if (LongPathFile.Exists(path))
            {
                List<XElement> nodes = new List<XElement>();
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader reader = new StreamReader(fs);
                try
                {
                    string line;
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();

                        try
                        {
                            XElement node = XElement.Parse(line);
                            rs.Add(node);
                        }
                        catch
                        {
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }


            }

            return rs.ToString();
        }

        public DateTime[] GetManagerLogAvaiableDates()
        {
            List<DateTime> rs = new List<DateTime>();
            string path = JobManagerSettings.Default.LogDirectoryFullPath;
           
            foreach (string fullPath in LongPathDirectory.EnumerateFiles(path, "*.xlog"))
            {
                string name = Path.GetFileNameWithoutExtension(fullPath);
                DateTime date = DateTime.Parse(name);
                rs.Add(date);
            }
            rs.Sort();
            return rs.ToArray();
        }

        public JobTemplate[] GetJobTemplates()
        {
            IJobTemplatesService svc = JobManager.Default.GetRequiredService<IJobTemplatesService>();
            return svc.GetJobTemplates(); 
        }

        public int GetTerminatedCount()
        {
            return Queue.Terminates.Count();
        }

        public int GetUnterminatedCount()
        {
            return Queue.Unterminates.Count();
        }


        public string[] GetAllApplications()
        {
            return Queue.GetAllApplications();
        }

        public string[] GetAllUsers()
        {
            return Queue.GetAllUsers();
        }


      


        public string GetSettings(string typeName)
        {
            return GetDefaultSettingsInstance(typeName).ToXml();
        }

        public void SetSettings(string typeName, string xml)
        {
            SettingsBase settings = this.GetDefaultSettingsInstance(typeName);
            settings.Renew(xml);
            settings.Save();
        }


        private SettingsBase GetDefaultSettingsInstance(string typeName)
        {
            Type t = Type.GetType(typeName, true);
            SettingsBase rs = (SettingsBase)t.InvokeMember("Default", BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Static, null, null, null);
            return rs;
        }

        public string Version()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            return assembly.GetName().Version.ToString();
        }


        #region IJobService Members


        public string GetLocation()
        {
            return LongPath.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
        }

        #endregion
    }

    
}
