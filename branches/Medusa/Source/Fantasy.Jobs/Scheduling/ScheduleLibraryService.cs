using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.IO;
using Fantasy.Jobs.Management;
using System.ServiceModel;
using System.Xml;
using Fantasy.XSerialization;
using System.IO;
using System.Threading;
using System.Xml.Linq;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Scheduling
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults=true, ConcurrencyMode=ConcurrencyMode.Multiple, Namespace=Consts.JobServiceNamespaceURI)]
    public class ScheduleLibraryService : WCFSingletonService, IScheduleLibraryService
    {
        private IScheduleService _scheduleService;

        public override void InitializeService()
        {
            _scheduleService = this.Site.GetService<IScheduleService>();
            LoadSchedules();
            
            base.InitializeService();
        }


        private List<ScheduleData> _schedules = new List<ScheduleData>();

        private void LoadSchedules()
        {
            ILogger logger = this.Site.GetService<ILogger>();
           

            string root = JobManagerSettings.Default.ScheduleDirectoryFullPath;
            string searchPattern = "*" + ScheduleExt;
            IEnumerable<string> dirs = root.Flatten(s => LongPathDirectory.EnumerateDirectories(s));
            IEnumerable<string> files = from dir in dirs
                                        from file in LongPathDirectory.EnumerateFiles(dir, searchPattern)
                                        select file;
            XSerializer schedSer = new XSerializer(typeof(ScheduleItem));
            XSerializer dataSer = new XSerializer(typeof(ScheduleData));
            foreach (string file in files)
            {
                string name = this.ToScheduleRelativePath(file);
                name = name.Remove(name.Length - ScheduleExt.Length);
                
                try
                {
                    XElement doc = XElement.Load(file);
                    
                    ScheduleItem sched = (ScheduleItem)schedSer.Deserialize(doc);

                    string dataFile = file + ScheduleDataExt;

                    ScheduleData data;
                    if (LongPathFile.Exists(dataFile))
                    {
                        XElement dataDoc = XElement.Load(dataFile);
                        
                        data = (ScheduleData)dataSer.Deserialize(dataDoc);
                    }
                    else
                    {
                        data = new ScheduleData();
                    }

                    data.Site = this.Site;
                    data.Name = name;
                    sched.Name = name;
                    data.ScheduleItem = sched;

                    sched.Trigger.NextRunTime = data.NextRunTime;
                  
                    this._schedules.Add(data);
                    if (data.OnLoad())
                    {
                        SaveData(data); 
                    }

                    this.RegisterToScheduleService(data);
                    
                }
                catch(Exception error)
                {
                    if (logger != null)
                    {
                        logger.LogError("Schedule", error, "Failed to load schedule {0}.", name);
                    }
                }
            }
        }

        private void RegisterToScheduleService(ScheduleData data)
        {
            string message;
            if (data.ScheduleItem.Enabled)
            {
                if (!data.Expired)
                {
                    if(this._scheduleService != null)
                    {
                        data.ScheduleCookie = this._scheduleService.Register(data.NextRunTime.Value, ()=>
                            {
                                data.RunAction();
                                data.EvalNext();
                                if (!data.Expired)
                                {
                                    RegisterToScheduleService(data);
                                }
                                if (data.Expired && data.ScheduleItem.DeleteAfterExpired)
                                {
                                    this._schedules.Remove(data);
                                    string path = LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, data.ScheduleItem.Name + ScheduleExt);
                                    if (LongPathFile.Exists(path))
                                    {
                                        LongPathFile.Delete(path);
                                    }
                                    path += ScheduleDataExt;
                                    if (LongPathFile.Exists(path))
                                    {
                                        LongPathFile.Delete(path);
                                    }
                                }
                                else
                                {
                                    SaveData(data);
                                }
                              
                            });
                    }
                    message = String.Format("Schedule {0} will be execute at {1}.", data.Name, data.NextRunTime);
                }
                else
                {
                    message = String.Format("Schedule {0} is enabled.", data.Name);
                }
            }
            else
            {
                message = String.Format("Schedule {0} is disabled.", data.Name);
            }
            ILogger logger = this.Site.GetService<ILogger>();
            if (logger != null)
            {
                logger.LogMessage("Schedule", message);
            }
        }

        private void SaveData(ScheduleData data)
        {
            SaveXml(LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, data.Name + ScheduleExt + ScheduleDataExt), data, true);
        }

        public override void UninitializeService()
        {
            base.UninitializeService();
        }


        private string ToScheduleRelativePath(string path)
        {
            string rs = LongPath.GetRelativePath(JobManagerSettings.Default.ScheduleDirectoryFullPath + "\\", path);
            if (rs.StartsWith(".\\"))
            {
                rs = rs.Substring(2);
            }
            return rs;
        }

       

      


        private const string ScheduleExt = ".xsched";
        private const string ScheduleDataExt = ".xdat";

        public void DeleteGroup(string path)
        {

            var query = from data in this._schedules where data.Name.StartsWith(path + "\\", StringComparison.OrdinalIgnoreCase) select data.Name;
            string[] schedules = query.ToArray();
            foreach (string schedule in schedules)
            {
                this.DeleteSchedule(schedule); 
            }

            path = LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, path);
            if (LongPathDirectory.Exists(path))
            {
                LongPathDirectory.Delete(path);
            }
        }

        private XmlWriterSettings _xmlWriterSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, Indent = true, IndentChars = "    ", OmitXmlDeclaration = false, CloseOutput = true };

        private object _syncRoot = new object();

        private ScheduleData GetDataByPath(string path)
        {
            var query = from d in this._schedules where StringComparer.OrdinalIgnoreCase.Compare(path, d.Name) == 0 select d;
            return query.SingleOrDefault(); 
        }

        private void SaveXml(string path, object instance, bool overwrite)
        {
           
           
            XSerializer ser = new XSerializer(instance.GetType() );
            XElement ele = ser.Serialize(instance);
           

            FileStream fs = LongPathFile.Open(path, overwrite ? FileMode.Create : FileMode.CreateNew, FileAccess.Write);
            XmlWriter writer = XmlWriter.Create(fs, _xmlWriterSettings);
            try
            {
             
                ele.Save(writer);
            }
            finally
            {
                writer.Close();
            }
        }

        #region IScheduleLibraryService Members

        public void CreateSchedule(string path, ScheduleItem schedule, bool overwrite)
        {
            lock (_syncRoot)
            {
                IScheduleService svc = this.Site.GetService<IScheduleService>();
                ScheduleData old = this.GetDataByPath(path);

                if (old == null || overwrite)
                {
                    List<ScheduleEvent> events = new List<ScheduleEvent>();
                    if (old != null)
                    {
                        if (old.ScheduleItem.Enabled || !old.Expired && svc != null)
                        {
                            svc.Unregister(old.ScheduleCookie);
                        }
                        events.AddRange(old.History);
                        this._schedules.Remove(old);
                    }
                    schedule.Name = path;
                    ScheduleData data = new ScheduleData();
                    data.Site = this.Site;
                    data.Name = path;
                    data.ScheduleItem = schedule;
                    data.History.AddRange(events);

                    SaveXml(LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath,path + ScheduleExt), schedule, true);
                    this._schedules.Add(data);

                    data.OnLoad();
                    this.SaveData(data);
                    this.RegisterToScheduleService(data);

                }
                else
                {
                    throw new ScheduleException(String.Format(Properties.Resources.ScheduleExistsText, path));
                }


            }

        }

        public void DeleteSchedule(string path)
        {
            lock (_syncRoot)
            {
                IScheduleService svc = this.Site.GetService<IScheduleService>();
                ScheduleData data = this.GetDataByPath(path);
                if (data != null)
                {
                    if (data.ScheduleItem.Enabled || !data.Expired && svc != null)
                    {
                        svc.Unregister(data.ScheduleCookie);
                    }
                    this._schedules.Remove(data);
                }

                path = LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, path + ScheduleExt);
                if (LongPathFile.Exists(path))
                {
                    LongPathFile.Delete(path);
                }
                path += ScheduleDataExt;
                if (LongPathFile.Exists(path))
                {
                    LongPathFile.Delete(path);
                }

            }
        }


        public string[] GetScheduleNames(string path)
        {

            path = path != string.Empty ? path + "\\" : path;

            var query = from data in this._schedules
                        where data.Name.StartsWith(path, StringComparison.OrdinalIgnoreCase) 
                            && data.Name.Substring(path.Length).IndexOf('\\') < 0
                        select data.Name;
            return query.ToArray();
        }

        public ScheduleItem GetSchedule(string path)
        {
            ScheduleData data = this.GetDataByPath(path);
            return data != null ? data.ScheduleItem : null;
        }

        public ScheduleEvent[] GetScheduleHistory(string path)
        {
            ScheduleData data = this.GetDataByPath(path);
            return data != null ? data.History.ToArray() : null;
        }

        public string[] GetTemplateNames()
        {
            List<string> rs = new List<string>();
            foreach (string file in LongPathDirectory.EnumerateFiles(JobManagerSettings.Default.ScheduleTemplateDirectoryFullPath, "*.xslt"))
            {
                rs.Add(Path.GetFileNameWithoutExtension(file));
            }
            return rs.ToArray();
        }

        public string GetTemplate(string name)
        {
            string file = LongPath.Combine(JobManagerSettings.Default.ScheduleTemplateDirectoryFullPath, name + ".xslt");
            return LongPathFile.ReadAllText(file);
        }

        public void CreateGroup(string path)
        {
            path = LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, path);
            if (!LongPathDirectory.Exists(path))
            {
                LongPathDirectory.Create(path);
            }
        }

        public string[] GetGroups(string path)
        {
            path = LongPath.Combine(JobManagerSettings.Default.ScheduleDirectoryFullPath, path);
            string[] dirs = LongPathDirectory.EnumerateDirectories(path).ToArray();
            var query = from p in dirs select ToScheduleRelativePath(p);
            return query.ToArray();
        }

        #endregion
    }
}
