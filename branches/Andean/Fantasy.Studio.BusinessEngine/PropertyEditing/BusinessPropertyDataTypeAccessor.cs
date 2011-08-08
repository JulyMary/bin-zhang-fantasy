using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
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
            else if (prop.DataType.Id == BusinessDataType.WellknownIds.Class)
            {
                return prop.DataClassType; 
            }
            else if (prop.DataType.Id == BusinessDataType.WellknownIds.Enum)
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
                prop.FieldType = rep.Guid.DefaultDatabaseType;
                prop.Length = rep.Guid.DefaultLength;
                prop.Precision = rep.Guid.DefaultPrecision; 

            }
            else if (entity is BusinessEnum)
            {
                BusinessEnum @enum = (BusinessEnum)entity;

                prop.DataType = rep.Enum;
                prop.DataClassType = null;
                prop.DataEnumType = @enum;
                prop.FieldType = rep.Int32.DefaultDatabaseType;
                prop.Length = rep.Int32.DefaultLength;
                prop.Precision = rep.Int32.DefaultPrecision; 
                

            }
            else
            {
                prop.DataType = (BusinessDataType)entity;
                prop.DataClassType = null;
                prop.DataEnumType = null;
                prop.FieldType = prop.DataType.DefaultDatabaseType;
                prop.Length = prop.DataType.DefaultLength;
                prop.Precision = prop.DataType.DefaultPrecision; 

            }
        }

        #endregion
    }
}
