using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.Html
{
    partial class TextEditor
    {

        protected override void PreExecute()
        {

            this.AddClass("text-editor");
            
            this.MergeAttribute("type", "text");
            base.PreExecute();
        }

        private string Value
        {
            get
            {
                object value = this.PropertyDescriptor.Value;
                return value != null ? value.ToString() : String.Empty;
            }
        }
    }
}