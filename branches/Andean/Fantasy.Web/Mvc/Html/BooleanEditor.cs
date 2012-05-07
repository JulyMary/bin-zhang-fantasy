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
        public BooleanEditor DisplayText(string trueText, string falseText)
        {
            this.TrueText = trueText;
            this.FalseText = falseText;
            return this;
        }

        public string TrueText { get; private set; }

        public string FalseText { get; private set; }
    }


   
}