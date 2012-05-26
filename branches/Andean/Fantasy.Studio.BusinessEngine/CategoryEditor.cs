using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.Studio.Descriptor;
using Fantasy.BusinessEngine.Services;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.EntityExtensions;

namespace Fantasy.Studio.BusinessEngine.PropertyEditing
{
    public class CategoryEditor : ListBoxDropDownTypeEditor, IObjectWithSite
    {

        public IServiceProvider Site { get; set; }


       

        protected override object[] Items
        {
            get 
            {
                return this.Site.GetRequiredService<CategoryNameProvider>().CategoryNames.ToArray();
            }
        }


       
    }
}
