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

            MenuItemModel root = this.CreateMenuItemRecursive(svc.Root);

            return View(root);

        }

        private MenuItemModel CreateMenuItemRecursive(BusinessMenuItem item)
        {
            MenuItemModel rs = new MenuItemModel();
            IBusinessApplicationService appSvc = BusinessEngineContext.Current.GetRequiredService<IBusinessApplicationService>();
           
            foreach (BusinessMenuItem childItem in item.ChildItems)
            {
                MenuItemModel childModel = CreateMenuItemRecursive(childItem);
                if (childModel != null)
                {
                    rs.ChildItems.Add(childModel);
                }
            }

            if (item.ApplicationId != null)
            {
                string appName = appSvc.GetApplicationFriendlyName((Guid)item.ApplicationId);
                rs.Url = this.Url.ApplicationUrl(appName, item.EntryObjectId != null ? new { entry = (Guid)item.EntryObjectId } : null);
            }
            else if(!string.IsNullOrWhiteSpace(item.ExternalUrl))
            {
                rs.Url = item.ExternalUrl;
            }

            IBusinessUserRoleService urs = BusinessEngineContext.Current.GetRequiredService<IBusinessUserRoleService>();

            if (!String.IsNullOrEmpty(rs.Url) || rs.ChildItems.Count > 0)
            {
                Guid[] roleId = urs.GetRolesForUser(BusinessEngineContext.Current.User).Select(r => r.Id).ToArray();
                if (roleId.Any(id => item.Roles.IndexOf(id) >= 0))
                {
                    rs.Name = item.Name;
                    rs.Description = item.Description;
                    IBusinessMenuService ms = BusinessEngineContext.Current.GetRequiredService<IBusinessMenuService>();
                    rs.Icon = this.Url.ImageList(ms.GetIconKey(item));

                    if (string.IsNullOrEmpty(rs.Url))
                    {
                        rs.Url = "#";
                    }

                    return rs;
                }
            }

            return null;


        }
    }
}
