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
        public EntityCodeNameSetAction()
        {
            this.Suffix = string.Empty;
        }

        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            dynamic entity = component;
            string name = (string)value;
            string ocname = UniqueNameGenerator.GetCodeName((string)entity.Name) + Suffix;
            entity.Name = name;
           
            if ((string)entity.CodeName == ocname)
            {
                string cname = UniqueNameGenerator.GetCodeName(name) + Suffix;
                entity.CodeName = cname;
            }

        }

        public string Suffix { get; set; }

        #endregion
    }
}
