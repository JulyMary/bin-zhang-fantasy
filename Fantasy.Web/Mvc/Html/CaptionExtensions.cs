using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Fantasy.BusinessEngine;
using System.Linq.Expressions;

namespace Fantasy.Web.Mvc.Html
{
    public static class CaptionExtensions
    {
        public static MvcHtmlString Caption<T>(this BusinessObjectRender<T> render, string propery, RouteValueDictionary htmlAttributes)
        {
            TagBuilder span = new TagBuilder("Label");
            if (htmlAttributes != null)
            {
                span.MergeAttributes(htmlAttributes);
            }
            string caption = render.Descriptor.Properties[propery].Name;

            span.SetInnerText(caption);
            return MvcHtmlString.Create(span.ToString());

        }

        public static MvcHtmlString Caption<T>(this BusinessObjectRender<T> render, string propery, object htmlAttributes = null)
        {
            return Caption(render, propery, htmlAttributes != null ? new RouteValueDictionary(htmlAttributes) : new RouteValueDictionary());
        }

        public static MvcHtmlString CaptionFor<T, TValue>(this BusinessObjectRender<T> render, Expression<Func<T, TValue>> property, RouteValueDictionary htmlAttributes) where T : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Caption(render, name, htmlAttributes);
        }


        public static MvcHtmlString CaptionFor<T, TValue>(this BusinessObjectRender<T> render, Expression<Func<T, TValue>> property, object htmlAttributes = null) where T : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Caption(render, name);
        }

    }
}