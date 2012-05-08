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
       
        public string TrueText { get; internal set; }

        public string FalseText { get; internal set; }


       
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