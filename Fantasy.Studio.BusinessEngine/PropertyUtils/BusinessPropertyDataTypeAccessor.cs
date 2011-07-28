using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine
{
    public class BusinessPropertyDataTypeAccessor : ObjectWithSite, ISetAction, IGetAction
    {

        #region IGetAction Members

        public object Run(object component, string property)
        {
            BusinessProperty prop = (BusinessProperty)component;
            if (prop.DataType == null)
            {
                return null; 
            }
            else if (prop.DataType.IsBusinessClass)
            {
                return prop.DataClassType; 
            }
            else if (prop.DataType.IsEnum)
            {
                return prop.DataEnumType;
            }
            else
            {
                return prop.DataType; 
            }
        }

        #endregion

        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessProperty prop = (BusinessProperty)component;
            IBusinessEntity entity = (IBusinessEntity)value;
            IBusinessDataTypeRepository rep = this.Site.GetRequiredService<IBusinessDataTypeRepository>();
            if (entity is BusinessClass)
            {
                prop.DataType = rep.Class;
                prop.DataClassType = (BusinessClass)entity;
                prop.DataEnumType = null;
            }
            if (entity is BusinessEnum)
            {
                prop.DataType = rep.Enum;
                prop.DataClassType = null;
                prop.DataEnumType = (BusinessEnum)entity;
            }
            else
            {
                prop.DataType = (BusinessDataType)entity;
                prop.DataClassType = null;
                prop.DataEnumType = null;
            }
        }

        #endregion
    }
}
