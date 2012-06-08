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

        public BooleanEditorFactory Display(string trueText, string falseText, string nullText)
        {
            ((BooleanEditor)this.Control).TrueText = trueText;
            ((BooleanEditor)this.Control).FalseText = falseText;
            ((BooleanEditor)this.Control).NullText = nullText;
            return this;
        }

        public BooleanEditorFactory ShowText(bool value)
        {
            ((BooleanEditor)this.Control).ShowText = value;
            return this;
        }


      
    }
}