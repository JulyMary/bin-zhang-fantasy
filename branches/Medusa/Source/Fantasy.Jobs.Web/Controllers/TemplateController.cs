using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Management;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Controllers
{
    public class TemplateController : Controller
    {
        //
        // GET: /Template/

        public ActionResult Index()
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                JobTemplate[] model = svc.Client.GetJobTemplates();
                return View(model);
            }
        }

        public ActionResult Details(int id)
        {
            using (ClientRef<IJobService> svc = ClientFactory.Create<IJobService>())
            {
                var query = from temp in svc.Client.GetJobTemplates() where temp.id == id select temp;
                JobTemplate model = query.Single();

                return View(model);
            }

        }

    }
}
