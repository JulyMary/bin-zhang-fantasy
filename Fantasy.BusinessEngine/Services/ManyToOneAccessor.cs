using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Properties;
using System.Reflection;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Services
{
    public class ManyToOneAccessor : IPropertyAccessor
    {
       

        public bool CanAccessThroughReflectionOptimizer
        {
            get { return false; }
        }

        public IGetter GetGetter(Type theClass, string propertyName)
        {
            throw new NotImplementedException();
        }

        public ISetter GetSetter(Type theClass, string propertyName)
        {
            throw new NotImplementedException();
        }


        private class Getter : IGetter
        {

            public Getter(Type type, string propertyName)
            {
               
                this.PropertyName = propertyName;
                PropertyInfo propInfo = type.GetProperty(propertyName);
                this.ReturnType = typeof(ObservableList<>).MakeGenericType(propInfo.PropertyType);

            }

            #region IGetter Members

            public object Get(object target)
            {
                return ((BusinessObject)target).GetPersistedCollection(this.PropertyName);
            }

            public object GetForInsert(object owner, System.Collections.IDictionary mergeMap, NHibernate.Engine.ISessionImplementor session)
            {
                return this.Get(owner);
            }

            public System.Reflection.MethodInfo Method
            {
                get { return null; }
            }

            public string PropertyName { get; private set; }


            public Type ReturnType { get; private set; }
            

            #endregion
        }

        private class Setter : ISetter
        {

            public Setter(Type type, string propertyName)
            {
                this.PropertyName = propertyName;
            }

           

            public System.Reflection.MethodInfo Method
            {
                get { return null; }
            }

            public string PropertyName { get; private set; }
           

            public void Set(object target, object value)
            {
                ((BusinessObject)target).SetPersistedCollection(this.PropertyName, value);
            }

           
        }

    }
}
