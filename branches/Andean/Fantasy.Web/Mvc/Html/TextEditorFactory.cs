using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Mvc.Html
{
    public class TextEditorFactory : UserControlFactory<TextEditor>
    {

        public TextEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {
             
        }

     

      
    }
}