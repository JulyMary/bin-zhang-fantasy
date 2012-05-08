using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using System.Linq.Expressions;
using System.IO;
using System.Web.Mvc;

namespace Fantasy.Web.Mvc.Html
{

    public class BusinessPropertyEditorBase : UserControl<BusinessObjectRender<BusinessObject>>
    {

        private string _propertyName;
        public string PropertyName 
        {
            get
            {
                return _propertyName;
            }
            internal set
            {
                _propertyName = value;
                this.PropertyDescriptor = this.Model.Descriptor.Properties[value];
            }
        }


        public BusinessPropertyDescriptor PropertyDescriptor { get; private set; }  

        protected void CreateName()
        {
            if (!this.Attributes.ContainsKey("name"))
            {
                this.Attributes.Add("name", this.Model.Descriptor.Object.Id + "_" + this.PropertyName); 
            }
        }


        public override void Execute()
        {
            this.CreateName();

            if (this.PropertyDescriptor.CanWrite)
            {
                this.Render();
            }
            else if (this.PropertyDescriptor.CanRead)
            {
                this.RenderReadOnly();
            }
            else if (this.Model.InvisibleOption == InvisibleOptions.Hidden)
            {
                this.RenderHidden();
            }
            else
            {
                this.RenderCollapsed();
            }
        }

       



        protected virtual void RenderReadOnly()
        {
            object value = this.PropertyDescriptor.Value;
            string text = value != null ? value.ToString() : string.Empty;
            RenderLabel(text);


        }

        private void RenderLabel(string text)
        {
            TagBuilder label = new TagBuilder("Label");

            label.MergeAttributes(this.Attributes);
            label.SetInnerText(text);

            this.Write(label);

        }

        protected virtual void RenderHidden()
        {
            RenderLabel(String.Empty);
        }

        protected void RenderCollapsed()
        {

        }

    }

    public static class BusinessPropertyEditorBaseExtensions
    {
        public static TEditor  Property<TEditor>(this TEditor editor, string propertyName) where TEditor : BusinessPropertyEditorBase
        {
            editor.PropertyName = propertyName;
            return editor;
        }

        public static TEditor Property<TEditor, TModel, TValue>(this TEditor editor, Expression<Func<TModel, TValue>> expression) 
            where TEditor :  BusinessPropertyEditorBase
            where TModel : BusinessObject
        {
              string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(expression, (TModel)editor.Model.Object);
              editor.PropertyName = name;
              return editor;
             
        }
    }
}