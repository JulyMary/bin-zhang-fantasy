using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Models
{
    public class StandardNavigationDefaultViewModel
    {
        public TreeItem RootTreeItem { get; set; }

        public BusinessObject EntryObject {get;set;}
    }
}