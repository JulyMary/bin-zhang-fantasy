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
using Fantasy.Web.EntityExtensions;
using System.Drawing;

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
            this.WriteObjectDefinition(selector);
            this.WriteObjectDataBinding(selector);

        }

        private void WriteObjectDataBinding(string selector)
        {
            string text = string.Format("$('{1}').be().applyBindings('{0}','{1}');", this.Object.Id, selector);
            this.Html.HtmlAssets().AddStartupScript(text);
        }


        private void WriteObjectDefinition(string selector)
        {
            string scriptId = "__businessobject_definition" + BusinessEngineContext.Current.Application.Data.Id .ToString() + this.Object.Id.ToString();
            
            if (!this.Html.HtmlAssets().ContainsStartupScript(scriptId))
            {

                JavaScriptSerializer ser = new JavaScriptSerializer();
                StringBuilder objText = new StringBuilder(1024);
                StringBuilder aclText = new StringBuilder(1024);
                var properDescs = from prop in this.Descriptor.Properties
                                  where prop.CanRead && prop.IsScalar
                                  select prop;
                foreach (BusinessPropertyDescriptor propDesc in properDescs)
                {
                    if (objText.Length > 0)
                    {
                        objText.Append(',');
                    }
                    object value = propDesc.Value;
                    if (value is BusinessObject)
                    {
                        BusinessObject oval = (BusinessObject)value;
                        objText.AppendFormat("{0} : $('{1}').be().shortcut({{Id: '{2}', Name : '{3}'}})", propDesc.CodeName,
                            selector, oval.Id, oval.Name);
                    }
                    else
                    {
                        objText.AppendFormat("{0} : {1}", propDesc.CodeName, JsonHelper.Serialize(value));
                    }

                    if (aclText.Length > 0)
                    {
                        aclText.Append(',');
                    }

                    Accessbility acc = propDesc.CanWrite ? Accessbility.Editable : Accessbility.Visible;

                    aclText.AppendFormat("{0} : '{1}'", propDesc.CodeName, acc.ToString().ToLower());

                  
                }

                if (aclText.Length > 0)
                {
                    aclText.AppendFormat(",deletable : {0}", this.Descriptor.CanDelete.ToString().ToLower()); 
                    objText.AppendFormat(", acl : {{{0}}}", aclText);
                    objText.AppendFormat(", entityState : '{0}'", this.Object.EntityState.ToString().ToLower());
                }

                string script = string.Format("$('{0}').be().renew({{{1}}});", selector, objText);
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

            
            BusinessPropertyDescriptor desc = render.Descriptor.Properties[propertyName];

            EditorExtension ext = desc.Entensions.OfType<EditorExtension>().FirstOrDefault();
            UserControlFactory rs;
            if (ext != null)
            {
                rs = ext.CreateEditor(render.Html);
            }
            else
            {
                rs = GetDefaultEditor(desc, render.Html);
            }

            rs.BindModel(render);
            if(rs.Control is BusinessPropertyEditorBase)
            {
                ((BusinessPropertyEditorBase)rs.Control).PropertyName = propertyName;
            }

            return rs;

           
        }


        private static UserControlFactory GetDefaultEditor(BusinessPropertyDescriptor desc, HtmlHelper html)
        {
            UserControlFactory rs = null;

            Type type = desc.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = type.GetGenericArguments()[0];
            }

            if (desc.IsScalar)
            {
                if (typeof(BusinessObject).IsAssignableFrom(type))
                {
                    rs = new UserControlFactory<BusinessObjectPropertyEditor>(html);
                }
                else if (typeof(Image).IsAssignableFrom(type))
                {

                }
                else if (type.IsEnum)
                {

                }
                else if (type == typeof(Guid))
                {
                    rs = new UserControlFactory<TextEditor>(html);
                }
                else
                {


                    switch (Type.GetTypeCode(desc.PropertyType))
                    {
                        case TypeCode.Boolean:
                            rs = new BooleanEditorFactory(html);
                            break;
                        case TypeCode.Byte:
                        case TypeCode.Char:
                        case TypeCode.Decimal:
                        case TypeCode.Double:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.SByte:
                        case TypeCode.Single:
                        case TypeCode.String:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                        case TypeCode.UInt64:
                            rs = new UserControlFactory<TextEditor>(html);
                            break;
                        case TypeCode.DateTime:
                            rs = new UserControlFactory<TextEditor>(html);
                            break;

                    }
                }
            }

            if (rs != null)
            {
                return rs;
            }
            else
            {
                throw new NotSupportedException();
            }
        }


        public static UserControlFactory EditorFor<TModel, TValue>(this BusinessObjectRender<TModel> render, Expression<Func<TModel, TValue>> property) 
            where TModel : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Editor(render, name);
        }

        public static UserControlFactory<HiddenEditor> Hidden<TModel>(this BusinessObjectRender<TModel> render, string propertyName)
            where TModel : BusinessObject
        {
            UserControlFactory<HiddenEditor> rs = new UserControlFactory<HiddenEditor>(render.Html);
            rs.BindModel(render);
            
            rs.Control.PropertyName = propertyName;

            return rs;
        }

        public static UserControlFactory<HiddenEditor> HiddenFor<TModel, TValue>(this BusinessObjectRender<TModel> render, Expression<Func<TModel, TValue>> property)
            where TModel : BusinessObject
        {
            string name = BusinessObjectMetaDataHelper.PropertyNameFromLambdaExpression(property, render.Object);
            return Hidden(render, name);
        }

    }
}