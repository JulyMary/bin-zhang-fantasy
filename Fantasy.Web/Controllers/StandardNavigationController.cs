
using System;
using System.Web.Mvc;
using System.ComponentModel;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Security;
using Fantasy.Web.Models;
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
            model.RootTreeItem = this.CreateTreeItem(entryObject);

            
            

            return View();
        }

        private TreeItem CreateTreeItem(BusinessObject entryObject)
        {
            BusinessApplication application = BusinessEngineContext.Current.Application;
            BusinessObjectSecurity security = application.GetObjectSecurity(entryObject);

            if(security.Properties


        }

        #endregion
    }
}