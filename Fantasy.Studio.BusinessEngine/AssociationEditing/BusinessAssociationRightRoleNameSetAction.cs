using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class BusinessAssociationRightRoleNameSetAction : ObjectWithSite, ISetAction
    {

        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessAssociation association = (BusinessAssociation)component;
            string name = (string)value;
            string ocname = UniqueNameGenerator.GetCodeName(association.RightRoleName);
            association.RightRoleName = name;

            if (association.RightRoleCode == ocname)
            {
                string cname = UniqueNameGenerator.GetCodeName(name);
                association.RightRoleCode = cname;
            }
        }

        #endregion
    }
}
