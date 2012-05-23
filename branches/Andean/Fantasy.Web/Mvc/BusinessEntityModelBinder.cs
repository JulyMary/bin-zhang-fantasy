using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web.Mvc
{
    public class BusinessEntityModelBinder : IModelBinder
    {
        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            BusinessObject model = (BusinessObject)bindingContext.Model;

            if (model == null)
            {

                Guid id = new Guid(bindingContext.ValueProvider.GetValue("Id").AttemptedValue);
                Guid classId = new Guid(bindingContext.ValueProvider.GetValue("ClassId").AttemptedValue);
                BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(classId);

                model = (BusinessObject)BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(@class.EntityType(), id);
               
            }
            BusinessObjectDescriptor descriptor = new BusinessObjectDescriptor(model);

            return model;

        }

        #endregion
    }
}