using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using Fantasy.BusinessEngine.Properties;


namespace Fantasy.BusinessEngine
{
    public static class BusinessObjectMetaDataHelper
    {
        public static string PropertyNameFromLambdaExpression<TParameter, TValue>(Expression<Func<TParameter, TValue>> expression, TParameter owner) where TParameter : BusinessObject
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (owner == null)
            {
                throw new ArgumentNullException("container");
            }
            string propertyName = null;
            Type containerType = null;
            bool flag = false;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    {
                        MemberExpression body = (MemberExpression)expression.Body;
                        propertyName = (body.Member is PropertyInfo) ? body.Member.Name : null;
                        containerType = body.Expression.Type;
                        flag = true;
                        break;
                    }

            }
            if (!flag)
            {
                throw new InvalidOperationException(Resources.TemplateLimitations);
            }

            return propertyName;


        }

    }
}
