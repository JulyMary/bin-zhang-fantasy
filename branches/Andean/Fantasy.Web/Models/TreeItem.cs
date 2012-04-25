using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Models
{
    public class TreeItem
    {


        public TreeItem()
        {
            this.ChildItems = new List<TreeItem>();
        }

        public string Text { get; set; }

        public string Url { get; set; }


        public string Icon { get; set; }

        List<TreeItem> ChildItems { get; private set; }
    }
}