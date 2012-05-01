using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using System.Web.Mvc;
using Fantasy.Web.Mvc;

namespace Fantasy.Web.Models
{
   

    public class MenuItemModel
    {

        public MenuItemModel()
        {
            this.ChildItems = new List<MenuItemModel>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public List<MenuItemModel> ChildItems { get; private set; }




        public static MenuItemModel CreateRoot(UrlHelper urlHelper)
        {
            IBusinessMenuService svc = BusinessEngineContext.Current.GetRequiredService<IBusinessMenuService>();

            MenuItemModel rs = CreateMenuItemRecursive(urlHelper, svc.Root);

            return rs;

        }

        private static MenuItemModel CreateMenuItemRecursive(UrlHelper urlHelper, BusinessMenuItem item)
        {
            MenuItemModel rs = new MenuItemModel();
            IBusinessApplicationService appSvc = BusinessEngineContext.Current.GetRequiredService<IBusinessApplicationService>();

            foreach (BusinessMenuItem childItem in item.ChildItems)
            {
                MenuItemModel childModel = CreateMenuItemRecursive(urlHelper,childItem);
                if (childModel != null)
                {
                    rs.ChildItems.Add(childModel);
                }
            }

            if (item.ApplicationId != null)
            {
                string appName = appSvc.GetApplicationFriendlyName((Guid)item.ApplicationId);
                rs.Url = urlHelper.ApplicationUrl(appName:appName, rootId:item.EntryObjectId);
            }
            else if (!string.IsNullOrWhiteSpace(item.ExternalUrl))
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
                    rs.Icon = urlHelper.ImageList(ms.GetIconKey(item));

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