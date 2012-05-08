using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fantasy.Web;
using Fantasy.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public partial class BooleanEditor
    {
       
        internal string TrueText { get; internal set; }

        internal string FalseText { get; internal set; }


        protected override void RenderEditable()
        {
            base.RenderEditable();
        }

        private string CheckedString
        {
            get
            {
                object value = this.PropertyDescriptor.Value;
                return value != null && (bool)value ? "checked=\"checked\"" : string.Empty;
            }
        }
    }


   
}