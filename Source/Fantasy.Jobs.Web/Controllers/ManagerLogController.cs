using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Web.Models;
using Fantasy.IO;
using Fantasy.Jobs.Web.Properties;
using System.IO;
using System.Xml.Linq;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Web.Hosting;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;
using System.Web.Caching;

namespace Fantasy.Jobs.Web.Controllers
{
    public class ManagerLogController : Controller
    {
        

        public ActionResult Details(string server, string date)
        {
            DateTime selected;
            try
            {
                selected = DateTime.ParseExact(date, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern, null);
            }
            catch
            {
                selected = DateTime.Now.Date;
            }
            DirectoryInfo _rootDir = GetLogDirectory();
            

            var query = from dir in _rootDir.GetDirectories()
                        select dir.Name;

            ManagerLogModel model = new ManagerLogModel();
            model.Servers = query.Except(Settings.Default.ExcludeLogDirectories.Cast<string>(), StringComparer.OrdinalIgnoreCase).ToArray(); 

            model.SelectedServer = !string.IsNullOrEmpty(server) ? server : model.Servers.FirstOrDefault();

            model.AvailableDates = this.GetAvailableDates(model.SelectedServer);
            if(Array.IndexOf(model.AvailableDates, selected) >= 0 || model.AvailableDates.Length == 0)
            {
                model.Date = selected ;
            }
            else
            {
                model.Date = model.AvailableDates.Last();
            }
            model.Log = GetLog(model.SelectedServer, model.Date);
               

            if (model.AvailableDates.Length > 0)
            {
                model.MaxDate = model.AvailableDates.Last();
                model.MinDate = model.AvailableDates.First();
            }
            else
            {
                model.MinDate = model.MaxDate = DateTime.Now.Date;
            }

            return View(model);
        }

        private static DirectoryInfo GetLogDirectory()
        {

            DirectoryInfo rs = (DirectoryInfo)HostingEnvironment.Cache["JobServiceLogDirectory"];

            if (rs == null)
            {
                try
                {
                    using(ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                    {
                        string xml = svc.Client.GetSettings<JobManagerSettings>();
                        JobManagerSettings jobSettings = new JobManagerSettings();
                        jobSettings.Load(xml);
                        string root = svc.Client.GetLocation();
                        rs = new DirectoryInfo(LongPath.Combine(root, jobSettings.LogDirectory));
                        Settings.Default.JobServiceLogDirectory = rs.FullName;
                        Settings.Default.Save();
                    }
                }
                catch
                {
                }
                if (rs == null)
                {
                    rs = new DirectoryInfo(Settings.Default.JobServiceLogDirectory);
                }

                HostingEnvironment.Cache.Add("JobServiceLogDirectory", rs, null, DateTime.Now.AddMinutes(10), Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return rs;
        }

        private DateTime[] GetAvailableDates(string server)
        {
            List<DateTime> rs = new List<DateTime>();
            string path = Settings.Default.JobServiceLogDirectory;
            path = LongPath.Combine(ToFullPath(path), server);
            foreach (string fullPath in LongPathDirectory.EnumerateFiles(path, "*.xlog"))
            {
                string name = Path.GetFileNameWithoutExtension(fullPath);
                DateTime date = DateTime.Parse(name);
                rs.Add(date);
            }
            rs.Sort();
            return rs.ToArray();
        }

        private string ToFullPath(string path)
        {
            if (!Path.IsPathRooted(path))
            {
                string siteRoot = Server.MapPath("~/");
                path = LongPath.Combine(siteRoot, path);
            }
            return path;
        }

        private XElement GetLog(string server, DateTime date)
        {
            XElement rs = new XElement("xlog");

            string path = Settings.Default.JobServiceLogDirectory;
            path = ToFullPath(path);
            
            path = LongPath.Combine(path, server, date.ToString("yyyy-MM-dd") + ".xlog");
            if (LongPathFile.Exists(path))
            {
                List<XElement> nodes = new List<XElement>();
                FileStream fs = null;
                for (int i = 0; i < 5 && fs == null; i++)
                {
                    try
                    {
                        fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch
                    {
                        Thread.Sleep(200);
                    }
                }

                if (rs != null)
                {
                    StreamReader reader = new StreamReader(fs);
                    try
                    {
                        rs = XLogHelper.Load(reader);
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                
            }

            return rs;
        }
    }
}
