using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;

namespace Fantasy.Web.Mvc.Html
{
    public class BusinessPropertyEditorBase : UserControl<BusinessObjectRender<BusinessObject>>
    {




        public string PropertyName { get; internal set; }

        


        public override void Render(System.IO.TextWriter writer)
        {
            BusinessPropertyDescriptor desc = this.Model2.Descriptor.Properties[this.PropertyName];

            if(desc.CanRead 


            base.Render(writer);
        }
    }


    public static class BusinessPropertyEditorBaseExtensions
    {
        public T Property<T>(this T editor, string propertyName) where T : BusinessPropertyEditorBase
        {
            editor.PropertyName = propertyName;
            return editor;
        }
    }
}