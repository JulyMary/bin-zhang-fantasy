using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Models
{
   

    public class MenuItemModel
    {

        public MenuItemModel()
        {
            this.ChildItems = new List<MenuItemModel>();
        }

        public string Name { get; set; }

        public string Url { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public List<MenuItemModel> ChildItems { get; private set; }
    }
}