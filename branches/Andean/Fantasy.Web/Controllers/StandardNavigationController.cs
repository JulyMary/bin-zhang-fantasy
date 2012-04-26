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
            //BusinessApplication application = BusinessEngineContext.Current.Application;
            //BusinessObject entryObject = application.EntryObject;

            //StandardNavigationDefaultViewModel model = new StandardNavigationDefaultViewModel();
            //model.RootTreeItem = this.CreateTreeItem(entryObject, 0);

            //if (model == null)
            //{
            //    FormsAuthentication.RedirectToLoginPage();
            //}
            
            

            //return View(model);
            return View();
        }

        private TreeItem CreateTreeItem(BusinessObject obj, int childDeep)
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObjectSecurity security = application.GetObjectSecurity(obj);
            if (security.Properties["Name"].CanRead == true)
            {
                TreeItem rs = new TreeItem() { Text = obj.Name, Icon = this.Url.ImageList(obj.IconKey) };
                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                BusinessClass @class = oms.FindBusinessClass(obj.ClassId);

                var assns = (from assn in @class.AllLeftAssociations()
                             where assn.RightNavigatable = true
                             
                             select new { Text = assn.RightRoleName, Property=assn.RightRoleCode, Order = assn.RightRoleDisplayOrder, IsScalar = (new Cardinality(assn.RightCardinality)).IsSingleton  })
                            .Union(from assn in @class.AllRightAssociations()
                                   where assn.LeftNavigatable
                                   select new { Text = assn.LeftRoleName, Property = assn.LeftRoleCode, Order = assn.LeftRoleDisplayOrder, IsScalar = (new Cardinality(assn.LeftCardinality)).IsSingleton  }).OrderBy(x => x.Order);

                foreach (var v in assns)
                {

                    if (security.Properties[v.Property].CanRead == true)
                    {

                        TreeItem folderItem = new TreeItem()
                        {
                            Text = v.Text
                        };

                        if (childDeep > 0)
                        {
                           
                            if(v.IsScalar)
                            {
                                BusinessObject child = (BusinessObject)obj.GetType().GetProperty(v.Property).GetValue(obj, null);
                                rs.ChildItems.Add(CreateTreeItem(child, childDeep - 1));
                            }
                            else
                            {
                                IEnumerable childItems = (IEnumerable)obj.GetType().GetProperty(v.Property).GetValue(obj, null);
                                foreach(BusinessObject child in childItems)
                                {
                                    rs.ChildItems.Add(CreateTreeItem(child, childDeep - 1));
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