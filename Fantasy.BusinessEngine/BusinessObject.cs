using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.BusinessEngine.Collections;

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
                ObservableList<T> persisted = this.GetPersistedCollection<T>(name);
                rs = new BusinessObjectCollection<T>(persisted);
                this._collections.Add(name, rs);
            }
            return (BusinessObjectCollection<T>)rs;
        }

        protected virtual ObservableList<T> GetPersistedCollection<T>(string name) where T : BusinessObject
        {
            object rs;
            if (!this._persistedCollections.TryGetValue(name, out rs))
            {
                rs = new ObservableList<T>();
                this._persistedCollections.Add(name, rs);
            }
            return (ObservableList<T>)rs;
        }

        protected virtual void SetPersistedCollection<T>(string name, ObservableList<T> value) where T : BusinessObject
        {
            this._persistedCollections[name] = value;
            object oc = this._collections.GetValueOrDefault(name);
            if (oc != null)
            {
                ((BusinessObjectCollection<T>)oc).Source = value;
            }
           
        }

    }
}
