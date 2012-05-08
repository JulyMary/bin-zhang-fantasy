using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fantasy.Web.Mvc.Html
{
    public class BooleanEditorFactory : UserControlFactory<BooleanEditor>
    {
        public BooleanEditorFactory Display(string trueText, string falseText)
        {
            this.Control.TrueText = trueText;
            this.Control.FalseText = falseText;
            return this;
        }


      
    }
}