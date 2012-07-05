using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fantasy.XSerialization;


namespace Fantasy.Jobs
{
    [XSerializable("stateBag", NamespaceUri=Consts.XNamespaceURI)]
    public class StateBag : IStateBag 
    {
        [XArray(Serializer=typeof(StateBagItemsSerializer))] 
        private List<StateBagItem> _items = new List<StateBagItem>();

        #region IStateBag Members

        public object this[string name]
        {
            get
            {
                int index = this.IndexOf(name);
                return index >= 0 ? _items[index].Value : null;
               
            }
            set
            {
                int index = this.IndexOf(name);
                if (index >= 0)
                {
                    _items[index].Value = value;
                }
                else
                {
                    this._items.Insert(~index, new StateBagItem() { Name = name, Value = value });
                }
            }
        }

        public T GetValue<T>(string name, T defaultValue)
        {
            int index = this.IndexOf(name);
            if (index >= 0)
            {
                return (T)this._items[index].Value;
            }
            else
            {
                this[name] = defaultValue; 
                return defaultValue; 
            }
        }

        private int IndexOf(string name)
        {
            int rs = _items.BinarySearchBy(name, item => item.Name, StringComparer.OrdinalIgnoreCase);
            return rs;
        }

        public int Count
        {
            get { return this._items.Count; }
        }

        public string[] Names
        {
            get { return (from state in this._items select state.Name).ToArray(); }
        }

        public void Remove(string name)
        {
            int index = this.IndexOf(name);
            if (index >= 0)
            {
                this._items.RemoveAt(index);
            }
        }

        public void Clear()
        {
            this._items.Clear();
        }

        #endregion

        #region IEnumerable<StateItem> Members

        public IEnumerator<StateBagItem> GetEnumerator()
        {
            return this._items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IStateBag Members


        public bool ContainsState(string name)
        {
            return this.IndexOf(name) >= 0;
        }

        #endregion
    }
}
