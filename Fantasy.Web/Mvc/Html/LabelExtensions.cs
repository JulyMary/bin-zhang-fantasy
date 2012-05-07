using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fantasy.BusinessEngine;
using System.Linq.Expressions;
using System.Web.Routing;

namespace Fantasy.Web.Mvc.Html
{
    public static class LabelExtensions
    {
        public static MvcHtmlString Label<T>(this BusinessObjectRender<T> render, string propery, RouteValueDictionary htmlAttributes)
        {
            BusinessPropertyDescriptor desc = render.Descriptor.Properties[propery];

            if (desc.CanRead == false)
            {
                return MvcHtmlString.Empty;
            }
            else
            {
                object value = render.Descriptor.Properties[propery].Value;
                string text = value != null ? value.ToString() : string.Empty;
                TagBuilder span = new TagBuilder("label");
                if (htmlAttributes != null)
                {
                    span.MergeAttributes(htmlAttributes);
                }
                span.SetInnerText(text);
                return MvcHtmlString.Create(span.ToString());
            }

        }

      

        public static MvcHtmlString Label<T>(this BusinessObjectRender<T> render, string propery, object htmlAttributes = null)
        {
            return Label(render, propery, htmlAttributes != null ? new RouteValueDictionary(htmlAttributes) : new RouteValueDictionary());
        }

        public static MvcHtmlString LabelFor<T, TValue>(this BusinessObjectRender<T> render, Expression<Func<T, TValue>> property, RouteValueDictionary htmlAttributes) where T : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Label(render, name, htmlAttributes);
        }


        public static MvcHtmlString LabelFor<T, TValue>(this BusinessObjectRender<T> render, Expression<Func<T, TValue>> property, object htmlAttributes = null) where T : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Label(render, name);
        }
    }
}