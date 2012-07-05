using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Management;
using Fantasy.Jobs.Web.Models;
using Fantasy.Tracking;
using System.IO;
using System.Xml.Linq;
using System.Text;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Controllers
{

    public class JobController : Controller
    {
        //
        // GET: /Job/

        public ActionResult Details(Guid id)
        {
            JobModel model = CreateModel(id);

            return View(model);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Refresh(Guid id)
        {
            JobModel model = CreateModel(id);
            model.ImageUrl = JobStateImageHelper.GetImageUrl(Url, model.MetaData.State);   
            return Json(model);
        }

        public JsonResult Stop(Guid id)
        {
            try
            {
                using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
                {

                    svc.Client.Cancel(id);
                }
            }
            catch
            {
            }
            return Json(true); 

        }

        public JsonResult Resume(Guid id)
        {
            try
            {
                using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
                {
                    service.Client.Resume(id);
                }
            }
            catch
            {
            }
            return Json(true); 
        }

        public JsonResult Pause(Guid id)
        {
            try
            {
                using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
                {
                    service.Client.Pause(id);
                }
            }
            catch
            {
            }
            return Json(true); 
        }

        private static JobModel CreateModel(Guid id)
        {
            using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
            {
                JobMetaData meta = service.Client.FindJobById(id);
                JobModel model = new JobModel();
                model.MetaData = meta;
                if (model.MetaData.State == JobState.Running)
                {
                    ITrackListener track = RunningJobs.Default.Tracks.Where(t => t.Id == id).SingleOrDefault();
                    if (track != null)
                    {
                        double max = Convert.ToDouble(track["progress.maximum"]);
                        double min = Convert.ToDouble(track["progress.minimum"]);
                        double value = Convert.ToDouble(track["progress.value"]);
                        int progress = (int)(100 * (value - min) / (max - min));
                        if (progress > 100 || progress < 0)
                        {
                            progress = 0;
                        }
                        model.Progress = progress;
                        model.Status = (string)track["status"] ?? string.Empty;
                    }
                }
                model.Script = service.Client.GetJobScript(id);
                return model;
            }
        }

        public ActionResult Log(Guid id)
        {

            using (ClientRef<IJobService> service = ClientFactory.Create<IJobService>())
            {

                JobMetaData meta = service.Client.FindJobById(id);

                string log = service.Client.GetJobLog(id);
                XElement element = null;
                if (log != null)
                {
                    MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(log));
                    StreamReader reader = new StreamReader(stream, Encoding.Unicode);
                    element = XLogHelper.Load(reader);
                }


                JobLogModel model = new JobLogModel()
                {
                    Job = meta,
                    Log = element
                };
                return View(model);
            }
        }

    }
}
