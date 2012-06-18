using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.Html
{
    partial class TextArea
    {

        protected override void PreExecute()
        {
            this.AddClass("textarea-editor");
            base.PreExecute();
        }

        public int? Columns { get; set; }

        public int? Rows { get; set; }

        
    }
}