using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Management;
using System.Threading;
using Fantasy.Jobs.Web.Models;
using Fantasy.Tracking;

namespace Fantasy.Jobs.Web.Controllers
{
    public class RunningJobsController : Controller
    {
        //
        // GET: /RunningJobs/

        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult List()
        {
            RunningJobsModel model = CreateModel();

            return PartialView(model);
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet );
        }
       
        public ActionResult Refresh(long version)
        {
            if (version < RunningJobs.Default.Version)
            {
                RunningJobsModel model = new RunningJobsModel() { Expired = true };
                return Json(model);
            }
            else
            {
                RunningJobsModel model = CreateModel();
                return Json(model);
            }
        }

        private  RunningJobsModel CreateModel()
        {
            RunningJobsModel model = new RunningJobsModel()
            {
                Version = RunningJobs.Default.Version
            };
            JobMetaData[] meta = RunningJobs.Default.Jobs;
            ITrackListener[] tracks = RunningJobs.Default.Tracks;

            JobModel[] jms = new JobModel[meta.Length];
            for (int i = 0; i < meta.Length; i++)
            {
                jms[i] = new JobModel() { MetaData = meta[i] };
                ITrackListener track = tracks.Where(t => t.Id == meta[i].Id).SingleOrDefault();
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
                    jms[i].Progress = progress;
                    jms[i].Status = (string)track["status"] ?? string.Empty;

                }
            }
            model.Jobs = jms;
            return model;
        }


    }
}
