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
    public class PersistentGenericObservableBag<T> : PersistentGenericBag<T>, IObservableList<T>, IList<T>
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
            this.OnItemAdded(item);
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
        protected void OnItemAdded(T item)
        {
            if (this.CollectionChanged != null)
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
            if (this.CollectionChanged != null)
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
            if (this.CollectionChanged != null)
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
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(
                    NotifyCollectionChangedAction.Remove, item, index));
            }
        }

        #endregion

        #region IList<T> Members

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
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

      
    }
}
