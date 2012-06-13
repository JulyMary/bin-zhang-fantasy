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

                ValueProviderResult r1 = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if (r1 != null)
                {
                    model = r1.RawValue as BusinessObject;
                }

                if (model == null)
                {
                    model = TryCreateModel(bindingContext);
                }
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
                        object value;

                        if (prop.PropertyType.IsSubclassOf(typeof(BusinessObject)))
                        {
                            string str = (string)vps.AttemptedValue;
                            if (!string.IsNullOrEmpty(str))
                            {
                                Guid id = new Guid(str);
                                value = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(prop.PropertyType, id);
                            }
                            else
                            {
                                value = null;
                            }

                        }
                        else
                        {
                            Type t = prop.PropertyType;
                            bool isNullable = false;
                            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                isNullable = true;
                                t = t.GetGenericArguments()[0];
                            }

                            if (t.IsEnum && t.IsDefined(typeof(FlagsAttribute), true))
                            {
                                string s = vps.AttemptedValue;
                                if (!string.IsNullOrEmpty(s))
                                {
                                   
                                    string[] values = s.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                    long v = 0L;
                                    foreach(string st in values)
                                    {
                                        v |= Int64.Parse(st);
                                    }
                                    value = Convert.ChangeType(v, t.GetEnumUnderlyingType());
                                }
                                else
                                {
                                    value = isNullable ? null : Activator.CreateInstance(t);
                                }
                            }
                            else
                            {

                                value = vps.ConvertTo(prop.PropertyType);
                            }
                        }

                        prop.Value = value;
                    }

                }
            }
        }

        private BusinessObject TryCreateModel(ModelBindingContext bindingContext)
        {
            BusinessObject rs = null;

            if (bindingContext.ValueProvider.GetValue("Id") != null)
            {

                Guid id = new Guid(bindingContext.ValueProvider.GetValue("Id").AttemptedValue);

                if (bindingContext.ValueProvider.GetValue("ClassId") != null)
                {
                    Guid classId = new Guid(bindingContext.ValueProvider.GetValue("ClassId").AttemptedValue);
                    BusinessClass @class = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>().FindBusinessClass(classId);

                    rs = (BusinessObject)BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(@class.RuntimeType(), id);
                }
                else
                {
                    rs = (BusinessObject)BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get(bindingContext.ModelType, id);
                }
            }
            return rs;
        }

        #endregion
    }
}