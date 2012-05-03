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
using Fantasy.Web.Properties;
using System.Collections.Generic;
using System.Reflection;
namespace Fantasy.Web.Controllers
{
    
    [Settings(typeof(StandardNavigationViewSettings))]
    public class StandardNavigationController : Controller, INavigationViewController, ICustomerizableViewController
    {
        public ViewResult Default(Guid? objId)
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObject entryObject = application.EntryObject;

            StandardNavigationDefaultViewModel model = new StandardNavigationDefaultViewModel();
            model.RootTreeItem = this.CreateTreeItem(entryObject, this._settings.Deep - 1);

            if (model == null)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            
            model.RootTreeItem.state = JsTreeNode.Open;
            
            return View(model);
           
        }



       

        public JsonResult LoadChildren(Guid objId, string property)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();

            BusinessObject parent = es.Get<BusinessObject>(objId);
            BusinessApplication application = BusinessEngineContext.Current.Application;

            BusinessObjectSecurity security = application.GetObjectSecurity(parent);


            IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
            BusinessClass @class = oms.FindBusinessClass(parent.ClassId);


            List<JsTreeNode> model = new List<JsTreeNode>();

            if (security.Properties[property].CanRead == true)
            {
               
                var lefts = from assn in @class.AllLeftAssociations() where string.Equals(property, assn.RightRoleCode, StringComparison.OrdinalIgnoreCase) select
                            new  { IsScalar = (new Cardinality(assn.RightCardinality)).IsSingleton };
                var desc = lefts.SingleOrDefault();

                if (desc == null)
                {
                    var rights = from assn in @class.AllRightAssociations() where string.Equals(property, assn.LeftRoleCode, StringComparison.OrdinalIgnoreCase)
                           select  new { IsScalar = (new Cardinality(assn.LeftCardinality)).IsSingleton };

                    desc = rights.Single(string.Format(Resources.PropertyNotFoundMessage, property, @class.FullCodeName));
                }

                IEnumerable children;
                if (desc.IsScalar)
                {
                    BusinessObject child = (BusinessObject)parent.GetType().GetProperty(property, _bindingFlags).GetValue(parent, null);
                    children = child != null ? new BusinessObject[] { child } : new BusinessObject[0];
                }
                else
                {
                    children = (IEnumerable)parent.GetType().GetProperty(property, _bindingFlags).GetValue(parent, null);
                    
                }

                foreach (BusinessObject child in children)
                {
                    JsTreeNode childNode = CreateTreeItem(child, this._settings.Deep - 1);
                    if (childNode != null)
                    {
                        model.Add(childNode);
                    }
                }
            }

            

            return Json(model, JsonRequestBehavior.AllowGet);

        }


        private static readonly BindingFlags _bindingFlags = System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

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
                        folderItem.metadata = new { url = this.Url.ApplicationUrl(objectId: obj.Id, action: "LoadChildren", property:v.Property) };


                        string imageKey = imageList.GetFolderKey(oms.GetImageKey(v.Class));



                        folderItem.data.icon = this.Url.ImageList(imageKey);
                       
                        rs.children.Add(folderItem);

                        if (BusinessEngineContext.Current.Application.GetViewType(obj, v.Property) == ViewType.Obj)
                        {

                            if (childDeep > 0)
                            {

                                IEnumerable childItems;
                                if (v.IsScalar)
                                {
                                    BusinessObject child = (BusinessObject)obj.GetType().GetProperty(v.Property, _bindingFlags).GetValue(obj, null);
                                    childItems = child != null ? new BusinessObject[] { child } : new BusinessObject[0];

                                }
                                else
                                {
                                    childItems = (IEnumerable)obj.GetType().GetProperty(v.Property, _bindingFlags).GetValue(obj, null);

                                }

                                foreach (BusinessObject child in childItems)
                                {
                                    JsTreeNode childNode = CreateTreeItem(child, childDeep - 1);
                                    if (childNode != null)
                                    {
                                        folderItem.children.Add(CreateTreeItem(child, childDeep - 1));
                                    }

                                }

                                if (folderItem.children.Count == 0)
                                {
                                    folderItem.state = JsTreeNode.Open;
                                }

                            }
                        }
                        else
                        {
                            //TODO: Add collection view link;
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


        private StandardNavigationViewSettings _settings;

        public void LoadSettings(object settings)
        {
            this._settings = (StandardNavigationViewSettings)settings;
        }

      
    }
}