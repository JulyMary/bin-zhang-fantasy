using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.XSerialization;
using Fantasy.ComponentModel;
using Fantasy.Web.Properties;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("BusinessUserEditor", NamespaceUri = Consts.ExtensionsNamespace)]
    [Icon(typeof(Resources), "UserEditor")]
    public class BusinessUserEditorExtension : EditorExtension
    {
        public BusinessUserEditorExtension()
        {
            this.Name = Resources.BusinessUserEditorName;
            this.Description = Resources.BusinessUserEditorDescription;
        }

        public override Mvc.Html.UserControlFactory CreateEditor(System.Web.Mvc.HtmlHelper htmlHelper)
        {
            return new Mvc.Html.UserControlFactory<Fantasy.Web.Mvc.Html.BusinessUserEditor>(htmlHelper);
        }
    }
}