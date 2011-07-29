using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine
{
    public class BusinessPropertyNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string propertyName, object value)
        {
            BusinessProperty property = component as BusinessProperty;
            string name = UniqueNameGenerator.GetCodeName((string)value);
            if (property != null)
            {
                string cname = UniqueNameGenerator.GetCodeName(property.Name);
                if (property.CodeName == cname)
                {
                    property.CodeName = name;
                }
                if (property.FieldName == cname)
                {
                    property.FieldName = name;
                }
                property.Name = name;
            }

        }

        #endregion
    }
}
