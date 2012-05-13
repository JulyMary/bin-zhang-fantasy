using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fantasy.BusinessEngine;
using Fantasy.BusinessEngine.Services;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Text;

namespace Fantasy.Web.Mvc.Html
{
    public class BusinessObjectRender<T> where T : BusinessObject
    {
        public BusinessObjectRender(HtmlHelper htmlHelper, T obj, string selector ) 
        {
            this.Html = htmlHelper;
            this.Object = obj;
            this.Descriptor = new BusinessObjectDescriptor(obj);
            this.Accessbility = Accessbility.Collapsed;
            this.WriteObjectDefinition();
            this.WriteObjectDataBinding(selector);

        }

        private void WriteObjectDataBinding(string selector)
        {
            string text = string.Format("be.applyBindings('{0}','{1}');", this.Object.Id, selector);
            this.Html.HtmlAssets().AddStartupScript(text);
        }


        private void WriteObjectDefinition()
        {
            string scriptId = "__businessobject_definition" + this.Object.Id.ToString();
            if (!this.Html.HtmlAssets().ContainsStartupScript(scriptId))
            {

                JavaScriptSerializer ser = new JavaScriptSerializer();
                StringBuilder text = new StringBuilder(1024);
                var properDescs = from prop in this.Descriptor.Properties
                                  where prop.CanRead && prop.MemberType == BusinessObjectMemberTypes.Property
                                  select prop;
                foreach (BusinessPropertyDescriptor propDesc in properDescs)
                {
                    if (text.Length > 0)
                    {
                        text.Append(',');
                    }
                    object value = propDesc.Value;
                    if (value is BusinessObject)
                    {
                        BusinessObject oval = (BusinessObject)value;
                        text.AppendFormat("{0} : be.shortcut({{Id: {1}, Name : '{2}'}})", propDesc.CodeName,
                            oval.Id, oval.Name);


                    }
                    else
                    {
                        text.AppendFormat("{0} : {1}", propDesc.CodeName, JsonHelper.Serialize(value));
                    }

                    Accessbility acc = propDesc.CanWrite ? Accessbility.Editable : Accessbility.Visible;

                    text.AppendFormat(",{0}Accessbility : '{1}'", propDesc.CodeName, acc.ToString().ToLower());
                  
                }

                string script = string.Format("be.renew({{{0}}});", text);
                this.Html.HtmlAssets().AddStartupScript(script,scriptId);
            }
        }

        public HtmlHelper Html { get; private set; }

        public T Object { get; private set; }

        public BusinessObjectDescriptor Descriptor { get; private set; }

        public Accessbility Accessbility { get; private set; }

    }

    public static class BusinessObjectRenderExtensions
    {
        public static BusinessObjectRender<T> BusinessObjectRender<T>(this HtmlHelper htmlHelper, T obj, string containerId) where T : BusinessObject
        {
            return new BusinessObjectRender<T>(htmlHelper, obj, containerId);
        }

        public static BusinessObjectRender<T> BusinessObjectRender<T>(this HtmlHelper htmlHelper, Guid id, string containerId) where T : BusinessObject
        {
            T obj = BusinessEngineContext.Current.GetRequiredService<IEntityService>().Get<T>(id);
            return BusinessObjectRender<T>(htmlHelper, obj, containerId);
        }

        public static UserControlFactory Editor<TModel>(this BusinessObjectRender<TModel> render, string propertyName)
            where TModel : BusinessObject
        {
            UserControlFactory<TextEditor> rs = new UserControlFactory<TextEditor>(render.Html);
            rs.BindModel(render);
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