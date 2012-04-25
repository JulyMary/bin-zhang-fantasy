using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Security;
using System.Linq.Expressions;

namespace Fantasy.BusinessEngine.Security
{
    public static class SecurityExtensions
    {
        public static BusinessObjectSecurity ObjectSecurity(this BusinessObject obj)
        {
            throw new NotImplementedException();
        }


        public static BusinessObjectMemberSecurity PropertySecurity(this BusinessObject obj, string propertyCodeName)
        {
            throw new NotImplementedException();
        }

        public static BusinessObjectMemberSecurity PropertySecurity(this BusinessObject obj, BusinessProperty property)
        {
            throw new NotImplementedException();
        }


        public static BusinessObjectMemberSecurity PropertySecurity<TClass, TValue>(this BusinessObject obj, Expression<Func<TClass, TValue>> expression) where TClass : BusinessObject
        {
            throw new NotImplementedException();
        }

        public static BusinessObjectSecurity ClassSecurity(this BusinessClass @class)
        {
            throw new NotImplementedException();
        }

    }
}
