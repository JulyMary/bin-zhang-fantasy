using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;
using System.Reflection;

namespace Fantasy.Studio.BusinessEngine
{
    public class EntityCodeNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            dynamic entity = component;
            string name = (string)value;
            string ocname = UniqueNameGenerator.GetCodeName((string)entity.Name);
            entity.Name = name;
            string cname = UniqueNameGenerator.GetCodeName(name);
            if ((string)entity.CodeName == ocname)
            {
                entity.CodeName = cname;
            }

        }

        #endregion
    }
}
