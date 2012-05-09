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
    partial class BooleanEditor
    {
       
        public string TrueText { get; internal set; }

        public string FalseText { get; internal set; }


        protected override void PreExecute()
        {

            this.AddClass("boolean-editor");

            this.MergeAttribute("id", this.GenerateId());
            this.MergeAttribute("type", "checkbox");

            if (this.Value)
            {
                this.MergeAttribute("checked", "checked");
            }

            else
            {
                this.RemoveAttribute("checked");
            }
            base.PreExecute();
        }

        private bool Value
        {
            get
            {
                object value = this.PropertyDescriptor.Value;
                return value != null && (bool)value == true;
            }
        }
    }


   
}