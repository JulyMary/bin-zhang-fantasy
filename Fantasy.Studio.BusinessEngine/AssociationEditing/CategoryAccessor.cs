using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.AssociationEditing
{
    public class AssociationLeftCategoryGetter : IGetAction
    {
        #region IGetAction Members

        public object Run(object component, string property)
        {
            BusinessAssociation ba = (BusinessAssociation)component;

            Category cate = ba.LeftExtensions.OfType<Category>().FirstOrDefault();

            return cate != null ? cate.Value : null;
        }

        #endregion
    }

    public class AssociationRightCategoryGetter : IGetAction
    {
        public object Run(object component, string property)
        {
            BusinessAssociation ba = (BusinessAssociation)component;

            Category cate = ba.RightExtensions.OfType<Category>().FirstOrDefault();

            return cate != null ? cate.Value : null;
        }
    }

    public class AssociationLeftCategorySetter : ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessAssociation ba = (BusinessAssociation)component;

            Category cate = ba.LeftExtensions.OfType<Category>().FirstOrDefault();
            if (cate == null)
            {
                cate = new Category() { Value = (string)value };
                ba.LeftExtensions.Add(cate);
            }
            else
            {
                cate.Value = (string)value;
            }
        }

        #endregion
    }


    public class AssociationRightCategorySetter : ISetAction
    {
        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessAssociation ba = (BusinessAssociation)component;

            Category cate = ba.RightExtensions.OfType<Category>().FirstOrDefault();
            if (cate == null)
            {
                cate = new Category() { Value = (string)value };
                ba.RightExtensions.Add(cate);
            }
            else
            {
                cate.Value = (string)value;
            }
        }

        #endregion
    }

}
