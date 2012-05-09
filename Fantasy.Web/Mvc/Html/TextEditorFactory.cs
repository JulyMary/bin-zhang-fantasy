using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class TextEditorFactory : UserControlFactory<BooleanEditor>
    {

        public TextEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

     

      
    }
}