
using System.Web.Mvc;
using System.ComponentModel;
namespace Fantasy.Web.Controllers
{
    
    public class StandardNavigationController : Controller, INavigationViewController
    {

        #region INavigationViewController Members

        public ViewResult Root(BusinessEngine.BusinessApplication application)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}