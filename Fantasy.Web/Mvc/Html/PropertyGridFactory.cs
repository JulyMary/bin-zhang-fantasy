using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class PropertyGridFactory : UserControlFactory<PropertyGrid>
    {
        public PropertyGridFactory(HtmlHelper helper)
            : base(helper)
        {

        }




        public PropertyGridFactory SortBy(PropertySort sort)
        {
            this.Control.Sort = sort;
            return this;
        }

       

    }


    public static class PropertyGridExensions
    {
        public static PropertyGridFactory PropertyGrid(this HtmlHelper htmlHelper)
        {
            return new PropertyGridFactory(htmlHelper);
        }
    }
}