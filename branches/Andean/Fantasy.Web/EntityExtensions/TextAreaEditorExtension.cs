using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.XSerialization;
using Fantasy.Web.Mvc.Html;
using Fantasy.ComponentModel;
using Fantasy.Web.Properties;

namespace Fantasy.Web.EntityExtensions
{

    [XSerializable("TextArea", NamespaceUri = Consts.ExtensionsNamespace)]
    [Icon(typeof(Resources), "TextArea")]
    public class TextAreaEditorExtension : EditorExtension
    {

        [XAttribute("columns")]
        private int? _rows;

        public int? Rows
        {
            get { return _rows; }
            set
            {
                if (_rows != value)
                {
                    _rows = value;
                    this.OnPropertyChanged("Rows");
                }
            }
        }

        [XAttribute("rows")]
        private int? _columns;

        public int? Columns
        {
            get { return _columns; }
            set
            {
                if (_columns != value)
                {
                    _columns = value;
                    this.OnPropertyChanged("Columns");
                }
            }
        }

        public override Mvc.Html.UserControlFactory CreateEditor(System.Web.Mvc.HtmlHelper htmlHelper)
        {
            return new TextAreaFactory(htmlHelper).Columns(this.Columns).Rows(this.Rows);
        }
    }
}