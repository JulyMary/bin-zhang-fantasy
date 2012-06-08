using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    partial class SelectList
    {

        private List<SelectListItem> _items = new List<SelectListItem>();
        public List<SelectListItem> Items
        {
            get
            {
                return _items;
            }
        } 
 

    }
}