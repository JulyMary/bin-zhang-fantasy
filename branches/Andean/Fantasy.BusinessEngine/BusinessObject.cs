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


        public virtual string IconKey
        {

            get
            {
                IObjectModelService oms = BusinessEngineContext.Current.GetRequiredService<IObjectModelService>();
                BusinessClass @class = oms.FindBusinessClass(this.ClassId);
               
                return oms.GetImageKey(@class);
            }
        }



    }
}
