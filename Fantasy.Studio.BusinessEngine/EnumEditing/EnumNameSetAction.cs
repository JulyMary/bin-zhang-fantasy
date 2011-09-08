using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.EnumEditing
{
    public class EnumNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessEnum @enum = (BusinessEnum)component;
            string name = (string)value;
            string ocname = UniqueNameGenerator.GetCodeName(@enum.Name);
            @enum.Name = name;
            if (!@enum.IsExternal)
            {
                string cname = UniqueNameGenerator.GetCodeName(name);
                if (@enum.CodeName == ocname)
                {
                    @enum.CodeName = cname;
                }
            }
        }

        #endregion
    }
}
