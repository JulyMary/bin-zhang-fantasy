using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.ClassEditing
{
    public class BusinessClassNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string propertyName, object value)
        {
            BusinessClass cls = component as BusinessClass;
            string name = (string)value;
            if (cls != null)
            {

                string ocname = UniqueNameGenerator.GetCodeName(cls.Name);
                string otname = Settings.Default.DefaultClassTablePrefix + "_" + ocname;
                cls.Name = name;
                string cname = UniqueNameGenerator.GetCodeName(name);
                if (cls.CodeName == ocname)
                {
                    cls.CodeName = cname;
                }
                if (cls.EntityState == EntityState.New && cls.TableName == otname)
                {
                    cls.TableName = Settings.Default.DefaultClassTablePrefix + "_" + cname;
                }

            }

        }

        #endregion
    }
}
