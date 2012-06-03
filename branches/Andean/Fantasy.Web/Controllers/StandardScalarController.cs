using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.Web.Properties;
using System.Collections;
using System.Reflection;

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

        public ViewResultBase Create(Guid parentId, string property, Guid classId)
        {

            IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
            IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
            BusinessObject parent = es.Get<BusinessObject>(parentId);

            BusinessObjectDescriptor parentDesc = new BusinessObjectDescriptor(parent);

            if (parentDesc.Properties[property].CanWrite == true)
            {
                BusinessClass @class = oms.FindBusinessClass(classId);
                BusinessObjectDescriptor childDesc = new BusinessObjectDescriptor(@class);
                if (childDesc.CanCreate)
                {
                    BusinessObject child = (BusinessObject)es.CreateEntity(@class.EntityType());
                    if (String.IsNullOrEmpty(child.Name))
                    {
                        string name = string.Format(Resources.NewBusinessObjectName, @class.Name);
                        if (!parentDesc.Properties[property].IsScalar)
                        {
                            IList siblings = Invoker.Invoke<IList>(parent, property);
                            name = Utils.UniqueNameGenerator.GetName(name, siblings.Cast<BusinessObject>().Select(s => s.Name));
                        }

                        child.Name = name;
                    }

                    parent.Append(property, child);

                    return PartialView(child);
                }


            }
           
            throw new OperationFobiddenException();
            
        }


        [HttpPost]
        public ViewResultBase Create(Guid objId, Guid classId)
        {
            IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
            BusinessClass @class = oms.FindBusinessClass(classId);

            BusinessObjectDescriptor desc = new BusinessObjectDescriptor(@class);

            if (desc.CanCreate)
            {

                IEntityService es = BusinessEngineContext.Current.GetRequiredService<IEntityService>();
                BusinessObject entity = (BusinessObject)es.CreateEntity(@class.EntityType(), objId);
                this.TryUpdateModel(entity);

                es.BeginUpdate();
                try
                {
                    es.SaveOrUpdate(entity);
                    es.EndUpdate(true);
                }
                catch
                {
                    es.EndUpdate(false);
                    throw;
                }

                return PartialView("Default", entity);


            }
            else
            {
                throw new OperationFobiddenException();
            }

        }

       
       
    }
}