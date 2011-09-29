using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.Utils;
using Fantasy.Studio.BusinessEngine.Properties;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class BusinessAssociationNameSetAction : ObjectWithSite, ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string propertyName, object value)
        {
            BusinessAssociation assn = component as BusinessAssociation;
            string name = (string)value;
            if (assn != null)
            {

                string ocname = UniqueNameGenerator.GetCodeName(assn.Name);
                string otname = (Settings.Default.DefaultAssociationTablePrefix + "_" + ocname).ToUpper();
                assn.Name = name;
                string cname = UniqueNameGenerator.GetCodeName(name);
                if (assn.CodeName == ocname)
                {
                    assn.CodeName = cname;
                }
                if (assn.EntityState == EntityState.New && assn.TableName == otname)
                {
                    assn.TableName = (Settings.Default.DefaultClassTablePrefix + "_" + cname).ToUpper();
                }

            }

        }

        #endregion
    }
}
