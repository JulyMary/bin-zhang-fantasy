using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.XSerialization;
using Fantasy.Web.Properties;
using Fantasy.ComponentModel;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("ClassIdEditor", NamespaceUri = Consts.ExtensionsNamespace)]
    [Icon(typeof(Resources), "ClassIdEditor")]
    public class ClassIdEditorExtension : EditorExtension
    {
        public ClassIdEditorExtension()
        {
            this.Name = Resources.ClassIdEditorName;
            this.Description = Resources.ClassIdEditorDescription;
        }

        public override Mvc.Html.UserControlFactory CreateEditor(System.Web.Mvc.HtmlHelper htmlHelper)
        {
            return new Mvc.Html.UserControlFactory<Fantasy.Web.Mvc.Html.ClassIdEditor>(htmlHelper);
        }

        
    }
}