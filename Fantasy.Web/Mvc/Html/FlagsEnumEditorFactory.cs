using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Properties;

namespace Fantasy.Web.Mvc.Html
{
    public class FlagsEnumEditorFactory : UserControlFactory<FlagsEnumEditor>
    {

        public FlagsEnumEditorFactory(HtmlHelper htmlHelper)
            : base(htmlHelper)
        {

        }

        public FlagsEnumEditorFactory NullText(string nullText)
        {
            if (string.IsNullOrEmpty(nullText))
            {
                throw new ArgumentException(String.Format(Resources.ArgumentNullOrEmptyStringText, "nullText"), "nullText");
            }

            ((FlagsEnumEditor)this.Control).OptionsCaption = nullText;
            return this;
        }
    }
}