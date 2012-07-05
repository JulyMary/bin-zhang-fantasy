using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Jobs.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Default()
        {
            return View();
        }

        public ActionResult ServiceUnavailable()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}
