﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Properties;
using System.Reflection;
using Fantasy.BusinessEngine.Collections;

namespace Fantasy.BusinessEngine.Services
{
    public class ManyToManyAccessor : IPropertyAccessor
    {
       

        public bool CanAccessThroughReflectionOptimizer
        {
            get { return false; }
        }

        public IGetter GetGetter(Type theClass, string propertyName)
        {
            return new Getter(theClass, propertyName);
        }

        public ISetter GetSetter(Type theClass, string propertyName)
        {
            return new Setter(theClass, propertyName);
        }

        private class Getter : IGetter
        {
            public Getter(Type type, string propertyName)
            {
                
                this.PropertyName = propertyName;
                PropertyInfo propInfo = type.GetProperty(propertyName);
                if (propInfo == null)
                {
                    throw new MissingMemberException(type.AssemblyQualifiedName, propertyName);
                }

                this._elementType = propInfo.PropertyType.GetGenericArguments()[0];
                this.ReturnType = typeof(ObservableList<>).MakeGenericType(this._elementType);

            }

            public object Get(object target)
            {
                return BusinessObject.GetPersistedCollection((BusinessObject)target, this.PropertyName, this._elementType);
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

           
            private Type _elementType;

            public Type ReturnType
            {
                get;private set;
            }

        }

        private class Setter : ISetter
        {
            public Setter(Type type, string propertyName)
            {
                this.PropertyName = propertyName;
            }

           

            public MethodInfo Method
            {
                get { return null; }
            }

            public string PropertyName { get; private set; }
           

            public void Set(object target, object value)
            {
                BusinessObject.SetPersistedCollection((BusinessObject)target, this.PropertyName, value);
            }
        }

    }
}