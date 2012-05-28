using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Web.Mvc
{
    public class BusinessObjectModelBinder : IModelBinder
    {
        #region IModelBinder Members

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {

            BusinessObject model = (BusinessObject)bindingContext.Model;

            if (model == null)
            {

                model = TryCreateModel(bindingContext, model);
            }

            if (model != null)
            {
                BindModel(bindingContext, model);
            }

            return model;

        }

        private void BindModel(ModelBindingContext bindingContext, BusinessObject model)
        {
            BusinessObjectDescriptor descriptor = new BusinessObjectDescriptor(model);

            foreach (BusinessPropertyDescriptor prop in descriptor.Properties)
            {
                if (prop.CanWrite)
                {
                    ValueProviderResult vps = bindingContext.ValueProvider.GetValue(prop.CodeName);
                    if (vps != null)
                    {
                        string str = (string)vps.AttemptedValue;

                        //TODO: Add converter for property;

                        if (str != string.Empty)
                        {
                            object value;
                            if (prop.PropertyType.IsSubclassOf(typeof(BusinessObject)))
                            {
                                Guid id = new Guid(str);
                                value = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(prop.PropertyType, id);

                            }
                            else
                            {
                                value = Convert.ChangeType(str, prop.PropertyType);
                            }

                            prop.Value = value;
                        }
                        else
                        {
                            if (prop.PropertyType == typeof(string))
                            {
                                prop.Value = string.Empty;
                            }
                            else if ((prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)) || prop.PropertyType.IsByRef)
                            {
                                prop.Value = null;
                            }
                            else
                            {
                                //Force throw invalidcast exception
                                prop.Value = Convert.ChangeType(string.Empty, prop.PropertyType);
                            }
                        }


                    }
                }
            }
        }

        private  BusinessObject TryCreateModel(ModelBindingContext bindingContext, BusinessObject model)
        {
            if (bindingContext.ValueProvider.GetValue("Id") != null)
            {

                Guid id = new Guid(bindingContext.ValueProvider.GetValue("Id").AttemptedValue);

                if (bindingContext.ValueProvider.GetValue("ClassId") != null)
                {
                    Guid classId = new Guid(bindingContext.ValueProvider.GetValue("ClassId").AttemptedValue);
                    BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(classId);

                    model = (BusinessObject)BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(@class.EntityType(), id);
                }
                else
                {
                    model = (BusinessObject)BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(bindingContext.ModelType, id);
                }
            }
            return model;
        }

        #endregion
    }
}