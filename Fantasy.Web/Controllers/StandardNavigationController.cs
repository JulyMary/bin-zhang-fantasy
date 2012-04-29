using System;
using System.Web.Mvc;
using System.ComponentModel;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Security;
using Fantasy.Web.Models;
using Fantasy.Web.Mvc;
using Fantasy.BusinessEngine.Services;
using System.Linq;
using System.Collections;
using System.Web.Security;
namespace Fantasy.Web.Controllers
{
    
    public class StandardNavigationController : Controller, INavigationViewController
    {

        #region INavigationViewController Members

        public ViewResult Default()
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObject entryObject = application.EntryObject;

            StandardNavigationDefaultViewModel model = new StandardNavigationDefaultViewModel();
            model.RootTreeItem = this.CreateTreeItem(entryObject, 3);

            if (model == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }
            model.RootTreeItem.state = JsTreeNode.Open;

            

            return View(model);
           
        }

        private JsTreeNode CreateTreeItem(BusinessObject obj, int childDeep)
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObjectSecurity security = application.GetObjectSecurity(obj);
            if (security.Properties["Name"].CanRead == true)
            {
                JsTreeNode rs = new JsTreeNode();
                rs.data.title = obj.Name;
                rs.data.icon = this.Url.ImageList(obj.IconKey);

                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                IImageListService imageList = BusinessEngineContext.Current.GetRequiredService<IImageListService>();
                BusinessClass @class = oms.FindBusinessClass(obj.ClassId);

                var assns = (from assn in @class.AllLeftAssociations()
                             where assn.RightNavigatable = true
                             
                             select new { Class = assn.RightClass, Text = assn.RightRoleName, Property=assn.RightRoleCode, Order = assn.RightRoleDisplayOrder, IsScalar = (new Cardinality(assn.RightCardinality)).IsSingleton  })
                            .Union(from assn in @class.AllRightAssociations()
                                   where assn.LeftNavigatable
                                   select new {Class = assn.LeftClass, Text = assn.LeftRoleName, Property = assn.LeftRoleCode, Order = assn.LeftRoleDisplayOrder, IsScalar = (new Cardinality(assn.LeftCardinality)).IsSingleton  }).OrderBy(x => x.Order);

                foreach (var v in assns)
                {

                    if (security.Properties[v.Property].CanRead == true)
                    {

                        JsTreeNode folderItem = new JsTreeNode();
                        folderItem.data.title = v.Property;


                        string imageKey = imageList.GetFolderKey(oms.GetImageKey(v.Class));



                        folderItem.data.icon = this.Url.ImageList(imageKey);
                       
                        rs.children.Add(folderItem);
                        if (childDeep > 0)
                        {
                           
                            if(v.IsScalar)
                            {
                                BusinessObject child = (BusinessObject)obj.GetType().GetProperty(v.Property).GetValue(obj, null);
                                if (child != null)
                                {
                                    folderItem.children.Add(CreateTreeItem(child, childDeep - 1));
                                }
                            }
                            else
                            {
                                IEnumerable childItems = (IEnumerable)obj.GetType().GetProperty(v.Property).GetValue(obj, null);
                                foreach(BusinessObject child in childItems)
                                {
                                    folderItem.children.Add(CreateTreeItem(child, childDeep - 1));
                                }
                            }
                            
                        }
                    }
                    
                }

                return rs; 
            }
            else
            {
                return null;
            }
           


        }

        #endregion
    }
}