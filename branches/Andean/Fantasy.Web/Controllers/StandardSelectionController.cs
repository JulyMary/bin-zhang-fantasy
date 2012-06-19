using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using Fantasy.Web.Properties;
using System.Collections;
using Fantasy.Web.Models;
using System.Reflection;
using Fantasy.BusinessEngine.Security;
using Fantasy.Web.Mvc.Html;
using Fantasy.Web.Mvc;
using System.Web.Mvc.Ajax;
namespace Fantasy.Web.Controllers
{
    public class StandardSelectionController : Controller, INavigationViewController, ICustomerizableViewController
    {
        #region INavigationViewController Members

        public ViewResultBase Default(Guid parentId, string parentProperty)
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObject entryObject = application.EntryObject;

            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();

            BusinessObject parent = es.Load<BusinessObject>(parentId);
            BusinessObjectDescriptor desc = new BusinessObjectDescriptor(parent);

            BusinessClass allowedClass = desc.Properties[parentProperty].ReferencedClass;

            if (allowedClass == null)
            {
                throw new ArgumentException(String.Format(Resources.StdSelInvalidPropertyText, parent.Name, parentProperty));
            }

            JsTreeNode root = this.CreateTreeItem(entryObject, 0);

            return PartialView(root);

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
                        JsTreeNode childNode = CreateTreeItem(child, 1);
                        if (childNode != null)
                        {
                            model.Add(childNode);
                        }
                    }
                }
            }



            return Json(model, JsonRequestBehavior.AllowGet);

        }

        private static readonly BindingFlags _bindingFlags = System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;

        private bool ShowAssocation(BusinessPropertyDescriptor prop)
        {
            bool rs = true;
            //if (this._settings.Customized)
            //{
            //    StandardNavigationViewClassSettings clsSettings = this._settings.ClassSettings.FirstOrDefault(s => s.ClassId == prop.Owner.Object.ClassId);
            //    if (clsSettings != null && clsSettings.Customized)
            //    {
            //        rs = !clsSettings.DisabledChildRoles.Any(s => string.Equals(s, prop.CodeName, StringComparison.OrdinalIgnoreCase));
            //    }
            //}
            //else
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
                rs.data.title = obj.Name;
                rs.data.icon = this.Url.ImageList(obj.IconKey);
                

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
                  
                    folderItem.metadata = new
                    {
                        url = this.Url.ApplicationUrl(objectId: obj.Id, action: "LoadChildren", property: prop.CodeName),
                    };

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
                   
                }

                return rs;
            }
            else
            {
                return null;
            }

        }

        public void LoadSettings(object settings)
        {
           
        }

        #endregion
    }
}