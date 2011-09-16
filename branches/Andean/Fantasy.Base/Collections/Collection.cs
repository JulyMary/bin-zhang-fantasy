using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Fantasy
{

    public class CollectionEventArgs<T> : EventArgs
    {
        public CollectionEventArgs(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }
        public int Index { get; private set; }
        public T Value { get; private set; }
    }

    public class CollectionSetEventArgs<T> : EventArgs
    {
        public CollectionSetEventArgs(int index, T oldValue, T newValue)
        {
            this.Index = index;
            this.OldValue = oldValue;
            this.NewValue = NewValue;
        }
        public int Index { get; private set; }
        public T OldValue { get; private set; }
        public T NewValue { get; private set; }
    }

    public class Collection<T> : IList<T>, ICollection<T>,
    IEnumerable<T>, IList, ICollection, IEnumerable
    {
        public Collection()
        {
            this._list = new List<T>();
        }

        public Collection(IEnumerable<T> collection)
        {
            this._list = new List<T>(collection);
        }

        public Collection(int capacity)
        {
            this._list = new List<T>(capacity);
        }

        private List<T> _list;

        internal List<T> InnerList
        {
            get
            {
                return _list;
            }
        }

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.OnInserting(index, item);
            this._list.Insert(index, item);
            this.OnInserted(index, item); 
           
        }

        public event EventHandler<CollectionEventArgs<T>> Inserting;   


        protected virtual void OnInserting(int index, T value)
        {
            if (Inserting != null)
            {
                Inserting(this, new CollectionEventArgs<T>(index, value));
            }
        }

        public event EventHandler<CollectionEventArgs<T>> Inserted; 

        protected virtual void OnInserted(int index, T value)
        {
            if (Inserted != null)
            {
                Inserted(this, new CollectionEventArgs<T>(index, value));
            }
        }




        public void RemoveAt(int index)
        {
            T item = this._list[index];
            this.OnRemoving(index, item);
            this._list.RemoveAt(index);
            this.OnRemoved(index, item); 
        }

        public event EventHandler<CollectionEventArgs<T>> Removing;

        public event EventHandler<CollectionEventArgs<T>> Removed; 

        protected virtual void OnRemoving(int index, T value)
        {
            if (Removing != null)
            {
                Removing(this, new CollectionEventArgs<T>(index, value));
            }
        }

        protected virtual void OnRemoved(int index, T value)
        {
            if (Removed != null)
            {
                Removed(this, new CollectionEventArgs<T>(index, value));
            }
        }

        public T this[int index]
        {
            get
            {
                return _list[index];
            }
            set
            {
                T old = this._list[index];
                this.OnSetting(index, old, value);
                this._list[index] = value;
                this.OnSet(index, old, value);
            }
        }

        public event EventHandler<CollectionSetEventArgs<T>> Setting;

        public event EventHandler<CollectionSetEventArgs<T>> Set;

        protected virtual void OnSetting(int index, T oldValue, T newValue)
        {
            if (Setting != null)
            {
                Setting(this, new CollectionSetEventArgs<T>(index, oldValue, newValue));
            }
        }

        protected virtual void OnSet(int index, T oldValue, T newValue)
        {
            if (Set != null)
            {
                Set(this, new CollectionSetEventArgs<T>(index, oldValue, newValue));
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            this.Insert(this.Count, item);
        }

        public event EventHandler Clearing;
        protected virtual void OnClearing()
        {
            if (this.Clearing != null)
            {
                this.Clearing(this, EventArgs.Empty);
            }
        }

        public event EventHandler Cleared;
        protected virtual void OnCleared()
        {
            if (this.Cleared != null)
            {
                this.Cleared(this, EventArgs.Empty);
            }
        }

        public void Clear()
        {
            this.OnClearing();
            this._list.Clear();
            this.OnCleared(); 
        }

        public bool Contains(T item)
        {
            return this._list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IList)_list).IsReadOnly; }
        }

       

        public bool Remove(T item)
        {
            int index = this._list.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }
            else
            {
                return false;
            }
 
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this._list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion


        #region IList Members

        int IList.Add(object value)
        {
            this.Add((T)value);
            return this.Count - 1;
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        bool IList.IsFixedSize
        {
            get { return ((IList)_list).IsFixedSize; }
        }

        bool IList.IsReadOnly
        {
            get { return ((IList)_list).IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            ((IList)_list).CopyTo(array, index);
        }

       

        bool ICollection.IsSynchronized
        {
            get { return ((IList)_list).IsSynchronized; }
        }

        object ICollection.SyncRoot
        {
            get { return ((IList)_list).SyncRoot; }
        }

        #endregion

        public int BinarySearch(T item)
        {
            return _list.BinarySearch(item);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return _list.BinarySearch(index, count, item, comparer);
        }

        public void Sort()
        {
            this._list.Sort();
        }

        public void Sort(IComparer<T> comparer)
        {
            this._list.Sort(comparer); 
        }

        public void Sort(Comparison<T> comparison)
        {
            this._list.Sort(comparison); 
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            this._list.Sort(index, count, comparer);
        }


    }
}
