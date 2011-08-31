using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Windows;


namespace Fantasy.Collection
{
    public class UnionedObservableCollection<T> : INotifyCollectionChanged, ICollection<T>, IWeakEventListener
    {

        private List<IEnumerable<T>> _childCollections = new List<IEnumerable<T>>();

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType == typeof(CollectionChangedEventManager))
            {
                this.ChildCollectionChanged(sender, (NotifyCollectionChangedEventArgs)e);
                return true;
            }
            return false;
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            _countChanged = true;
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }

        public void Union(IEnumerable<T> child)
        {
            int newIndex = this.Count;
            this._childCollections.Add(child);
            
            INotifyCollectionChanged n = child as INotifyCollectionChanged;
            if (n != null)
            {
                CollectionChangedEventManager.AddListener(n, this);
            }

            if (child.Count() > 0)
            {
                NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(child), newIndex);
                this.OnCollectionChanged(args);
            }


        }

        void ChildCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            int starting = 0;
            foreach (IEnumerable<T> child in _childCollections)
            {
                if (!object.Equals(child, sender))
                {
                    starting += child.Count();
                }
                else
                {
                    NotifyCollectionChangedEventArgs args;
                    switch (e.Action)
                    {
                        case NotifyCollectionChangedAction.Add:
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.NewItems, e.NewStartingIndex + starting);
                            break;
                        case NotifyCollectionChangedAction.Move:
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, e.OldItems, e.NewStartingIndex + starting, e.OldStartingIndex + starting);
                            break;
                        case NotifyCollectionChangedAction.Remove:
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, e.OldItems, e.OldStartingIndex + starting); 
                            break;
                        case NotifyCollectionChangedAction.Replace:
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.NewItems, e.OldItems, e.OldStartingIndex + starting);  
                            break;
                        case NotifyCollectionChangedAction.Reset:
                            args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset); 
                            break;
                        default:
                            throw new Exception("Abusrd!");
                    }

                    this.OnCollectionChanged(args);
                    break;
                }
            }
            
        }

        public void Division(IEnumerable<T> child)
        {

            int starting = 0;
            foreach (IEnumerable<T> items in _childCollections)
            {
                if (!object.Equals(items, child))
                {
                    starting += items.Count();
                }
                else
                {
                    NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(child), starting);

                    INotifyCollectionChanged n = child as INotifyCollectionChanged;
                    if (n != null)
                    {
                        CollectionChangedEventManager.RemoveListener(n, this);
                    }
                    this._childCollections.Remove(child);
                    this.OnCollectionChanged(args);
                    break;
                }
            }

        }


        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return (from items in _childCollections
                    from item in items
                    select item).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            foreach (object c in this._childCollections)
            {
                INotifyCollectionChanged n = c as INotifyCollectionChanged;
                if (n != null)
                {
                    CollectionChangedEventManager.RemoveListener(n, this);
                }
            }
            this._childCollections.Clear();
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool Contains(T item)
        {
            foreach (IEnumerable<T> items in this._childCollections)
            {
                if (items.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (T item in this)
            {
                if (arrayIndex < array.Length)
                {
                    array[arrayIndex] = item;
                }
                else
                {
                    break;
                }
            }
        }
        private int _count = 0;
        private bool _countChanged = false;
        public int Count
        {
            get 
            {

                if (_countChanged)
                {
                    _countChanged = false;

                    _count = 0;
                    if (this._childCollections.Count > 0)
                    {
                        _count = this._childCollections.Sum(c => c.Count());
                    }

                }
                return _count; 
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        #endregion




        
    }
}
