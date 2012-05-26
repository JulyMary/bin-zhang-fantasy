using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Events;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine
{
    public class CategoryNameListener : ObjectWithSite, IEntityPropertyChangedEventHandler 
    {
        #region IEntityPropertyChangedEventHandler Members

        public void Execute(Fantasy.BusinessEngine.EntityPropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ExtensionsData")
            {
                if (e.Entity is BusinessProperty)
                {
                    Category cate = ((BusinessProperty)e.Entity).Extensions.OfType<Category>().SingleOrDefault();
                    if (cate != null && ! string.IsNullOrEmpty(cate.Value))
                    {
                        this.Site.GetRequiredService<CategoryNameProvider>().AddCategory(cate.Value);
                    }
                }
                else if (e.Entity is BusinessAssociation)
                {
                    BusinessAssociation assn = (BusinessAssociation)e.Entity;

                    foreach (Category cate in assn.LeftExtensions.Union(assn.RightExtensions).OfType<Category>())
                    {
                        if (!string.IsNullOrEmpty(cate.Value))
                        {
                            this.Site.GetRequiredService<CategoryNameProvider>().AddCategory(cate.Value);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
