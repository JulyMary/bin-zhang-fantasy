using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumValueNameSetAction : ObjectWithSite, ISetAction
    {

        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessEnumValue enumValue = (BusinessEnumValue)component;
            string name = (string)value;
            string ocname = UniqueNameGenerator.GetCodeName(enumValue.Name);
            enumValue.Name = name;
            if (!enumValue.Enum.IsExternal)
            {
                string cname = UniqueNameGenerator.GetCodeName(name);
                if (enumValue.CodeName == ocname)
                {
                    enumValue.CodeName = cname;
                }
            }
       
        }

        #endregion
    }
}
