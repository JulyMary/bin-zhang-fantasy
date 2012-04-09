using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;

namespace Fantasy.BusinessEngine.Collections
{
    public class ObservableListView<T> : IObservableListView<T>, IObservableListView, IObservableList<T>, IObservableList
    {

        private IObservableList<T> _source;

        public IObservableList<T> Source
        {
            get { return _source; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (_source != null)
                {
                    _source.CollectionChanged -= new NotifyCollectionChangedEventHandler(SourceCollectionChanged);
                }

                _source = value;
                _source.CollectionChanged += new NotifyCollectionChangedEventHandler(SourceCollectionChanged);

                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); 
                
            }
        }

        void SourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }


        public ObservableListView(IObservableList<T> initialSource)
        {
            this.Source = initialSource;
        }


        public void Swap(int x, int y)
        {
            this._source.Swap(x, y);
        }

      

        public int IndexOf(T item)
        {
            return this._source.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this._source.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            this._source.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return this._source[index];
            }
            set
            {
                this._source[index] = value;
            }
        }

       

        public void Add(T item)
        {
            this._source.Add(item);
        }

        public void Clear()
        {
            this._source.Clear();
        }

        public bool Contains(T item)
        {
            return this._source.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this._source.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._source.Count; }
        }

        public bool IsReadOnly
        {
            get { return this._source.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            return this._source.Remove(item);
        }

       

        public IEnumerator<T> GetEnumerator()
        {
            return this._source.GetEnumerator();
        }

       

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

      

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }


        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

      

        int IList.Add(object value)
        {
            this.Add((T)value);
            return this.Count - 1;
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
            get { return false; }
        }

        void IList.Remove(object value)
        {
            this.Remove((T)value);
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return (T)this[index];
            }
            set
            {
                this[index] = (T)value;
            }
        }

      

        void ICollection.CopyTo(Array array, int index)
        {

            int copyLenth = Math.Min(array.Length - index, this.Count);

            for (int i = 0; i < copyLenth; i++)
            {
                array.SetValue(this[i], index + i);
            }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }


        private object _syncRoot = new object();

        object ICollection.SyncRoot
        {
            get { return _syncRoot; }
        }

       

        IObservableList IObservableListView.Source
        {
            get
            {
                return (IObservableList)this.Source;
            }
            set
            {
                this.Source = (IObservableList<T>)value;
            }
        }

      

      

       
       
    }
}
