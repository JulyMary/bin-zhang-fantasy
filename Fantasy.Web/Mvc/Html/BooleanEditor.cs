using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Fantasy.Web;
using Fantasy.Web.Mvc;
using Fantasy.Web.Properties;

namespace Fantasy.Web.Mvc.Html
{
    partial class BooleanEditor
    {
       

        private string _trueText = Resources.BooleanEditorDefaultTrueText;

        public string TrueText
        {
            get { return _trueText; }
            set { _trueText = value; }
        }


        private string _falseText = Resources.BooleanEditorDefaultFalseText;

        public string FalseText
        {
            get { return _falseText; }
            set { _falseText = value; }
        }


        private string _nullText = Resources.BooleanEditorDefaultNullText;

        public string NullText
        {
            get { return _nullText; }
            set { _nullText = value; }
        }

        private bool _showText = true;

        public bool ShowText
        {
            get { return _showText; }
            set { _showText = value; }
        }



      
       

        private bool Value
        {
            get
            {
                object value = this.PropertyDescriptor.Value;
                return value != null && (bool)value == true;
            }
        }
    }


   
}