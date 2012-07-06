using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Security;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Security;
using Fantasy.BusinessEngine.Services;
using Fantasy.Web.Models;
using Fantasy.Web.Mvc;
using Fantasy.Web.Mvc.Html;
using Fantasy.Web.Properties;
using System.Web.Mvc.Ajax;
using System.Web;
using Fantasy.ComponentModel;
using System.Xml.Linq;
using Fantasy.XSerialization;
namespace Fantasy.Web.Controllers
{

    [NavigationView(EnableCustomizeDetailView=true),
    ResourceCaption(typeof(Resources), "StandardNavigationName")]
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


            model.ObjId = objId ?? entryObject.Id;



            model.RootTreeItem.state = JsTreeNode.Open;

            return View(model);

        }

        public JsonResult LoadChildren(Guid objId, string property)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();

            BusinessObject parent = es.Get<BusinessObject>(objId);
            List<JsTreeNode> model = new List<JsTreeNode>();
            if (parent != null)
            {
                BusinessApplication application = BusinessEngineContext.Current.Application;

                BusinessObjectSecurity security = application.GetObjectSecurity(parent);


                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                BusinessClass @class = oms.FindBusinessClass(parent.ClassId);




                if (security.Properties[property].CanRead == true)
                {

                    var lefts = from assn in @class.AllLeftAssociations()
                                where string.Equals(property, assn.RightRoleCode, StringComparison.OrdinalIgnoreCase)
                                select
                                    new { IsScalar = (new Cardinality(assn.RightCardinality)).IsScalar };
                    var desc = lefts.SingleOrDefault();

                    if (desc == null)
                    {
                        var rights = from assn in @class.AllRightAssociations()
                                     where string.Equals(property, assn.LeftRoleCode, StringComparison.OrdinalIgnoreCase)
                                     select new { IsScalar = (new Cardinality(assn.LeftCardinality)).IsScalar };

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
            }



            return Json(model, JsonRequestBehavior.AllowGet);

        }


        public JsonResult SwitchDisplayOrder(Guid objId, Guid other)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();

            BusinessObject obj = es.Load<BusinessObject>(objId);
            BusinessObject _other = es.Load<BusinessObject>(other);

            BusinessObjectDescriptor d1 = new BusinessObjectDescriptor(obj);
            BusinessObjectDescriptor d2 = new BusinessObjectDescriptor(_other);

            if (d1.Properties["DisplayOrder"].CanWrite && d2.Properties["DisplayOrder"].CanWrite)
            {
                long temp = obj.DisplayOrder;
                obj.DisplayOrder = _other.DisplayOrder;
                _other.DisplayOrder = temp;

                es.BeginUpdate();
                try
                {
                    es.SaveOrUpdate(obj);
                    es.SaveOrUpdate(_other);
                    es.EndUpdate(true);
                }
                catch
                {
                    es.EndUpdate(false);
                    throw;
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                throw new OperationFobiddenException();
            }



        }

        private static readonly BindingFlags _bindingFlags = System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

        private bool ShowAssocation(BusinessPropertyDescriptor prop)
        {
            bool rs = true;
            if (this._settings.Customized)
            {
                StandardNavigationViewClassSettings clsSettings = this._settings.ClassSettings.FirstOrDefault(s => s.ClassId == prop.Owner.Object.ClassId);
                if (clsSettings != null && clsSettings.Customized)
                {
                    rs = !clsSettings.DisabledChildRoles.Any(s => string.Equals(s, prop.CodeName, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                if (prop.IsScalar)
                {
                    Cardinality card;
                    if (prop.MemberType == BusinessObjectMemberTypes.LeftAssociation)
                    {
                        if (prop.Association.LeftNavigatable)
                        {
                            card = new Cardinality(prop.Association.LeftCardinality);
                            rs = card.IsScalar;
                        }

                        
                    }
                    else
                    {

                        if (prop.Association.RightNavigatable)
                        {
                            card = new Cardinality(prop.Association.RightCardinality);
                            rs = card.IsScalar;
                        }
                       
                    }
                     
                }
            }
            return rs;
        }

        internal JsTreeNode CreateTreeItem(BusinessObject obj, int childDeep)
        {

            BusinessObjectDescriptor descriptor = new BusinessObjectDescriptor(obj);


            if (descriptor.Properties["Name"].CanRead == true)
            {
                JsTreeNode rs = new JsTreeNode();
                rs.data.title = "<span data-bind=\"text:Name\"></span>";
                rs.data.icon = this.Url.ImageList(obj.IconKey);
                string id = "navigationTree" + obj.Id.ToString();
                IDictionary<string, object> attr = Url.GetSclarViewLinkAttributes(obj.Id, ajaxOptions: new AjaxOptions()
                    {
                        UpdateTargetId = "contentpanel",
                        OnBegin = "stdnav.confirmSaving"
                    });

                attr.Add("id", id);
                rs.data.attr = attr;

                rs.metadata = new { entity = new { Id = obj.Id, Name = obj.Name }, contextmenu = this.CreateEntityContextMenu(descriptor) };

                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                IImageListService imageList = BusinessEngineContext.Current.GetRequiredService<IImageListService>();
                BusinessClass @class = oms.FindBusinessClass(obj.ClassId);


                var props = from prop in descriptor.Properties
                            where prop.CanRead && (prop.MemberType == BusinessObjectMemberTypes.LeftAssociation || prop.MemberType == BusinessObjectMemberTypes.RightAssociation)
                                && ShowAssocation(prop)
                            select prop;

                foreach (BusinessPropertyDescriptor prop in props)
                {
                    JsTreeNode folderItem = new JsTreeNode();
                    folderItem.data.title = prop.Name;
                    folderItem.data.attr = new {id=id + prop.CodeName };
                    folderItem.metadata = new { url = this.Url.ApplicationUrl(objectId: obj.Id, action: "LoadChildren", property: prop.CodeName),
                    contextmenu=this.CreateFolderContextMenu(prop)};

                    string imageKey = imageList.GetFolderKey(oms.GetImageKey(prop.ReferencedClass));

                    folderItem.data.icon = this.Url.ImageList(imageKey);

                    rs.children.Add(folderItem);

                    if (BusinessEngineContext.Current.Application.GetViewType(obj, prop.CodeName) == ViewType.Obj)
                    {

                        if (childDeep > 0)
                        {

                            IEnumerable childItems;
                            if (prop.IsScalar)
                            {
                                BusinessObject child = (BusinessObject)prop.Value;
                                childItems = child != null ? new BusinessObject[] { child } : new BusinessObject[0];

                            }
                            else
                            {
                                childItems = (IEnumerable)prop.Value;

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

                return rs;
            }
            else
            {
                return null;
            }

        }


        private object CreateEntityContextMenu(BusinessObjectDescriptor desc)
        {
            if (desc.Properties["DisplayOrder"].CanRead)
            {
                Dictionary<string, object> items = new Dictionary<string, object>();
                items.Add("moveup", new { name = "Move Up", callback = "stdnav.moveNode" });
                items.Add("movedown", new { name = "Move Down", callback = "stdnav.moveNode" });
                string url = this.Url.ApplicationUrl(action: "SwitchDisplayOrder");
                return new { items = items, events = new { show = "stdnav.entityContextMenuShow" }, url=url };
            }
            else
            {
                return null;
            }
        }

        private object CreateFolderContextMenu(BusinessPropertyDescriptor prop)
        {
            Dictionary<string, object> items = new Dictionary<string, object>();
            
            if (prop.CanWrite)
            {
                AjaxOptions ao = new AjaxOptions(){UpdateTargetId = "contentpanel"};
                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                BusinessApplication app = BusinessEngineContext.Current.Application;
                var classes = from cls in prop.ReferencedClass.Flatten(c => c.ChildClasses)
                              where app.GetClassSecurity(cls).CanCreate == true
                              select cls;

                foreach (BusinessClass @class in classes)
                {
                    string href = this.Url.ApplicationUrl(objectId: prop.Owner.Object.Id, action: "create", property: prop.CodeName, routeValues: new { classId = @class.Id });
                    IDictionary<string, object> attr = ao.ToUnobtrusiveHtmlAttributes();
                    string name = string.Format(Resources.StandardNavigationAddChildText, @class.Name);
                   
                    string icon = this.Url.ImageList(oms.GetImageKey(@class));
                    items.Add(@class.CodeName, new {type="anchor",icon=icon, href= href, name=name, attr = attr }); 
                }
            }
            if (items.Count > 0)
            {
                items.Add("separator1", "-----");
            }
            items.Add("refresh", new { name = "Refresh", callback="stdnav.refreshNode"}); 

            if (items.Count > 0)
            {
                return new { items = items };
            }
            else
            {
                return null;
            }
        }


        private StandardNavigationViewSettings _settings;

        public void LoadSettings(XElement settings)
        {
            XSerializer ser = new XSerializer(typeof(StandardNavigationViewSettings));
            this._settings = (StandardNavigationViewSettings)ser.Deserialize(settings);
        }


        public ViewResultBase Create(Guid objId, string property, Guid classId)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();

            object model = new
            {
                Parent = es.Get<BusinessObject>(objId),
                Property = property,
                ClassId = classId
            };

            return PartialView(model);
 
        }


    }
}