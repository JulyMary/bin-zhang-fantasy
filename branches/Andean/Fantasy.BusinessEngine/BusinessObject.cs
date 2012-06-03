using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using System.Collections;
using System.Reflection;
using Fantasy.BusinessEngine.Services;

namespace Fantasy.BusinessEngine
{
    public class BusinessObject : BusinessEntity
    {

        private Dictionary<string, object> _collections = new Dictionary<string, object>();
        private Dictionary<string, object> _persistedCollections = new Dictionary<string, object>();

        protected virtual BusinessObjectCollection<T> GetCollection<T>(string name) where T: BusinessObject
        {
            object rs;
            if(!this._collections.TryGetValue(name, out rs))
            {
                IObservableList<T> persisted = (IObservableList<T>)GetPersistedCollection(this, name, typeof(T));
                rs = new BusinessObjectCollection<T>(persisted);
                this._collections.Add(name, rs);
            }
            return (BusinessObjectCollection<T>)rs;

           
        }

        internal static object GetPersistedCollection(BusinessObject obj, string name, Type elementType)
        {
            object rs;

            if (!obj._persistedCollections.TryGetValue(name, out rs))
            {
                
                Type t = typeof(ObservableList<>).MakeGenericType(elementType);

                rs = Activator.CreateInstance(t);
                obj._persistedCollections.Add(name, rs);
            }
            return rs;
        }

        internal static void SetPersistedCollection(BusinessObject obj, string name, object value)
        {
            obj._persistedCollections[name] = value;
            object oc = obj._collections.GetValueOrDefault(name);
            if (oc != null)
            {
                ((IObservableListView)oc).Source = (IObservableList)value;
            }
           
        }


        protected virtual T GetManyToOneValue<T>(string name) where T: BusinessObject
        {
            BusinessObjectCollection<T> col = this.GetCollection<T>(name);
            return col.FirstOrDefault();
 
        }

        protected virtual bool SetManyToOneValue<T>(string name, T value) where T: BusinessObject
        {
            bool rs = false;
            BusinessObjectCollection<T> col = this.GetCollection<T>(name);
            T oldValue = col.FirstOrDefault();
            if (oldValue != value)
            {
                EntityPropertyChangingEventArgs e = new EntityPropertyChangingEventArgs(this, name, value, oldValue);
                this.OnPropertyChanging(e);
                if (!e.Cancel)
                {
                    rs = true;
                    col.Clear();
                    col.Add(value);

                    this.OnPropertyChanged(new EntityPropertyChangedEventArgs(this, name, value, oldValue));
                }
            }

            return rs;
        }


        public virtual Guid ClassId
        {
            get
            {
                return (Guid)this.GetValue("ClassId", Guid.Empty);
            }
            set
            {
                this.SetValue("ClassId", value);
            }
        }

        public virtual string Name
        {
            get
            {
                return (string)this.GetValue("Name", null);
            }
            set
            {
                this.SetValue("Name", value);
            }
        }


        public virtual long DisplayOrder
        {
            get
            {
                return (long)this.GetValue("DisplayOrder", null);
            }
            set
            {
                this.SetValue("DisplayOrder", value);
            }
        }


        public virtual Guid Creator
        {
            get
            {
                return (Guid)this.GetValue("Creator", null);
            }
            set
            {
                this.SetValue("Creator", value);
            }
        }


        public virtual string IconKey
        {

            get
            {
                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                BusinessClass @class = oms.FindBusinessClass(this.ClassId);
               
                return oms.GetImageKey(@class);
            }
        }

       

        public virtual void Append(string propertyCodeName, BusinessObject other)
        {
            BusinessObjectDescriptor desc = new BusinessObjectDescriptor(this);
            BusinessPropertyDescriptor propDesc = desc.Properties[propertyCodeName];

            PropertyInfo pi = this.GetType().GetProperty(propertyCodeName);
            
            switch (propDesc.MemberType)
            {
                case BusinessObjectMemberTypes.Property:
                    {
                        pi.SetValue(this, other, null);
                    }
                    break;
                case BusinessObjectMemberTypes.LeftAssociation:
                    {
                        if (propDesc.IsScalar)
                        {
                            pi.SetValue(this, other, null);
                        }
                        else
                        {
                            IList col = (IList)pi.GetValue(this, null);
                            if (col.IndexOf(other) < 0)
                            {
                                col.Add(other);
                            }
                        }

                        if (propDesc.Association.LeftNavigatable)
                        {
                            PropertyInfo otherPi = other.GetType().GetProperty(propDesc.Association.LeftRoleCode);
                            if ((new Cardinality(propDesc.Association.LeftCardinality)).IsScalar)
                            {
                                otherPi.SetValue(other, this, null);
                            }
                            else
                            {
                                IList col = (IList)otherPi.GetValue(other, null);
                                if (col.IndexOf(this) < 0)
                                {
                                    col.Add(this);
                                }
                            }
                        }
                    }
                    break;
                case BusinessObjectMemberTypes.RightAssociation:
                    {
                        if (propDesc.IsScalar)
                        {
                            pi.SetValue(this, other, null);
                        }
                        else
                        {
                            IList col = (IList)pi.GetValue(this, null);
                            if (col.IndexOf(other) < 0)
                            {
                                col.Add(other);
                            }
                        }

                        if (propDesc.Association.RightNavigatable)
                        {
                            PropertyInfo otherPi = other.GetType().GetProperty(propDesc.Association.RightRoleCode);
                            if ((new Cardinality(propDesc.Association.RightCardinality)).IsScalar)
                            {
                                otherPi.SetValue(other, this, null);
                            }
                            else
                            {
                                IList col = (IList)otherPi.GetValue(other, null);
                                if (col.IndexOf(this) < 0)
                                {
                                    col.Add(this);
                                }
                            }
                        }
                    }
                    break;
               
            }
        }

        public virtual void Remove(string propertyCodeName, BusinessObject other)
        {
            BusinessObjectDescriptor desc = new BusinessObjectDescriptor(this);
            BusinessPropertyDescriptor propDesc = desc.Properties[propertyCodeName];

            PropertyInfo pi = this.GetType().GetProperty(propertyCodeName);

            switch (propDesc.MemberType)
            {
                case BusinessObjectMemberTypes.Property:
                    {
                        pi.SetValue(this, null, null);
                    }
                    break;
                case BusinessObjectMemberTypes.LeftAssociation:
                    {
                        if (propDesc.IsScalar)
                        {
                            pi.SetValue(this, null, null);
                        }
                        else
                        {
                            IList col = (IList)pi.GetValue(this, null);
                            col.Remove(other);
                        }

                        if (propDesc.Association.LeftNavigatable)
                        {
                            PropertyInfo otherPi = other.GetType().GetProperty(propDesc.Association.LeftRoleCode);
                            if ((new Cardinality(propDesc.Association.LeftCardinality)).IsScalar)
                            {
                                otherPi.SetValue(other, null, null);
                            }
                            else
                            {
                                IList col = (IList)otherPi.GetValue(other, null);
                                col.Remove(this);
                            }
                        }
                    }
                    break;
                case BusinessObjectMemberTypes.RightAssociation:
                    {
                        if (propDesc.IsScalar)
                        {
                            pi.SetValue(this, null, null);
                        }
                        else
                        {
                            IList col = (IList)pi.GetValue(this, null);
                            col.Remove(other);
                        }

                        if (propDesc.Association.RightNavigatable)
                        {
                            PropertyInfo otherPi = other.GetType().GetProperty(propDesc.Association.RightRoleCode);
                            if ((new Cardinality(propDesc.Association.RightCardinality)).IsScalar)
                            {
                                otherPi.SetValue(other, null, null);
                            }
                            else
                            {
                                IList col = (IList)otherPi.GetValue(other, null);
                                col.Remove(this);
                            }
                        }
                    }
                    break;

            }
        }


    }
}
