using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.Web.Mvc.Html;
using Fantasy.XSerialization;
using Fantasy.Web.Properties;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("TextEditor", NamespaceUri = Consts.ExtensionsNamespace)]
    public class TextEditorExtension : EditorExtension
    {
         public TextEditorExtension()
         {
             this.Name = Resources.TextEditorName;
             this.Description = Resources.TextEditorDescription;
         }

        public override UserControlFactory CreateEditor(HtmlHelper htmlHelper)
        {
            return new UserControlFactory<TextEditor>(htmlHelper);
        }

        public override bool ApplyTo(object context)
        {
            return true;
        }
       
    }
}