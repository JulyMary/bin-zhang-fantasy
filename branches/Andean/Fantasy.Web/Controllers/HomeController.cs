using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Web.Models;
using Fantasy.Web.Mvc;

namespace Fantasy.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your quintessential app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your quintessential contact page.";

            return View();


            //return this.RedirectToActionPermanent 
        }

        [ChildActionOnly]
        public ActionResult MainMenu()
        {
            IBusinessMenuService svc = BusinessEngineContext.Current.GetRequiredService<IBusinessMenuService>();

            MenuItemModel root = MenuItemModel.CreateRoot(this.Url);

            return View(root);

        }

       
    }
}
