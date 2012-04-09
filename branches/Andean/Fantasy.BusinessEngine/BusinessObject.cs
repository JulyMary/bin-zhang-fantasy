using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;
using System.Collections;
using System.Reflection;

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
                ObservableList<T> persisted = (ObservableList<T>)this.GetPersistedCollection(name);
                rs = new BusinessObjectCollection<T>(persisted);
                this._collections.Add(name, rs);
            }
            return (BusinessObjectCollection<T>)rs;
        }

        internal object GetPersistedCollection(string name)
        {
            object rs;

            if (!this._persistedCollections.TryGetValue(name, out rs))
            {
                PropertyInfo propInfo = this.GetType().GetProperty(name);
                Type elementType = propInfo.PropertyType.GetGenericArguments()[0];
                Type t = typeof(ObservableList<>).MakeGenericType(elementType);

                rs = Activator.CreateInstance(t);
                this._persistedCollections.Add(name, rs);
            }
            return rs;
        }

        internal void SetPersistedCollection(string name, object value)
        {
            this._persistedCollections[name] = value;
            object oc = this._collections.GetValueOrDefault(name);
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



    }
}
