using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using NHibernate.UserTypes;

namespace Fantasy.BusinessEngine.Collections
{
    public class ObservableSet<T> : IObservableSet<T>, IUserCollectionType
    {

        private SortedSet<T> _set = new SortedSet<T>();

        #region ISet<T> Members

        public bool Add(T item)
        {
            bool rs = this._set.Add(item);
            if (rs)
            {
                this.OnItemAdded(item);
            }
            return rs;
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
               this.Remove(item); 
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            foreach (T item in new List<T>(this._set))
            {
                if (! other.Contains(item))
                {
                    this.Remove(item);
                }
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this._set.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this._set.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this._set.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return this._set.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return this._set.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            IEnumerable<T> union = this._set.Union(other).ToList(); ;
            IEnumerable<T> intersect = this._set.Intersect(other).ToList();
            foreach (T item in union.Except(intersect))
            {
                this.Remove(item);
            }
            

        }

        public void UnionWith(IEnumerable<T> other)
        {
            foreach (T item in other)
            {
                this.Add(item);
            }
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }

        public void Clear()
        {
            if (this._set.Count > 0)
            {
                this._set.Clear();
                this.OnCollectionReset();
            }
        }

        public bool Contains(T item)
        {
            return this._set.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._set.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._set.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        private int IndexOf(T item)
        {
            int index = 0;
            foreach (T i in this._set)
            {
                if (Object.Equals(item, i))
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            bool rs = this._set.Remove(item);
            if (rs)
            {
                this.OnItemRemoved(item, index);
            }
            return rs;
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _set.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

       
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



        #region IUserCollectionType Members

        public bool Contains(object collection, object entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable GetElements(object collection)
        {
            throw new NotImplementedException();
        }

        public object IndexOf(object collection, object entity)
        {
            throw new NotImplementedException();
        }

        public object Instantiate(int anticipatedSize)
        {
            throw new NotImplementedException();
        }

        public NHibernate.Collection.IPersistentCollection Instantiate(NHibernate.Engine.ISessionImplementor session, NHibernate.Persister.Collection.ICollectionPersister persister)
        {
            throw new NotImplementedException();
        }

        public object ReplaceElements(object original, object target, NHibernate.Persister.Collection.ICollectionPersister persister, object owner, IDictionary copyCache, NHibernate.Engine.ISessionImplementor session)
        {
            throw new NotImplementedException();
        }

        public NHibernate.Collection.IPersistentCollection Wrap(NHibernate.Engine.ISessionImplementor session, object collection)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
