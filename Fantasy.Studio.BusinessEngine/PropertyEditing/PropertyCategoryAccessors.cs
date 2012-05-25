using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class PropertyCategoryGetter : IGetAction
    {
        #region IGetAction Members

        public object Run(object component, string property)
        {
            BusinessProperty bp = (BusinessProperty)component;

            Category cate = bp.Extensions.OfType<Category>().FirstOrDefault();

            return cate != null ? cate.Value : null;

        }

        #endregion
    }

    public class PropertyCategorySetter : ISetAction
    {

        #region ISetAction Members

        public void Run(object component, string property, object value)
        {
            BusinessProperty bp = (BusinessProperty)component;

            Category cate = bp.Extensions.OfType<Category>().FirstOrDefault();
            if (cate == null)
            {
                cate = new Category() { Value = (string)value };
                bp.Extensions.Add(cate);
            }
            else
            {
                cate.Value = (string)value;
            }

        }

        #endregion
    }
}
