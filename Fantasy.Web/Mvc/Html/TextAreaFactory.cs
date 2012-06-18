using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{
    public class TextAreaFactory : UserControlFactory<TextArea>
    {

        public TextAreaFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {
        }
            public TextAreaFactory Columns(int? Columns)
            {
                ((TextArea)this.Control).Columns = Columns;
                return this;
            }

            public TextAreaFactory Rows(int? rows)
            {
                ((TextArea)this.Control).Rows = rows;
                return this;
            }

        }
    
}