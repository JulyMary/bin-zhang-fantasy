using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class BooleanEditorFactory : UserControlFactory<BooleanEditor>
    {

        public BooleanEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

        public BooleanEditorFactory Display(string trueText, string falseText)
        {
            this.Control.TrueText = trueText;
            this.Control.FalseText = falseText;
            return this;
        }


      
    }
}