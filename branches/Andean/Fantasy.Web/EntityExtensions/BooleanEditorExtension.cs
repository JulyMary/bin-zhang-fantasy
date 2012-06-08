using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.XSerialization;
using Fantasy.ComponentModel;
using Fantasy.Web.Properties;
using Fantasy.Web.Mvc.Html;

namespace Fantasy.Web.EntityExtensions
{
    [XSerializable("BooleanEditor", NamespaceUri = Consts.ExtensionsNamespace)]
    [Icon(typeof(Resources), "CheckBox")]
    public class BooleanEditorExtension : EditorExtension
    {
        public BooleanEditorExtension()
        {
            this.Name = Resources.BooleanEditorName;
            this.Description = Resources.BooleanEditorDescription;
            this._trueText = Resources.BooleanEditorDefaultTrueText;
            this._falseText = Resources.BooleanEditorDefaultFalseText;
            this._nullText = Resources.BooleanEditorDefaultNullText;
        }

        public override Mvc.Html.UserControlFactory CreateEditor(System.Web.Mvc.HtmlHelper htmlHelper)
        {
            return (new BooleanEditorFactory(htmlHelper)).Display(this.TrueText, this.FalseText, this.NullText).ShowText(this.ShowText);
        }

        [XAttribute("trueText")]
        private string _trueText;

        public string TrueText
        {
            get { return _trueText; }
            set
            {
                if (_trueText != value)
                {
                    _trueText = value;
                    this.OnPropertyChanged("TrueText");
                }
            }
        }

        [XAttribute("falseText")]
        private string _falseText;

        public string FalseText
        {
            get { return _falseText; }
            set
            {
                if (_falseText != value)
                {
                    _falseText = value;
                    this.OnPropertyChanged("FalseText");
                }
            }
        }

        [XAttribute("nullText")]
        private string _nullText;

        public string NullText
        {
            get { return _nullText; }
            set
            {
                if (_nullText != value)
                {
                    _nullText = value;
                    this.OnPropertyChanged("NullText");
                }
            }
        }

        [XAttribute("showText")]
        private bool _showText = true;

        public bool ShowText
        {
            get { return _showText; }
            set
            {
                if (_showText != value)
                {
                    _showText = value;
                    this.OnPropertyChanged("ShowText");
                }
            }
        }
    }
}