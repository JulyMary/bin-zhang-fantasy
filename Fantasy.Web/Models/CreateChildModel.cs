using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Models
{
    public class CreateChildModel
    {
        public BusinessObject Parent { get; set; }

        public string Property { get; set; }

        public BusinessObject Child { get; set; }

        public JsTreeNode TreeNode { get; set; }
    }
}