﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;

namespace Fantasy.Web.Mvc.Html
{
    public class BusinessObjectRender<T> where T : BusinessObject
    {
        public BusinessObjectRender(HtmlHelper htmlHelper, T obj ) 
        {
            this.Html = Html;
            this.Object = obj;
            BusinessObjectDescriptor descriptor = new BusinessObjectDescriptor(obj);
            this.InvisibleOption = InvisibleOptions.Collapsed;

        }

        public HtmlHelper Html { get; private set; }

        public T Object { get; private set; }

        public BusinessObjectDescriptor Descriptor { get; private set; }

        public InvisibleOptions InvisibleOption { get; private set; }
        
    }


    public static class BusinessObjectRenderExtensions
    {
        public static BusinessObjectRender<T> BusinessObjectRender<T>(this HtmlHelper htmlHelper, T obj) where T : BusinessObject
        {
            return new BusinessObjectRender<T>(htmlHelper, obj);
        }

        public static BusinessObjectRender<T> BusinessObjectRender<T>(this HtmlHelper htmlHelper, Guid id) where T : BusinessObject
        {
            T obj = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<T>(id);
            return BusinessObjectRender<T>(htmlHelper, obj);
        }

        public static UserControlFactory Editor<TModel>(this BusinessObjectRender<TModel> render, string propertyName)
            where TModel : BusinessObject
        {
            UserControlFactory<TextEditor> rs = new UserControlFactory<TextEditor>(render.Html);
            rs.BindModel<UserControlFactory<TextEditor>, TextEditor>(render);
            rs.Control.PropertyName = propertyName;

            return rs;
        }

        public static UserControlFactory EditorFor<TModel, TValue>(this BusinessObjectRender<TModel> render, Expression<Func<TModel, TValue>> property) 
            where TModel : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Editor(render, name);
        }

    }
}