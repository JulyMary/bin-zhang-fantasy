using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Properties;

namespace Fantasy.Web.Mvc.Html
{
    public class EnumEditorFactory : UserControlFactory<EnumEditor>
    {
        public EnumEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

        public EnumEditorFactory NullText(string nullText)
        {
            if (string.IsNullOrEmpty(nullText))
            {
                throw new ArgumentException(String.Format(Resources.ArgumentNullOrEmptyStringText, "nullText"), "nullText");
            }

            ((EnumEditor)this.Control).OptionsCaption = nullText;
            return this;
        }

    }
}