using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Fantasy.BusinessEngine.Collections
{
    public class ObservableListView<T> : IObservableList<T>
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

        #region IObservableList<T> Members

        public void Swap(int x, int y)
        {
            this._source.Swap(x, y);
        }

        #endregion

        #region IList<T> Members

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

        #endregion

        #region ICollection<T> Members

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

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this._source.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region INotifyCollectionChanged Members

        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(this.CollectionChanged != null)
            {
                this.CollectionChanged(this, e);
            }
        }


        public event System.Collections.Specialized.NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion
    }
}
