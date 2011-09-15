using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class BusinessPropertyNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string propertyName, object value)
        {
            BusinessProperty property = component as BusinessProperty;
            string name = (string)value;
            if (property != null)
            {
               
                string ocname = UniqueNameGenerator.GetCodeName(property.Name);
                property.Name = name;
                string cname = UniqueNameGenerator.GetCodeName(name);
                if (property.CodeName == ocname)
                {
                    property.CodeName = cname;
                }
                if (property.EntityState == EntityState.New && property.FieldName == ocname.ToUpper())
                {
                    property.FieldName = cname.ToUpper();
                }
                
            }

        }

        #endregion
    }
}
