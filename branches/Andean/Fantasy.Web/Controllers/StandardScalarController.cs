using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Controllers
{
    
    public class StandardScalarController : Controller, IScalarViewController//, ICustomerizableViewController
    {
      

        public void LoadSettings(object settings)
        {
            //throw new NotImplementedException();
        }

       

        public ViewResultBase Default(Guid objId)
        {
            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
            BusinessObject obj = es.Get<BusinessObject>(objId);

            return PartialView(obj);
        }


        public ViewResultBase Save(Guid objId)
        {

            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
            BusinessObject obj = es.Get<BusinessObject>(objId);
            if (obj != null)
            {
                this.TryUpdateModel(obj);
                es.BeginUpdate();
                try
                {
                    es.SaveOrUpdate(obj);
                    es.EndUpdate(true);
                }
                catch
                {
                    es.EndUpdate(false);
                    throw;
                }
               
            }


            return PartialView("Default", obj);
        }

        public ViewResultBase Create(BusinessObject obj)
        {
            throw new NotImplementedException();
        }


        [HttpPost]
        public ViewResultBase Create()
        {
            throw new NotImplementedException();
        }

       
    }
}