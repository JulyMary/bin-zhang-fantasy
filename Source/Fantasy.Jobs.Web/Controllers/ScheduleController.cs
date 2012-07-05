using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Jobs.Web.Models;
using Fantasy.Jobs.Scheduling;
using System.IO;
using Fantasy.IO;
using Fantasy.ServiceModel;

namespace Fantasy.Jobs.Web.Controllers
{
    [ValidateInput(false)]
    public class ScheduleController : Controller
    {
        //
        // GET: /Schedule/

        protected override JsonResult  Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
 	         return base.Json(data, contentType, contentEncoding, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(string g)
        {
            if (!String.IsNullOrEmpty(g) && g.StartsWith(".\\"))
            {
                g = g.Substring(2);
            }

            this.ViewData["selected"] = string.IsNullOrEmpty(g) ? "." : g;
            
            ScheduleGroup model = GetRootGroup();
            return View(model);
        }


        private ScheduleGroup GetRootGroup()
        {
            ScheduleGroup root = new ScheduleGroup() { DisplayName = "Schedule Library", Path = "." };

            LoadChildGroups(root);

            return root;
        }

        private IScheduleLibraryService _scheduleService; 
        public IScheduleLibraryService ScheduleService
        {
            get
            {
                if (_scheduleService == null)
                {
                    _scheduleService = ClientFactory.Create<IScheduleLibraryService>().Client;
                }
                return _scheduleService;
            }
        }

        private void LoadChildGroups(ScheduleGroup group)
        {
           
            string[] paths = this.ScheduleService.GetGroups(group.Path);
            foreach (string childPath in paths)
            {
                ScheduleGroup child = new ScheduleGroup() { Path = childPath, DisplayName = childPath.Split('\\').Last() };
                group.ChildGroups.Add(child);
                LoadChildGroups(child);
            }
        }


        

        public ActionResult CreateGroup(string path)
        {
          
            this.ScheduleService.CreateGroup(path);
            return RedirectToActionPermanent("Index", new { g = path });
            
            
        }

        public ActionResult RemoveGroup(string path)
        {
            this.ScheduleService.DeleteGroup(path);
            string parent = Path.GetDirectoryName(path);
            return RedirectToActionPermanent("Index", new { g = parent }); 
        }


        public PartialViewResult ScheduleList(string group)
        {
            if (group == ".")
            {
                group = string.Empty;
            }

            string[] names = this.ScheduleService.GetScheduleNames(@group);
            List<ScheduleItem> model = new List<ScheduleItem>();
            foreach(string name in names)
            {
                model.Add(this.ScheduleService.GetSchedule(name));
            }
           
            return PartialView(model);
        }

        public ActionResult Edit(string path)
        {
            ScheduleItem item = this.ScheduleService.GetSchedule(path);
            if (item == null)
            {
                throw new ArgumentException(String.Format("Can not find schedule {0}.", path), "path");
            }

            ScheduleModel model = new ScheduleModel();
            
            model.IsNew = false;
            model.ParentPath = Path.GetDirectoryName(path);
            model.Name = Path.GetFileName(path);
            model.AvailableTemplates = this.ScheduleService.GetTemplateNames();
            if (this.Request.HttpMethod.ToUpper() == "POST")
            {
                this.TryUpdateModel(model);
                if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
                {
                    model.Author = HttpContext.User.Identity.Name;
                }
                else
                {
                    model.Author = "Anonymous";
                }


                item = model.ToSchedule();

                this.ScheduleService.CreateSchedule(path, item, true);

            }
            else
            {
                model.LoadFromSchedule(item);
            }
            
            return View("Schedule", model);
        }

        public ActionResult NewSchedule(string path)
        {
            ScheduleModel model = new ScheduleModel();
            model.IsNew = true;
            model.ParentPath = path;
            model.Name = "new task";
            model.StartTime = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(HttpContext.User.Identity.Name))
            {
                model.Author = HttpContext.User.Identity.Name;
            }
            else
            {
                model.Author = "Anonymous";
            }
            model.AvailableTemplates = this.ScheduleService.GetTemplateNames();
            return View("Schedule", model);
        }

        [HttpPost] 
        public ActionResult NewSchedule(string path, FormCollection form)
        {
            ScheduleModel model = new ScheduleModel();

            this.TryUpdateModel(model);
            model.Author = HttpContext.User.Identity.Name;
            ScheduleItem item = model.ToSchedule();
            string fullPath;
            if (String.IsNullOrEmpty(path) || path == "." || path == "\\")
            {
                fullPath = model.Name;
            }
            else
            {
                fullPath = path + "\\" + model.Name;
            }

            this.ScheduleService.CreateSchedule(fullPath, item, false);

            return RedirectToActionPermanent("Edit", new {path=fullPath});
        }

        public ActionResult RemoveSchedule(string path)
        {
            this.ScheduleService.DeleteSchedule(path);
            string parent = Path.GetDirectoryName(path);
            return RedirectToActionPermanent("ScheduleList", new { group = parent });
        }

    }
}
