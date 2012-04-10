using System;
using System.Collections.Specialized;

using NHibernate.Engine;
using NHibernate.Collection.Generic;
using System.Collections.Generic;

namespace Fantasy.BusinessEngine.Collections
{
    /// <summary>
    /// Represents a persistent wrapper for a generic bag collection that fires events when the
    /// collection's contents have changed.
    /// </summary>
    /// <typeparam name="T">Type of item to be stored in the list.</typeparam>
    public class PersistentGenericObservableBag<T> : PersistentGenericBag<T>, IObservableList<T>, IObservableList, IList<T>
    {
        public PersistentGenericObservableBag(ISessionImplementor session, ObservableList<T> list) : base(session, list)
        {
        }

        public PersistentGenericObservableBag(ISessionImplementor session) : base(session)
        {
        }

        public void Add(T item)
        {
            base.Add(item);
            this.OnItemAdded(item, this.Count - 1);
        }

        public new void Clear()
        {
            base.Clear();
            this.OnCollectionReset();
        }

        public void Insert(int index, T item)
        {
            base.Insert(index, item);
            this.OnItemInserted(index, item);
        }

        public bool Remove(T item)
        {
            bool rs = false;
            int index = this.IndexOf(item);
            if (index >= 0)
            {

                base.Remove(item);
                this.OnItemRemoved(item, index);
                rs = true;
            }

            return rs;
        }

        public new T this[int index]
        {
            get
            {
                return (T)base[index];
            }
            set
            {

                object old = this[index];
                if (!Object.Equals(old, value))
                {
                    base[index] = value;
                    this.OnItemReplaced(value, old);

                }
            }
        }

        public new void RemoveAt(int index)
        {
            T item = (T)this[index];

            base.RemoveAt(index);
            this.OnItemRemoved(item, index);
        }

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has been 
        /// added to the end of the collection.
        /// </summary>
        /// <param name="item">Item added to the collection.</param>
        protected void OnItemAdded(T item, int index)
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, this.Count - 1));
            }
        }

        /// <summary>
        /// Fires the <see cref="CollectionChanged"/> event to indicate the collection 
        /// has been reset.  This is used when the collection has been cleared or 
        /// entirely replaced.
        /// </summary>
        protected void OnCollectionReset()
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Reset));
            }
        }

        /// <summary>
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has 
        /// been inserted into the collection at the specified index.
        /// </summary>
        /// <param name="index">Index the item has been inserted at.</param>
        /// <param name="item">Item inserted into the collection.</param>
        protected void OnItemInserted(int index, T item)
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Add, item, index));
            }
        }

        /// <summary>
        /// Fires the <see cref="CollectionChanged"/> event to indicate an item has
        /// been removed from the collection at the specified index.
        /// </summary>
        /// <param name="item">Item removed from the collection.</param>
        /// <param name="index">Index the item has been removed from.</param>
        protected void OnItemRemoved(T item, int index)
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, index));
            }
        }

        #endregion

        #region IList<T> Members

        int IList<T>.IndexOf(T item)
        {
            return base.IndexOf(item);
        }

        void IList<T>.Insert(int index, T item)
        {
            this.Insert(index, item);
            
        }

        void IList<T>.RemoveAt(int index)
        {
            T item = (T)this[index];
            this.RemoveAt(index);
           
        }

        T IList<T>.this[int index]
        {
            get
            {
                return (T)this[index];
            }
            set
            {

                this[index] = value;
            }
        }

        protected virtual void OnItemReplaced(object newItem, object oldItem)
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Replace, newItem, oldItem));
            }
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        void ICollection<T>.Clear()
        {
            this.Clear();
        }

        bool ICollection<T>.Remove(T item)
        {
            return this.Remove(item);
        }

        #endregion

        private bool _blockCollectionChanged = false;

        public void Swap(int x, int y)
        {



            int a = Math.Min(x, y);
            int b = Math.Max(x, y);

            T oa = (T)this[a];
            T ob = (T)this[b];
            this._blockCollectionChanged = true;
            try
            {
                this.RemoveAt(b);
                this.Insert(a, ob);
            }
            finally
            {
                this._blockCollectionChanged = false;
            }
            this.OnItemMoved(ob, a, b);

            int newA = this.IndexOf(oa);
            if (newA != b)
            {
                this._blockCollectionChanged = true;
                try
                {
                    this.RemoveAt(newA);
                    this.Insert(b, oa);
                }
                finally
                {
                    this._blockCollectionChanged = false;
                }
                this.OnItemMoved(oa, b, newA);
            }

        }

        protected virtual void OnItemMoved(T item, int index, int oldIndex)
        {
            if (this.CollectionChanged != null && !this._blockCollectionChanged)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Move, item, index, oldIndex));
            }
        }
    }
}
