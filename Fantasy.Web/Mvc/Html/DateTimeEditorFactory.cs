using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class DateTimeEditorFactory : UserControlFactory<DateTimeEditor>
    {

        public DateTimeEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

        public DateTimeEditorFactory Format(string format)
        {
            this.Control.Format = format;

            return this;
        }
    }
}